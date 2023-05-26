using UnityEngine;

namespace Entities.Entity_Systems.UI
{
    public class UIController : EntitySystem
    {
        public Entity Entity => entity;
        [Tooltip("Indicates if the UI elements keeps the same rotation anytime")][SerializeField] protected bool isStable = true;
        private Quaternion _baseRotation;
        
        protected override void Awake()
        {
            _baseRotation = transform.rotation;
        }

        protected void FixedUpdate()
        {
            if (!isStable) return;
            transform.rotation = _baseRotation;
        }
        
    }
}