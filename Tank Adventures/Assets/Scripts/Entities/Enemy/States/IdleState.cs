namespace Entities.Enemy.States
{
    /// <summary>
    /// Idle State for Enemies
    /// </summary>
    public class IdleState : EnemyState
    {
        protected override void OnEnter()
        {
            NextState();
        }

        protected override void OnExit()
        {
        
        }

        protected override void UpdateState()
        {
       
        }
    }
}
