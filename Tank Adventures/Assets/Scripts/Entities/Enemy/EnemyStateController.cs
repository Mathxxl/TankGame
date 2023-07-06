using System;
using Entities.Enemy.States;
using Entities.Entity_Systems.Weapons;
using Entities.State_Machine;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Enemy
{
    /// <summary>
    /// State Controller specific for enemies
    /// </summary>
    public class EnemyStateController : StateController
    {
        public NavMeshAgent agent;
        protected bool Moving;

        [SerializeField] private Weapon weapon;
        public Weapon ThisWeapon => weapon;
        
        #region States
        
        public IdleState idleState = new IdleState();
        public PatrolState patrolState = new PatrolState();
        public ChaseState chaseState = new ChaseState();

        #endregion

        #region Methods

        #region Mono Behaviours

        protected virtual void Start()
        {
            ChangeState(idleState); //not in awake to avoid problems
        }

        protected virtual void OnDisable()
        {
            idleState.OnNextState -= SetPatrol;
            patrolState.TargetFound -= SetChase;
            chaseState.OnPlayerLost -= SetPatrol;
            entity.GameManager.Events.OnGoalAchieved -= SetIdle;
            entity.Events.OnDying -= SetIdle;
        }

        #endregion

        #region Protected Methods
        
        protected override void TransitionSetup()
        {
            idleState.OnNextState += SetPatrol; //Patrol after idle
            patrolState.TargetFound += SetChase; //Chase after a target is found
            chaseState.OnPlayerLost += SetPatrol; //Patrol after target is lost
            if (entity.GameManager != null)
            {
                entity.GameManager.Events.OnGoalAchieved += SetIdle;
            }
            entity.Events.OnDying += SetIdle;
        }
        #endregion

        #region Private Methods

        //Change to Patrol State
        private void SetPatrol()
        {
            Debug.Log("Set patrol");
            ChangeState(patrolState);
        }

        //Set target and change state to Chase
        private void SetChase(Transform target)
        {
            Debug.Log($"Set Chase on {target.name}");
        
            chaseState.SetTarget(target);
            ChangeState(chaseState);
        }

        //Change to idle state
        protected void SetIdle()
        {
            Debug.Log("Set idle");
            ChangeState(idleState);
        }
        
        //Events
        private void FixedUpdate()
        {
            if (agent.velocity != Vector3.zero && !Moving)
            {
                entity.Events.OnStartMoving?.Invoke();
                Moving = true;
            }

            if (agent.velocity == Vector3.zero && Moving)
            {
                entity.Events.OnStopMoving?.Invoke();
                Moving = false;
            }
        }

        #endregion
        
        #endregion

    }
}
