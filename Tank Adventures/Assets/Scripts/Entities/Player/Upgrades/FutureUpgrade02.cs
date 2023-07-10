using System;
using System.Collections;
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
        [SerializeField] private float cooldown = 0.2f;
        [SerializeField] private float speedFloor = 5f;
        private bool _coolingDown;
        
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

        private void DamageOnCollision(Collision collision)
        {
            if (_coolingDown) return;
            
            var target = collision.transform;
            
            if (!manager.ThisEntity.TryGetComponent(out Rigidbody rb)) return;
            if (!target.TryGetComponent(out IDamageable damageable)) return;

            var speed = collision.relativeVelocity.magnitude;

            if (speed < speedFloor) return;
            
            var vDamages = GetValues(UpgradeData.UpgradeValuesType.Damages);
            if (vDamages == null) return;
            
            var fDamages = vDamages.Value.fixedValue;
            var pDamages = vDamages.Value.percentageValue;
            var totalDamages = speed * (fDamages + pDamages);

            Debug.Log($"Attack on collision with damages {totalDamages} [speed : {speed} * damages : {fDamages + pDamages}]");
            
            damageable.TakeDamages(totalDamages);
            
            StartCoroutine(Cooldown());
        }

        private IEnumerator Cooldown()
        {
            _coolingDown = true;
            yield return new WaitForSeconds(cooldown);
            _coolingDown = false;
        }
    }
}