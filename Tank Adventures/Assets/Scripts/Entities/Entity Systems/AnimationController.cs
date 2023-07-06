using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Entities.Entity_Systems
{
    public class AnimationController : EntitySystem
    {
        [SerializeField] protected Animator animator;
        
        private static readonly int ShootTrigger = Animator.StringToHash("ShootTrigger");
        private static readonly int DeathTrigger = Animator.StringToHash("Death");

        protected virtual void OnEnable()
        {
            if (animator == null)
            {
                Debug.LogWarning($"Animation Controller for {entity.name} : no animator set");
                return;
            }
            
            entity.Events.OnPerformingAttack += () => StartAnimation(ShootTrigger);
            entity.Events.OnDying += () => StartAnimation(DeathTrigger);
        }

        protected virtual void OnDisable()
        {
            if (animator == null) return;
            
            entity.Events.OnPerformingAttack -= () => StartAnimation(ShootTrigger);
            entity.Events.OnDying -= () => StartAnimation(DeathTrigger);
        }

        protected void StartAnimation(int id)
        {
            if (animator.parameters.All(para => para.nameHash != id)) return;
            animator.SetTrigger(id);
        }

    }
}