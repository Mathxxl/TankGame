using System;
using Entities.Entity_Systems.Weapons;
using UnityEngine;

namespace Entities.Enemy.States
{
    /// <summary>
    /// Enemy State for Chase purposes
    /// </summary>
    [Serializable]
    public class ChaseState : EnemyState
    {
        #region Attributes

        [SerializeField] private float chaseSpeed = 8f; //movement speed when chasing
        [SerializeField] private float chaseRotSpeed = 180f; //rotation speed when chasing
        [SerializeField] private float loseDistance = 15f; //stop following its target when its further than this distance
        [SerializeField] private float goalDistance = 3f; //stop moving towards its target when its closer than this distance
        [SerializeField] private float attackDistance = float.MaxValue; //distance from which the enemy attacks
        [SerializeField] private bool isHandToHand;

        private Transform _myTransform;
        private Transform _target;
        private Weapon _weapon;

        #endregion

        #region Methods

        #region Public Methods

        public void SetTarget(Transform t)
        {
            _target = t;
        }

        #endregion

        #region Protected Methods

        protected override void OnEnter()
        {
            base.OnEnter();
            
            Debug.Log($"Controller = {Controller}");
            
            //Components setup
            _myTransform = Controller.transform;
            if (_myTransform.TryGetComponent(out Weapon w))
            {
                _weapon = w;
            }

            //Agent setup
            if (Agent != null)
            {
                Agent.speed = chaseSpeed;
                Agent.angularSpeed = chaseRotSpeed;
                Agent.stoppingDistance = goalDistance;
            }
        }

        protected override void UpdateState()
        {
            if (TargetLost())
            {
                OnPlayerLost?.Invoke();
                Controller.ControllerEntity.Events.OnTargetLost?.Invoke();
            }
            else
            {
                //Go as close as its target as indicated by the goalDistance //=> now with agent
                //if (Vector3.Distance(_myTransform.position, _target.position) >= goalDistance)
                //{
                    Chase();
                //}
                Turn();

                if (!(Vector3.Distance(_myTransform.position, _target.position) < attackDistance)) return;
                
                if (isHandToHand)
                {
                    if (Agent.velocity != Vector3.zero) return;
                }
                _weapon.ToAttack();

            }
        }

        #endregion

        #region Private Methods

        //Move towards its target
        protected void Chase()
        {
            //_myTransform.position = Vector3.MoveTowards(_myTransform.position, _target.position, Time.deltaTime * chaseSpeed);
            Agent.SetDestination(_target.position);
        }

        //Turn to face its target
        private void Turn()
        {
            var dir = (_target.position - _myTransform.position).normalized;
            var goalRot = Quaternion.LookRotation(dir);
            _myTransform.rotation = Quaternion.RotateTowards(_myTransform.rotation, goalRot, Time.deltaTime * chaseRotSpeed);
        }

        //Check if the target is lost, when it is to far away or when it is undefined
        private bool TargetLost()
        {
            if (!_target)
            {
                return true;
            }

            if (Vector3.Distance(_myTransform.position, _target.position) >= loseDistance)
            {
                Debug.Log($"Player lost with distance {Vector3.Distance(_myTransform.position, _target.position)}");
                return true;
            }

            return false;
        }

        #endregion

        #endregion

        #region Events

        public event Action OnPlayerLost;

        #endregion
        
        
    }
}
