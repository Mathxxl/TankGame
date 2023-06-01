using System;
using UnityEngine;

namespace Entities.Player.Upgrades
{
    public class RaceUpgrade02 : Upgrade
    {
        protected override void UpgradeObtained()
        {
            manager.ThisEntity.Events.OnPerformingAttack += DamagesWithSpeed;
        }

        protected override void UpgradeUpdate()
        {
            //Nothing
        }

        protected override void UpgradeLevelUp()
        {
            //Nothing
        }

        private void OnDisable()
        {
            manager.ThisEntity.Events.OnPerformingAttack -= DamagesWithSpeed;
        }

        private void DamagesWithSpeed()
        {
            //Get rigidbody
            if (!manager.ThisEntity.TryGetComponent(out Rigidbody rb)) return;
            //Get Speed
            var speed = rb.velocity.magnitude;
            Debug.Log($"Damages with speed : speed = {speed}");
            //Get factor
            var factor = GetValues(UpgradeData.UpgradeValuesType.Damages);
            //Call event improve damages on hit
            if (factor != null)
                manager.ThisEntity.Events.OnImproveDamageForOneHit?.Invoke(speed * factor.Value.fixedValue);
        }
    }
}