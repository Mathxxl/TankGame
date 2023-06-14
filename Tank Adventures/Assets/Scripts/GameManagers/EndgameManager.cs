using System;
using UnityEngine;

namespace GameManagers
{
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
        }
    }
}