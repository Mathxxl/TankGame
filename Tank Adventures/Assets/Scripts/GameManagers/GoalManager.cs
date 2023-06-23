using Entities;
using UnityEngine;

namespace GameManagers
{
    /// <summary>
    /// Manages the goal of worlds
    /// </summary>
    public class GoalManager : Manager
    {
        private void OnEnable()
        {
            gameManager.Events.OnGoalFailed += GoalFailed;
        }

        private void OnDisable()
        {
            gameManager.Events.OnGoalFailed -= GoalFailed;
        }

        private void GoalFailed()
        {
            if (gameManager.Player.TryGetComponent(out MortalEntity me))
            {
                me.Die();   
            }
            else
            {
                Debug.LogError("The player is not a mortal entity");
            }
        }
    }
}