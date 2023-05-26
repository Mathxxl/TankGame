using Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Entities.Entity_Systems
{
    public class Stabilizer : EntitySystem
    {
        #region Attributes

        [Header("Event functions")]
        
        [SerializeField] private UnityEvent onFall;
        [SerializeField] private UnityEvent onTurned;

        [Header("Parameters")]
        
        [Tooltip("Hauteur du vide en Y")]
        [SerializeField] private float voidY; //hauteur du vide en Y

        private Transform _myTransform;
        private Quaternion _registeredRotation;
        private Vector3 _registeredPosition;

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
        }

        #region Detectors

        private bool CheckFall()
        {
            return (_myTransform.position.y <= voidY);
        }

        private bool CheckTurned()
        {
            return false;
            
            //TODO
            //var currentRot = _myTransform.rotation;
            //return (Quaternion.Angle(currentRot, _registeredRotation) >= 90);
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

        #endregion
        
        public void RegisterCurrentTransform()
        {
            _registeredPosition = _myTransform.position;
            _registeredRotation = _myTransform.rotation;
        }

        #endregion
        
    }
}