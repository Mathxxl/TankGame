﻿using System;
using System.Linq;
using UnityEngine;

namespace Utilities
{
    [RequireComponent(typeof(Collider))]
    public class EntityDetector : MonoBehaviour
    {
        private Collider _collider;
        [SerializeField][TagSelector] protected string[] detectableTags;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var obj = other.gameObject;
            if (CompareAllTags(obj))
            {
                OnEntityDetected?.Invoke(obj.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var obj = other.gameObject;
            if (CompareAllTags(obj))
            {
                OnEntityLeft?.Invoke(obj.transform);
            }
        }

        private bool CompareAllTags(GameObject obj)
        {
            return detectableTags.Any(obj.CompareTag);
        }

        public event Action<Transform> OnEntityDetected;
        public event Action<Transform> OnEntityLeft;
    }
}