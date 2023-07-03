using System;
using System.Linq;
using CustomEditor;
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
            if (other.TryGetComponent(out Stabilizer stabilizer) && tags.Any(other.CompareTag))
            {
                stabilizer.RegisterCurrentTransform();
            }
        }
    }
}