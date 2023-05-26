using System;
using Entities.State_Machine;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Enemy
{
    /// <summary>
    /// Specific State for Enemies
    /// </summary>
    public abstract class EnemyState : State
    {
        //Agent
        protected NavMeshAgent Agent;
        
        //Called when a target is found
        public event Action<Transform> TargetFound;

        //Call the TargetFound Event
        protected void ToTargetFound(Transform t)
        {
            TargetFound?.Invoke(t);
        }

        //Add agent
        protected override void OnEnter()
        {
            base.OnEnter();
            if (Controller is EnemyStateController esc)
            {
                Agent = esc.agent;
            }
        }
    }
}