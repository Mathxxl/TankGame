using UnityEngine;

namespace Entities.Enemy
{
    public class ChaserStateController : EnemyStateController
    {
        protected override void OnDisable()
        {
            if (entity == null || entity.GameManager == null) return;
            
            entity.GameManager.Events.OnGoalAchieved -= SetIdle;
            entity.GameManager.Events.OnLevelStart -= ChaseStart;
        }

        protected override void TransitionSetup()
        {
            entity.GameManagerForced.Events.OnGoalAchieved += SetIdle;
            entity.GameManagerForced.Events.OnLevelStart += ChaseStart;
        }

        private void ChaseStart()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("No player found for chasers");
                return;
            }
            
            chaseState.SetTarget(player.transform);
            ChangeState(chaseState);
        }
    }
}