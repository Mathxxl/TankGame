using System;
using UnityEngine;

namespace Entities.Entity_Systems
{
    public class AnimationController : EntitySystem
    {
        [SerializeField] protected Animator animator;
        private static readonly int ShootTrigger = Animator.StringToHash("ShootTrigger");

        protected virtual void OnEnable()
        {
            if (animator == null)
            {
                Debug.LogWarning($"Animation Controller for {entity.name} : no animator set");
                return;
            }
            
            entity.Events.OnPerformingAttack += () => StartAnimation(ShootTrigger);
        }

        protected virtual void OnDisable()
        {
            if (animator == null) return;
            
            entity.Events.OnPerformingAttack -= () => StartAnimation(ShootTrigger);
        }

        protected void StartAnimation(int id)
        {
            animator.SetTrigger(id);
        }
        
    }
}