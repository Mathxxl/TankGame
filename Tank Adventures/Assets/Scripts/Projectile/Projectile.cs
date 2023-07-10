using System;
using System.Collections;
using System.Linq;
using Entities;
using Entities.Entity_Systems;
using Entities.Entity_Systems.Weapons;
using Interfaces;
using Physics.Explosion;
using UnityEngine;
using UnityEngine.Serialization;

namespace Projectile
{
    /// <summary>
    /// Describes in-game basic projectiles
    /// </summary>
    public class Projectile : Subweapon, IDestructible
    {
        #region Attributes

        [SerializeField][Range(0f, 1000)] protected float damagesMultiplier = 1.0f; //Multiply the damages of the weapon
        [SerializeField][Range(0f, 100000)] protected float speed = 1.0f; //Speed movement of the projectile
        [SerializeField][Range(0f, 100)] protected float lifeDuration = 1.0f; //life time in seconds
        [SerializeField][Range(0.01f, 1000)] protected float size = 1.0f;
        [SerializeField] protected GameObject explosionPrefab;

        protected Transform ThisTransform;
        protected Rigidbody Rb;
        private string _selfTag; //tag of the entity having this weapon
        private float _initSpeed;
        
        #endregion

        #region Properties

        public ProjectilePool LinkedPool; //Projectile Pool it belongs to
        public Weapon weapon;
        public float Multiplier => damagesMultiplier;

        public float Size
        {
            get => size;
            set
            {
                size = value;
                SetSizeObject();
            }
        }

        #endregion


        #region Methods

        #region Public Methods

        //Release the projectile if linked to a pool, else destroy it
        public virtual void DestroyObject()
        {
            if (LinkedPool != null)
            {
                LinkedPool.Pool.Release(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region Mono Behaviours

        protected virtual void Awake()
        {
            ThisTransform = transform;
            Rb ??= GetComponent<Rigidbody>();
            SetSizeObject();
            _initSpeed = speed;
        }

        protected virtual void OnEnable()
        {
            if (weapon != null)
            {
                //Add initial velocity
                if (weapon.ThisEntity.TryGetComponent(out Rigidbody entityRb))
                {
                    speed += entityRb.velocity.magnitude;
                }
            }
            
            Move(); 
            StartCoroutine(LifeControl());
        }

        protected virtual void OnDisable()
        {
            StopCoroutine(LifeControl());
            speed = _initSpeed;
        }
        
        protected virtual void Start()
        {
            if (weapon != null)
            {
                //Set Attributes from weapon
                
                lifeDuration = weapon.Range / speed;
            }
            else
            {
                Debug.LogWarning($"No weapon found for Projectile {name}");
            }
        
        }

        #endregion
        
        #region Protected Methods

        //Move by adding foward speed to its Rigidbody
        public virtual void Move()
        {
            Rb.velocity =  speed * transform.forward;
        }

        //Called just before destroying the object
        protected virtual void Explosion()
        {
            //Animation and force
            if (explosionPrefab != null)
            {
                var thisTransform = transform;
                var explosion = Instantiate(explosionPrefab, thisTransform.position, thisTransform.rotation);
                explosion.GetComponent<Explosion>().ExplodeNonAlloc();
            }

            //Disappear
            DestroyObject();    
        }
        
        //Manages the lifetime of the projectile
        protected virtual IEnumerator LifeControl()
        {
            yield return new WaitForSeconds(lifeDuration);
            Explosion();
        }


        #endregion

        #region Private Methods

        private void OnTriggerEnter(Collider other)
        {
            //Cases not considered
            if (other.isTrigger) return;  //hit on trigger zone

            //Try to attack target
            if (!weapon.TryToAttackTarget(other.transform, this)) return;
            
            OnImpact?.Invoke();
            Explosion();

        }

        private void SetSizeObject()
        {
            if (ThisTransform != null) ThisTransform.localScale = Vector3.one*size;
        }

        #endregion

        #endregion

        #region Events

        public event Action OnImpact;

        #endregion

    }
}
