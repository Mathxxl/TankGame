using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameManagers
{
    public class EndScreenManager : Manager
    {
        [SerializeField] private GameObject endScreen;
        [SerializeField] private GameObject lostButtonsHolder;
        [SerializeField] private GameObject winButtonsHolder;
        [SerializeField] private Image endGameBackground;
        [SerializeField] private TextMeshProUGUI tmHolder;

        [SerializeField] [TextArea] private string loseText;
        
        private void OnEnable()
        {
            gameManager.Events.OnPlayerKilled += PlayerLost;
        }

        private void OnDisable()
        {
            gameManager.Events.OnPlayerKilled -= PlayerLost;
        }

        private void PlayerWon()
        {
            
        }

        private void PlayerLost()
        {
            //endGameBackground.color = Color.red;
            tmHolder.text = loseText;
            
            endScreen.SetActive(true);
            lostButtonsHolder.SetActive(true);
        }
        
        
    }
}