using System;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Entities.Entity_Systems
{
    /// <summary>
    /// EntitySystem that modify the player position and rotation when it is in an undesired position
    /// </summary>
    public class Stabilizer : EntitySystem
    {
        #region Attributes

        [Header("Event functions")]
        
        [Tooltip("Function to call when the entity falls in the void")]
        [SerializeField] private UnityEvent onFall;
        [Tooltip("Function to call when the entity is turned around (and therefore can't move)")]
        [SerializeField] private UnityEvent onTurned;

        [Header("Parameters")]
        
        [Tooltip("Hauteur du vide en Y")]
        [SerializeField] private float voidY; //hauteur du vide en Y

        private Transform _myTransform;
        private Quaternion _registeredRotation;
        private Vector3 _registeredPosition;

        public bool stayGrounded;
        public float maxHeight;

        #endregion

        #region Methods

        protected override void Awake()
        {
            base.Awake();
            _myTransform = entity.transform;
            RegisterCurrentTransform();
        }
        
        private void Update()
        {
            if (CheckFall())
            {
                Debug.Log("FALL");
                onFall?.Invoke();
                entity.Events.OnFall?.Invoke();
            }

            if (CheckTurned())
            {
                Debug.Log("TURNED");
                onTurned?.Invoke();
            }

            if (stayGrounded)
            {
                CheckGround();
            }
        }

        private void OnEnable()
        {
            entity.Events.OnPlayerSpawned += RegisterCurrentTransform;
        }

        private void OnDisable()
        {
            entity.Events.OnPlayerSpawned -= RegisterCurrentTransform;
        }

        #region Detectors

        /// <summary>
        /// Checks if the player has fallen in the void
        /// </summary>
        /// <returns></returns>
        private bool CheckFall()
        {
            return (_myTransform.position.y <= voidY);
        }

        
        /// <summary>
        /// Checks if the player is turned around
        /// </summary>
        /// <returns></returns>
        private bool CheckTurned()
        {
            return false;
            
            //TODO
            //var currentRot = _myTransform.rotation;
            //return (Quaternion.Angle(currentRot, _registeredRotation) >= 90);
        }
        
        private void CheckGround()
        {
            RaycastHit hit;
            const int layerMask = 1 << 3;
            if (!UnityEngine.Physics.Raycast(entity.transform.position, -entity.transform.up, out hit, Mathf.Infinity,
                    layerMask)) return;
            if (hit.distance > maxHeight)
            {
                ForceGround();
            }
        }

        #endregion

        #region Entity Specific Methods

        //PLAYER
        public void OnPlayerFall()
        {
            _myTransform.position = new Vector3(_registeredPosition.x, _registeredPosition.y + 2, _registeredPosition.z);
            _myTransform.rotation = _registeredRotation;
            TryDamage(1f);
            TryStop();
        }

        public void OnPlayerTurned()
        {
            if (entity.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(_myTransform.up);
            }
        }
        
        //OTHER
        public void OnEntityFall()
        {
            if (entity is MortalEntity mEntity)
            {
                mEntity.Die();
            }
            else
            {
                entity.gameObject.SetActive(false);
            }
            
        }

        #endregion

        #region Sub Methods

        private void TryDamage(float value)
        {
            if (entity.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamages(value);
            }
        }

        private void TryStop()
        {
            if (entity.TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = Vector3.zero;
            }
        }

        private void ForceGround()
        {
            if (!entity.TryGetComponent(out Rigidbody rb)) return;
            
            rb.AddForce(Vector3.down * 10, ForceMode.Impulse);
        }


        #endregion
        
        /// <summary>
        /// Register the current position and rotation of the player, useful for going back to a register position afterwards
        /// </summary>
        public void RegisterCurrentTransform()
        {
            _registeredPosition = _myTransform.position;
            _registeredRotation = _myTransform.rotation;
        }

        #endregion
        
    }
}