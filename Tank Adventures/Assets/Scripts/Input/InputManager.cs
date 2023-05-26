using System;
using System.Collections;
using DesignPatterns;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    /// <summary>
    ///  Manage the inputs of the in game player
    /// </summary>
    public sealed class InputManager : Singleton<InputManager>
    {
        [SerializeField] private PlayerInput playerInput;

        private void OnEnable()
        {
            InputSystem.onActionChange += OnActionChange;
        }

        private void OnDisable()
        {
            InputSystem.onActionChange -= OnActionChange;
        }


        private void OnActionChange(object obj, InputActionChange change)
        {
            Debug.Log($"On Action Change : {change}");
            
            if (obj is not InputAction action) return;
            if (change != InputActionChange.ActionPerformed) return;

            var control = action.activeControl;

            if (control == null) return;

            playerInput.SwitchCurrentControlScheme(control.device);
            //StartCoroutine(Test());
            OnDeviceChanged?.Invoke();
        }
        
        private IEnumerator Test()
        {
            yield return new WaitForSeconds(2f);
            Debug.Log("Switching Back");
            playerInput.SwitchCurrentControlScheme("Gamepad");
        }

        #region ActionMaps
        
  
        public void DisableControls()
        {
            playerInput.enabled = false;
        }
    

        public void EnableControls()
        {
            playerInput.enabled = true;
        }

        #endregion

        #region Events

        //New InputDevice action
        public event Action OnDeviceChanged;

        #endregion
        
    }
}