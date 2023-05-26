using System;
using UnityEngine;

namespace Entities.Entity_Systems
{
    //DEPRECATED
    public class Foot : EntitySystem
    {
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"Foot collision with {collision.collider.gameObject.name}");
            
            if (!collision.collider.CompareTag("Ground")) return;
            
            entity.Events.OnGroundTouched?.Invoke();
            Debug.Log("TOUCH GROUND");
        }

        private void OnCollisionExit(Collision other)
        {
            Debug.Log($"Foot collision exit with {other.collider.gameObject.name}");
            
            if (!other.collider.CompareTag("Ground")) return;
            
            entity.Events.OnGroundLeft?.Invoke();
            Debug.Log("LEFT GROUND");
        }
    }
}