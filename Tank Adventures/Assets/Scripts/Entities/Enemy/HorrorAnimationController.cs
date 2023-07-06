using System.Collections;
using Entities.Entity_Systems;
using UnityEngine;

namespace Entities.Enemy
{
    public class HorrorAnimationController : AnimationController
    {
        private static readonly int Moving = Animator.StringToHash("moving");
        private static readonly int Battle = Animator.StringToHash("battle");

        protected override void OnEnable()
        {
            entity.Events.OnStartMoving += () => Walk(true);
            entity.Events.OnStopMoving += () => Walk(false);
            entity.Events.OnPerformingAttack += Attack;
            entity.Events.OnTakeDamage += (_) => Damaged();
            entity.Events.OnDying += Death;
            entity.Events.OnTargetLost += TargetLost;
            entity.Events.OnTargetAcquired += (_) => Target(true);

            //entity.Events.OnStartMoving += () => Debug.Log("Start Moving");
            //entity.Events.OnStopMoving += () => Debug.Log("Stop Moving");
        }

        protected override void OnDisable()
        {
            entity.Events.OnStartMoving -= () => Walk(true);
            entity.Events.OnStopMoving -= () => Walk(false);
            entity.Events.OnPerformingAttack -= Attack;
            entity.Events.OnTakeDamage -= (_) => Damaged();
            entity.Events.OnDying -= Death;
            entity.Events.OnTargetLost -= TargetLost;
            entity.Events.OnTargetAcquired -= (_) => Target(true);
        }

        private void Walk(bool version)
        {
            animator.SetInteger(Moving, version ? 1 : 0);
        }

        private void Attack()
        {
            Target(true);
            animator.SetInteger(Moving, 2);
        }

        private void Target(bool version)
        {
            animator.SetInteger(Battle, version ? 1 : 0);
        }

        private void TargetLost()
        {
            Target(false);
        }

        private void Damaged()
        {
            Target(true);
            StartCoroutine(SubDamaged());
        }

        private void Death()
        {
            animator.SetInteger(Moving, 14);
        }

        private IEnumerator SubDamaged()
        {
            var prevMoving = animator.GetInteger(Moving);
            animator.SetInteger(Moving, 8);
            yield return new WaitForSeconds(0.1f);
            
            if(animator.GetInteger(Moving) == 8) animator.SetInteger(Moving, prevMoving);
        }
    }
}