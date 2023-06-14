using System;
using Interfaces;
using UnityEngine;

namespace Entities.Player.Upgrades
{
    /// <summary>
    /// Upgrade 02 of Future World
    /// </summary>
    /// <remarks>
    /// Stage 1 : Add damages on collision with entities
    /// Stage 2 : Increase damages of collisions
    /// Stage 3 : Increase damages of collisions
    /// </remarks>
    public class FutureUpgrade02 : Upgrade
    {
        protected override void UpgradeObtained()
        {
            manager.ThisEntity.Events.OnCollision += DamageOnCollision;
        }

        protected override void UpgradeUpdate()
        {
            
        }

        protected override void UpgradeLevelUp()
        {
            
        }

        private void OnDisable()
        {
            if (manager == null) return;
            
            manager.ThisEntity.Events.OnCollision -= DamageOnCollision;
        }

        private void DamageOnCollision(Transform target)
        {
            if (!manager.ThisEntity.TryGetComponent(out Rigidbody rb)) return;
            if (!target.TryGetComponent(out IDamageable damageable)) return;
            
            var speed = rb.velocity.magnitude;
            var vDamages = GetValues(UpgradeData.UpgradeValuesType.Damages);
            if (vDamages == null) return;
            
            var fDamages = vDamages.Value.fixedValue;
            var pDamages = vDamages.Value.percentageValue;
            var totalDamages = speed * (fDamages + pDamages);

            damageable.TakeDamages(totalDamages);
        }
    }
}