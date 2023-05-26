using System;
using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Camera
{
    public class CameraBehaviour : MonoBehaviour
    {
        [SerializeField] private float mDampTime = 0.1f;                 // Approximate time for the camera to refocus.
        [SerializeField] private float mScreenEdgeBuffer = 2f;           // Space between the top/bottom most target and the screen edge.
        [SerializeField] private float mMinSize = 6.5f;                  // The smallest orthographic size the camera can be.
        [SerializeField] private List<Transform> mTargets; // All the targets the camera needs to encompass.
        [SerializeField] private Vector3 compensation;

        private UnityEngine.Camera _mCamera;                        // Used for referencing the camera.
        private float _mZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
        private Vector3 _mMoveVelocity;                 // Reference velocity for the smooth damping of the position.
        private Vector3 _mDesiredPosition;              // The position the camera is moving towards.
        private List<Entity> _entities;

        private void Awake ()
        {
            _mCamera = GetComponentInChildren<UnityEngine.Camera> ();
            _entities = new List<Entity>();
        
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


        private void FixedUpdate ()
        {
            if (mTargets.Count == 0) return;
        
            // Move the camera towards a desired position.
            Move ();

            // Change the size of the camera based.
            Zoom ();
        }


        private void Move ()
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
            Vector3 averagePos = new Vector3 ();
            int numTargets = 0;

            // Go through all the targets and add their positions together.
            foreach (var t in mTargets)
            {
                if (t == null) continue;
            
                // If the target isn't active, go on to the next one.
                if (!t.gameObject.activeSelf)
                    continue;

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
            float requiredSize = FindRequiredSize();
            _mCamera.orthographicSize = Mathf.SmoothDamp (_mCamera.orthographicSize, requiredSize, ref _mZoomSpeed, mDampTime);
        }


        private float FindRequiredSize ()
        {
            // Find the position the camera rig is moving towards in its local space.
            Vector3 desiredLocalPos = transform.InverseTransformPoint(_mDesiredPosition);

            // Start the camera's size calculation at zero.
            float size = 0f;

            // Go through all the targets...
            foreach (var t in mTargets)
            {
                // ... and if they aren't active continue on to the next target.
                if (!t.gameObject.activeSelf)
                    continue;

                // Otherwise, find the position of the target in the camera's local space.
                Vector3 targetLocalPos = transform.InverseTransformPoint(t.position);

                // Find the position of the target from the desired position of the camera's local space.
                Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

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


        public void SetStartPositionAndSize ()
        {
            // Find the desired position.
            FindAveragePosition ();

            // Set the camera's position to the desired position without damping.
            transform.position = _mDesiredPosition;

            // Find and set the required size of the camera.
            _mCamera.orthographicSize = FindRequiredSize ();
        }

        private void AddCheckOnDeath()
        {
            foreach (var target in mTargets)
            {
                if (target.TryGetComponent(out MortalEntity entity))
                {
                    entity.Events.OnDeath += () => OnTargetDeath(target);
                    _entities.Add(entity);
                }
            }
        }

        private void RemoveCheckOnDeath()
        {
            foreach (var entity in _entities)
            {
                if(entity != null) entity.Events.OnDeath -= () => OnTargetDeath(entity.transform);
            }
        }

        private void OnTargetDeath(Transform target)
        {
            mTargets.Remove(target);
        }
    }
}
