using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities.Player.Player_Systems
{
    /// <summary>
    /// Manages the movements of the player with a PlayerInput component
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMovement : EntitySystem
    {
        #region Attributes
    
        private Rigidbody _rb;
        private Vector2 _moveVector = Vector2.zero;
        private bool _moving;

        [SerializeField] private float speed = 15f;
        [SerializeField] private float turnSpeed = 180f;
        [SerializeField] private float maxSpeedFactor = 1/4f;

        public bool canRotate = true;
        
        public float Speed
        {
            get => speed;
            set
            {
                speed = value;
                entity.Events.OnChangeSpeed?.Invoke(speed);
            }
        }

        #endregion

        #region Methods

        protected override void Awake()
        {
            base.Awake();
            _rb = entity.gameObject.GetComponent<Rigidbody>();
            speed = entity.id.speed;
        }

        private void FixedUpdate()
        {
            if(_moveVector == Vector2.zero && _moving){
                entity.Events.OnStopMoving?.Invoke();
                _moving = false;
                return;
            }
            
            if (_moveVector != Vector2.zero && !_moving)
            {
                entity.Events.OnStartMoving?.Invoke();
                _moving = true;
            }

            /*var vel = _rb.velocity;
            if (vel != Vector3.zero)
            {
                _rb.velocity = AdjustVelocityToSlope(vel);
            }*/
            
            Move();
            Turn();
        }

        private void OnEnable()
        {
            entity.Events.OnImproveSpeed += ImproveSpeed;
            entity.Events.OnImproveSpeedFixed += ImproveSpeedFixed;
            
            _rb.isKinematic = false;
        }
    
        private void OnDisable()
        {
            entity.Events.OnImproveSpeed -= ImproveSpeed;
            entity.Events.OnImproveSpeedFixed -= ImproveSpeedFixed;
            
            _rb.isKinematic = true;
        }

        private void OnMovement(InputValue value)
        {
            _moveVector = value.Get<Vector2>();
        }

        private void Move()
        {
            if (_rb.velocity.magnitude > speed * maxSpeedFactor)
            {
                Debug.Log($"LIMITING SPEED : {_rb.velocity.magnitude} vs {speed*maxSpeedFactor}");
                return;
            }

            if (!canRotate)
            {
                var posNegCheck = _moveVector.x + _moveVector.y;
                _rb.AddForce(transform.right * ((posNegCheck > 0 ? 1 : -1) * _moveVector.magnitude * speed));
            }
            else _rb.AddForce(transform.forward * (_moveVector.magnitude * speed));
            
        }

        private void Turn()
        {
            if (_moveVector == Vector2.zero || !canRotate) return;
            
            var movementDirection = new Vector3(_moveVector.x, 0f, _moveVector.y);
            var toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            _rb.rotation = Quaternion.RotateTowards(_rb.rotation, toRotation, turnSpeed * Time.deltaTime);
        }

        private void ImproveSpeed(float value)
        {
            Speed *= (1.0f + value);
        }

        private void ImproveSpeedFixed(float value)
        {
            Speed += value;
        }

        private Vector3 AdjustVelocityToSlope(Vector3 velocity)
        {
            var ray = new Ray(entity.transform.position, Vector3.down);

            if (UnityEngine.Physics.Raycast(ray, out RaycastHit hit, 0.2f))
            {
                var slopeRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                var adjustVelocity = slopeRotation * velocity;

                if (adjustVelocity.y < 0)
                {
                    return adjustVelocity;
                }
            }

            return velocity;
        }

        #endregion
    }
}
