using Entities.Enemy.States;
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
        
        #region States
        
        public IdleState idleState = new IdleState();
        public PatrolState patrolState = new PatrolState();
        public ChaseState chaseState = new ChaseState();

        #endregion

        #region Methods

        #region Mono Behaviours

        protected void Start()
        {
            ChangeState(idleState);
        }

        private void OnEnable()
        {
            TransitionSetup();
        }

        private void OnDisable()
        {
            idleState.OnNextState -= SetPatrol;
            patrolState.TargetFound -= SetChase;
            chaseState.OnPlayerLost -= SetPatrol;
        }

        #endregion

        #region Protected Methods
        
        protected override void TransitionSetup()
        {
            idleState.OnNextState += SetPatrol; //Patrol after idle
            patrolState.TargetFound += SetChase; //Chase after a target is found
            chaseState.OnPlayerLost += SetPatrol; //Patrol after target is lost
        }
        #endregion

        #region Private Methods

        //Change to Patrol State
        private void SetPatrol()
        {
            ChangeState(patrolState);
        }

        //Set target and change state to Chase
        private void SetChase(Transform target)
        {
            Debug.Log($"Set Chase on {target.name}");
        
            chaseState.SetTarget(target);
            ChangeState(chaseState);
        }
        

        #endregion
        
        #endregion

    }
}
