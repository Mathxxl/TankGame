using UnityEngine;

namespace GameManagers
{
    /// <summary>
    /// Manages the end of the game, when the final level is completed
    /// </summary>
    public class EndgameManager : Manager
    {
        private void OnEnable()
        {
            gameManager.Events.OnFinalLevelAchieved += Endgame;
        }

        private void OnDisable()
        {
            gameManager.Events.OnFinalLevelAchieved -= Endgame;
        }

        private void Endgame()
        {
            Debug.Log("Congratulations you have won");
            gameManager.Events.OnSelectedScene?.Invoke("CreditsScene");
        }
    }
}