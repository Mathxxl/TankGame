using UnityEngine;

namespace Entities.Entity_Systems.UI
{
    /// <summary>
    /// Abstract for UI Systems
    /// </summary>
    public abstract class UISystem : MonoBehaviour
    {
        [SerializeField] protected UIController controller;

        protected virtual void Awake() { }
    }
}