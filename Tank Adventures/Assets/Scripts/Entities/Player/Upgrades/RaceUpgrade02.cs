using System;
using UnityEngine;

namespace Entities.Player.Upgrades
{
    /// <summary>
    /// Upgrade 02 of Race World
    /// </summary>
    /// <remarks>
    /// Stage 1 : Increase damages with speed <br/>
    /// Stage 2 : Increase damages with speed <br/>
    /// Stage 3 : Increase damages with speed
    /// </remarks>
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
            if(manager != null) manager.ThisEntity.Events.OnPerformingAttack -= DamagesWithSpeed;
        }

        private void DamagesWithSpeed()
        {
            //Get rigidbody
            if (!manager.ThisEntity.TryGetComponent(out Rigidbody rb))
            {
                Debug.Log("NO RB");
                return;
            }

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