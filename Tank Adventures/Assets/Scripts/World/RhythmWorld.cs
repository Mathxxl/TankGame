using Entities.Player.Player_Systems;
using UnityEngine;

namespace World
{
    public class RhythmWorld : World
    {
        private Rigidbody _playerRb;
        private PlayerMovement _playerMovement;
        private UnityEngine.Camera _pCamera;
        
        protected override void OnEnter()
        {
            //Player Movement
            if(manager.GManager.Player.TryGetComponent(out Rigidbody rb))
            {
                _playerRb = rb;
                _playerRb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;

                if (rb.gameObject.TryGetComponent(out PlayerMovement playerMovement))
                {
                    _playerMovement = playerMovement;
                    _playerMovement.canRotate = false;
                }
            }
            
            //Camera
            _pCamera = manager.GManager.playerCamera;
            if (_pCamera != null)
            {
                _pCamera.gameObject.SetActive(false);
            } 
        }

        protected override void OnExit()
        {
            //Player Movement
            if (_playerRb != null)
            {
                _playerRb.constraints = RigidbodyConstraints.None;
            }

            if (_playerMovement != null)
            {
                _playerMovement.canRotate = true;
            }
            
            //Camera
            if (_pCamera != null)
            {
                _pCamera.gameObject.SetActive(true);
            } 
        }

        protected override void OnUpdate()
        {
            
        }
    }
}