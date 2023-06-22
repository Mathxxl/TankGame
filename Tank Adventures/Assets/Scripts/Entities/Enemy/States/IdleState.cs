using UnityEngine;

namespace Entities.Enemy.States
{
    /// <summary>
    /// Idle State for Enemies
    /// </summary>
    public class IdleState : EnemyState
    {
        protected override void OnEnter()
        {
            Controller.ControllerEntity.GameManager.Events.OnLevelStart += NextState;
        }

        protected override void OnExit()
        {
            if(Controller != null) Controller.ControllerEntity.GameManager.Events.OnLevelStart -= NextState;
        }

        protected override void UpdateState()
        {
       
        }
    }
}
