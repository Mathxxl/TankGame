using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
            entity.GameManagerForced.Events.OnGoalAchieved += () => { PausePlayerInput(); };
            entity.GameManagerForced.Events.OnLevelStart += () => { PausePlayerInput(false); };
        }

        private void OnDisable()
        {
            if (entity.GameManager != null)
            {
                entity.GameManager.Events.OnGoalAchieved -= () => { PausePlayerInput(); };
                entity.GameManager.Events.OnLevelStart -= () => { PausePlayerInput(false); };
            }
        }

        private void PausePlayerInput(bool pause = true)
        {
            Debug.Log($"Player Input enabled = {!pause}");
            playerInput.enabled = !pause;
        }
    }
}