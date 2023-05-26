using System;
using UnityEngine;

namespace Entities.Entity_Systems
{
    public class AnimationController : EntitySystem
    {
        [SerializeField] private Animator animator;
        private static readonly int ShootTrigger = Animator.StringToHash("ShootTrigger");

        private void OnEnable()
        {
            if (animator == null)
            {
                Debug.LogWarning($"Animation Controller for {entity.name} : no animator set");
                return;
            }
            
            entity.Events.OnPerformingAttack += () => StartAnimation(ShootTrigger);
        }

        private void OnDisable()
        {
            if (animator == null) return;
            
            entity.Events.OnPerformingAttack -= () => StartAnimation(ShootTrigger);
        }

        private void StartAnimation(int id)
        {
            animator.SetTrigger(id);
        }
        
    }
}