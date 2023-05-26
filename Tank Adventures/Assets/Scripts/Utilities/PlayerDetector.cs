using System;
using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// Send information when a player is detected within a defined zone
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class PlayerDetector : MonoBehaviour
    {
        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var obj = other.gameObject;
            if (obj.CompareTag("Player"))
            {
                OnPlayerDetected?.Invoke(obj.transform);
            }
        }

        public event Action<Transform> OnPlayerDetected;
    }
}