using System;
using UnityEngine;

namespace Entities.Entity_Systems.UI
{
    public class UISystem : MonoBehaviour
    {
        [SerializeField] protected UIController controller;

        protected virtual void Awake() { }
    }
}