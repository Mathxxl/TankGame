using System;
using UnityEngine;

namespace Entities.State_Machine
{
    /// <summary>
    /// Abstract class for States of State Machines
    /// </summary>
    public abstract class State
    {
        #region Attributes

        protected StateController Controller;

        #endregion


        #region Methods

        #region Public Methods

        //Called when entering state
        public void OnStateEnter(StateController controller)
        {
            Controller = controller;
            OnEnter();
        }

        //Called each frame when state is active
        public void OnStateUpdate()
        {
            UpdateState();
        }

        
        //Called when state is exited
        public void OnStateExit()
        {
            OnExit();
        }

        #endregion

        #region Protected Methods

        protected virtual void OnEnter(){}
        protected virtual void OnExit(){}
        protected virtual void UpdateState(){}
        
        protected void NextState()
        {
            Debug.Log("NextState Event");
            OnNextState?.Invoke();
        }

        #endregion
        
        #endregion

        #region Events

        public event Action OnNextState;
        
        #endregion
    }
}
