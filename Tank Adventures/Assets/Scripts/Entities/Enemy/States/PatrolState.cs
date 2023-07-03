using System;
using System.Collections.Generic;
using Utilities;
using UnityEngine;

namespace Entities.Enemy.States
{
    /// <summary>
    /// Patrol State for Enemies
    /// </summary>
    [Serializable]
    public class PatrolState : EnemyState
    {

        #region Attributes

        [SerializeField] private float patrolSpeed = 5f; //movement speed when on patrol
        [SerializeField] private float patrolRotSpeed = 120f; //rotation speed when on patrol
        [SerializeField] private int waypoint; //number of waypoints
        [SerializeField] private List<Transform> waypoints; //waypoints for movement
        [SerializeField] private PlayerDetector detector; //detection zone
        private Transform _myTransform;
        private RaycastHit _hitInfo;

        #endregion

        #region Methods

        #region Protected Methods

        protected override void OnEnter()
        {
            base.OnEnter();
            
          
            if(Controller == null){
                Debug.LogWarning("PatrolState.OnEnter() : Controller is null");
                return;
            }
            
            //Components setup
            _myTransform = Controller.transform;
            
            //Agent setup
            if (Agent != null)
            {
                Agent.speed = patrolSpeed;
                Agent.angularSpeed = patrolRotSpeed;
                Agent.stoppingDistance = 0f;
            }
            
            //Events
            if (detector != null) detector.OnPlayerDetected += ToTargetFound;
            Controller.ControllerEntity.Events.OnAttacked += ToTargetFound;
        }

        protected override void OnExit()
        {
            detector.OnPlayerDetected -= ToTargetFound;
            Controller.ControllerEntity.Events.OnAttacked -= ToTargetFound;
        }

        protected override void UpdateState()
        {
            //Patrol
            if (waypoints.Count > 0) Patrol();
            //Search Player
            if (LookForPlayer())
            {
                ToTargetFound(_hitInfo.transform);
            }
        }

        #endregion

        #region Private Methods

        //Move and rotate from one waypoint to another
        private void Patrol()
        {
            if (Vector3.Distance(_myTransform.position,waypoints[waypoint].position) >= 1f)
            {
                //_myTransform.position = Vector3.MoveTowards(_myTransform.position, waypoints[waypoint].position, patrolSpeed * Time.deltaTime);
                //_myTransform.rotation = Quaternion.RotateTowards(_myTransform.rotation, waypoints[waypoint].rotation, Time.deltaTime * patrolRotSpeed);
                Agent.SetDestination(waypoints[waypoint].position);
            }
            else
            {
                waypoint++;
                if (waypoint >= waypoints.Count)
                {
                    waypoint = 0;
                }
            }
        }

        //Raycast to look for player
        private bool LookForPlayer()
        {
            return UnityEngine.Physics.Raycast(_myTransform.position, _myTransform.forward, out _hitInfo, 3) && _hitInfo.collider.CompareTag("Player");
        }
        
        #endregion

        #endregion
        
    }
}
