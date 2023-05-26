using System;
using UnityEngine;

namespace Entities.Player.Player_Systems
{
    /// <summary>
    /// 
    /// </summary>
    public class GroundManager : EntitySystem
    {
        [SerializeField] private GameObject foot;
        private bool _grounded;

        private void OnCollisionEnter(Collision collision)
        {
            if (_grounded) return;
            if (!CheckCollisionWithGround(collision)) return;
            
            Debug.Log("Ground touched");
            _grounded = true;
        }

        private void OnCollisionExit(Collision other)
        {
            if(!_grounded) return;
            if (!CheckCollisionWithGround(other)) return;
            
            Debug.Log("Ground Left");
            _grounded = false;
        }

        private bool CheckCollisionWithGround(Collision collision)
        {
            if(collision.collider.CompareTag("Ground"))
            {
                Debug.Log($"Contact with ground : {collision.contacts.Length} contacts");
                if (collision.contacts.Length > 0)
                {
                    Debug.Log($"Collider object is {collision.GetContact(0).thisCollider}");
                }
            }
            
            
            if (collision.contacts.Length <= 0) return false;

            var myCollider = collision.GetContact(0).thisCollider;
            return collision.collider.CompareTag("Ground") && myCollider.name == "Foot";
        }
    }
}