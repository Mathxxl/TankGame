using System;
using System.Collections;
using Entities.Entity_Systems;
using Interfaces;
using UnityEngine;

namespace Entities
{
    /// <summary>
    /// A mortal entity is an entity that is both damageable and destructible.
    /// </summary>
    public class MortalEntity : Entity, IDamageable, IDestructible
    {
        #region Attributes

        [Header("Components")]
        [SerializeField] private EntityHealth entityHealth; //health
        [SerializeField] private EntityDefense entityDefense;
        [Tooltip("[Can be null] Object to destroy on death of entity")][SerializeField] private GameObject toDestroy;
        [Header("Parameters")]
        [SerializeField] private float invincibilityTime = 0.5f; //how long is the entity invincible after receiving a hit

        private bool _invincible;

        public bool Invincible
        {
            get => _invincible;
            set
            {
                _invincible = value;
                Events.OnInvincible?.Invoke(value);
            }
        }

        #endregion

        #region Methods

        public void DestroyObject()
        {
            Destroy(gameObject);
            if(toDestroy != null) Destroy(toDestroy);
        }

        public void TakeDamages(float value)
        {
            if (_invincible) return;                    //No damages taken if invincible
            if (entityDefense != null)                  //Defense handling
            {
                var diff = value - entityDefense.Defense;
                value = (diff >= 0) ? diff : 0;
            }
            Events.OnTakeDamage?.Invoke(value);         //Call event
            entityHealth.Health -= value;               //Change Health value
            StartCoroutine(BeingInvincible());    //Start invincible time
            
            Debug.Log($"{gameObject.name} has lost {value} LP and has now {entityHealth.Health} LP");

            //Destroy entity if health goes negative
            if (entityHealth.Health <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            StartCoroutine(Dying());
        }

        protected override void Awake()
        {
            base.Awake();
            if (entityHealth == null) entityHealth = gameObject.AddComponent<EntityHealth>();
        }

        protected void OnEnable()
        {
            GameManagerForced.Events.OnGoalAchieved += OnLevelEnd;
        }

        protected void OnDisable()
        {
            if (GameManager != null)
            {
                GameManager.Events.OnGoalAchieved -= OnLevelEnd;
            }
        }

        private void OnLevelStart() //ADD WORLD TYPE ?
        {
            _invincible = false;
        }

        private void OnLevelEnd()
        {
            _invincible = true;
        }

        private IEnumerator BeingInvincible()
        {
            yield return null;
            Debug.Log($"------ INVINCIBLE {gameObject.name} ------");
            Invincible = true;
            yield return new WaitForSeconds(invincibilityTime);
            Debug.Log($"------ INVINCIBLE END {gameObject.name} ------");
            Invincible = false;
        }

        private IEnumerator Dying()
        {
            //Animation
            
            //Wait
            yield return new WaitForSeconds(0);
            
            //Object Destroyed
            Events.OnDeath?.Invoke();
            DestroyObject();
        }

        #endregion
        
    }
}