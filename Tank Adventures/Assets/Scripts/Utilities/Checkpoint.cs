using System.Linq;
using Entities.Entity_Systems;
using UnityEngine;

namespace Utilities
{
    [RequireComponent(typeof(Collider))]
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] [TagSelector] private string[] tags;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!tags.Any(other.CompareTag)) return;
            
            var stab = other.GetComponentInChildren<Stabilizer>();
            
            if (stab == null) return;
            
            stab.RegisterCurrentTransform();
        }
    }
}