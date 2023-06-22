using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Entities.Player.Player_Systems
{
    /// <summary>
    /// Manages the player input component
    /// TODO : Changer the input functions from classes to events
    /// </summary>
    public class PlayerInputManager : EntitySystem
    {
        [SerializeField] private PlayerInput playerInput;
        private bool _paused;

        private void OnEnable()
        {
            entity.GameManagerForced.Events.OnGoalAchieved += PausePlayerInput;
            entity.GameManagerForced.Events.OnZoneStart += ResumePlayerInput;
            
            //DEBUG
            entity.GameManagerForced.Events.OnFreeZoneReached += ResumePlayerInput;
        }

        private void OnDisable()
        {
            if (entity.GameManager == null) return;
            
            entity.GameManager.Events.OnGoalAchieved -= PausePlayerInput;
            entity.GameManager.Events.OnZoneStart -= ResumePlayerInput;
        }

        private void PausePlayerInput()
        {
            Debug.Log($"Player Input paused");
            playerInput.enabled = false;
        }

        private void ResumePlayerInput()
        {
            Debug.Log("Player input resumed");
            playerInput.enabled = true;
        }
    }
}