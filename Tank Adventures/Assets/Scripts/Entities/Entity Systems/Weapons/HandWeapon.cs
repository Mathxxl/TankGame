using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;
using Utilities;

namespace Entities.Entity_Systems.Weapons
{
    public class HandWeapon : Weapon
    {
        [SerializeField] private EntityDetector attackZone;
        [SerializeField] private float attackPerformingTime = 1.06f;

        protected override void Attack()
        {
            StartCoroutine(Attacking());
        }

        protected override void AttackTarget(Transform target, Subweapon wubweapon = null)
        {
            //Impact - check if damageable by projectile
            if (!target.gameObject.TryGetComponent(out IDamageable damageable) || !AttackableTags.Any(t => target.gameObject.CompareTag(t))) return;
            
            entity.Events.OnAttack?.Invoke(target);
            
            damageable.TakeDamages(damages);

            //if linked to an entity, summon the OnAttacked event on self
            if (target.gameObject.TryGetComponent(out Entity targetEntity))
            {
                targetEntity.Events.OnAttacked?.Invoke(entity.transform);
            }
        }

        private void SubAttackTarget(Transform target)
        {
            AttackTarget(target);
        }

        protected override void Awake()
        {
            base.Awake();
            attackZone.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (attackZone == null) return;
            attackZone.OnEntityDetected += SubAttackTarget;
        }

        private void OnDisable()
        {
            if (attackZone == null) return;
            attackZone.OnEntityDetected -= SubAttackTarget;
        }

        private IEnumerator Attacking()
        {
            attackZone.gameObject.SetActive(true);
            yield return new WaitForSeconds(attackPerformingTime);
            attackZone.gameObject.SetActive(false);
        }
    }
}