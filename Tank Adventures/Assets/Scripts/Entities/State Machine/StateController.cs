using UnityEngine;

namespace Entities.State_Machine
{
    /// <summary>
    /// State Controller for State Machines
    /// </summary>
    public class StateController : EntitySystem 
    {

        #region Attributes

        private State _currentState;

        #endregion

        #region Properties

        public Entity ControllerEntity => entity;

        #endregion

        #region Methods

        #region Mono Behaviours

        protected override void Awake()
        {
            base.Awake();
            TransitionSetup();
        }
        
        protected void Update()
        {
            _currentState?.OnStateUpdate();
        }

        #endregion

        #region Protected Methods

        //Change to newState by exiting currentState, changing its value and entering the new state
        protected void ChangeState(State newState)
        {
            Debug.Log($"Changing state to {newState}");
            _currentState?.OnStateExit();
            _currentState = newState;
            _currentState.OnStateEnter(this);
        }
        
        protected virtual void TransitionSetup(){}

        #endregion

        #endregion

    }
}
