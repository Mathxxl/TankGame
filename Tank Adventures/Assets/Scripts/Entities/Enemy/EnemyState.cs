using System;
using Entities.Entity_Systems.Weapons;
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

        protected Weapon ThisWeapon;
        //Agent
        protected NavMeshAgent Agent;
        
        //Called when a target is found
        public event Action<Transform> TargetFound;

        //Call the TargetFound Event
        protected void ToTargetFound(Transform t)
        {
            TargetFound?.Invoke(t);
            Controller.ControllerEntity.Events.OnTargetAcquired?.Invoke(t);
        }

        //Add agent
        protected override void OnEnter()
        {
            base.OnEnter();
            
            //Debug.Log($"Controller = {Controller}");
            
            if (Controller is EnemyStateController esc)
            {
                Agent = esc.agent;
                ThisWeapon = esc.ThisWeapon;
            }
            
            //Debug.Log($"Controller = {Controller}");
        }
    }
}