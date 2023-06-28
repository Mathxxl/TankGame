using System;
using UnityEngine;

namespace Testing
{
    public class DebugObject : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}