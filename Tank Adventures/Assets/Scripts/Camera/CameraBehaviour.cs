using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using UnityEngine;

namespace Camera
{
    /// <summary>
    /// Manages the movement of the camera
    /// </summary>
    public class CameraBehaviour : MonoBehaviour
    {
        [SerializeField] private float mDampTime = 0.1f;                 // Approximate time for the camera to refocus.
        [SerializeField] private float mScreenEdgeBuffer = 2f;           // Space between the top/bottom most target and the screen edge.
        [SerializeField] private float mMinSize = 6.5f;                  // The smallest orthographic size the camera can be.
        [SerializeField] private List<Transform> mTargets; // All the targets the camera needs to encompass.
        [SerializeField] private Vector3 compensation;
        [SerializeField] private Transform normalPosition;
        [SerializeField] private Transform rhythmPosition;
        [SerializeField] private Transform racePosition;

        private UnityEngine.Camera _mCamera;                        // Used for referencing the camera.
        private float _mZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
        private Vector3 _mMoveVelocity;                 // Reference velocity for the smooth damping of the position.
        private Vector3 _mDesiredPosition;              // The position the camera is moving towards.
        private List<Entity> _entities;
        private CameraMode _mode;
        
        
        public CameraMode Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                SetCameraWithMode();
            }
        }

        private void Awake()
        {
            _mCamera = GetComponentInChildren<UnityEngine.Camera>();
            _entities = new List<Entity>();
            Mode = CameraMode.Normal;
            
            //Manage compensation
            FindAveragePosition();
            compensation = transform.position - _mDesiredPosition;

            //Find player if no targets
            if (mTargets == null || mTargets.Count == 0)
            {
                mTargets = new List<Transform>(){GameObject.Find("Player").transform};
            }
        }

        private void OnEnable()
        {
            AddCheckOnDeath();
        }

        private void OnDisable()
        {
            RemoveCheckOnDeath();
        }


        private void FixedUpdate()
        {
            if (mTargets.Count == 0) return;
        
            // Move the camera towards a desired position.
            Move();

            // Change the size of the camera based.
            Zoom();
        }


        private void Move()
        {
            // Find the average position of the targets.
            FindAveragePosition();

            //Compensation
            _mDesiredPosition += compensation;
        
            // Smoothly transition to that position.
            transform.position = Vector3.SmoothDamp(transform.position, _mDesiredPosition, ref _mMoveVelocity, mDampTime);
        }


        private void FindAveragePosition ()
        {
            var averagePos = new Vector3 ();
            var numTargets = 0;

            // Go through all the targets and add their positions together.
            foreach (var t in mTargets.Where(t => t != null).Where(t => t.gameObject.activeSelf))
            {
                // Add to the average and increment the number of targets in the average.
                averagePos += t.position;
                numTargets++;
            }

            // If there are targets divide the sum of the positions by the number of them to find the average.
            if (numTargets > 0)
                averagePos /= numTargets;

            // Keep the same y value.
            averagePos.y = transform.position.y;

            // The desired position is the average position;
            _mDesiredPosition = averagePos;
        }


        private void Zoom ()
        {
            // Find the required size based on the desired position and smoothly transition to that size.
            var requiredSize = FindRequiredSize();
            _mCamera.orthographicSize = Mathf.SmoothDamp (_mCamera.orthographicSize, requiredSize, ref _mZoomSpeed, mDampTime);
        }


        private float FindRequiredSize ()
        {
            // Find the position the camera rig is moving towards in its local space.
            var desiredLocalPos = transform.InverseTransformPoint(_mDesiredPosition);

            // Start the camera's size calculation at zero.
            var size = 0f;

            // Go through all the targets...
            foreach (var desiredPosToTarget in from t in mTargets
                     where t.gameObject.activeSelf
                     select transform.InverseTransformPoint(t.position)
                     into targetLocalPos
                     select targetLocalPos - desiredLocalPos)
            {
                // Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

                // Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / _mCamera.aspect);
            }

            // Add the edge buffer to the size.
            size += mScreenEdgeBuffer;

            // Make sure the camera's size isn't below the minimum.
            size = Mathf.Max (size, mMinSize);

            return size;
        }


        public void SetStartPositionAndSize()
        {
            // Find the desired position.
            FindAveragePosition ();

            // Set the camera's position to the desired position without damping.
            transform.position = _mDesiredPosition;

            // Find and set the required size of the camera.
            _mCamera.orthographicSize = FindRequiredSize ();
        }

        /// <summary>
        /// Check in camera targets if there are any mortal entities, to remove them of the following object if needed
        /// </summary>
        private void AddCheckOnDeath()
        {
            foreach (var target in mTargets)
            {
                if (!target.TryGetComponent(out MortalEntity entity)) continue;
                entity.Events.OnDeath += () => OnTargetDeath(target);
                _entities.Add(entity);
            }
        }

        private void RemoveCheckOnDeath()
        {
            foreach (var entity in _entities.Where(entity => entity != null))
            {
                entity.Events.OnDeath -= () => OnTargetDeath(entity.transform);
            }
        }

        private void OnTargetDeath(Transform target)
        {
            mTargets.Remove(target);
        }

        private void SetCameraWithMode()
        {
            if (_entities.Count == 0) return;
            
            switch (Mode)
            {
                case CameraMode.Normal:
                    if (normalPosition == null) return; 
                    SetModePosition(normalPosition);
                    break;
                case CameraMode.Race:
                    if (racePosition == null) return;
                    SetModePosition(racePosition);
                    break;
                case CameraMode.Rhythm:
                    if (rhythmPosition == null) return;
                    SetModePosition(rhythmPosition);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void SetModePosition(Transform target)
        {
            var thisTransform = transform;
            thisTransform.position = target.position;
            thisTransform.rotation = target.rotation;
        }
    }
}
