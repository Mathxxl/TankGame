using System.Collections;
using UnityEngine;
using Utilities;

namespace Entities.Player.Upgrades
{
    /// <summary>
    /// Upgrade 01 of Horror World
    /// </summary>
    /// <remarks>
    /// Stage 1 : Heal when inflicting damages <br/>
    /// Stage 2 : Increase heal and increase total damages <br/>
    /// Stage 3 : Increase heal, damages and heal a part of total health when killing an enemy
    /// </remarks>
    public class HorrorUpgrade01 : Upgrade
    {
        private float _memFDamages;
        private float _memPDamages;

        private float _healFactor;
        private float _healKillFValue;
        private float _healKillPValue;
        
        protected override void UpgradeObtained()
        {
            //Values
            SetValues();
            
            //Events
            manager.ThisEntity.Events.OnAttack += OnAttackSomething;
        }

        protected override void UpgradeUpdate(){ }

        protected override void UpgradeLevelUp()
        {
            ImproveDamages();
            SetValues();
        }

        private void OnDisable()
        {
            if (manager == null) return;
            
            manager.ThisEntity.Events.OnAttack -= OnAttackSomething;
        }

        private void OnAttackSomething(Transform target)
        {
            if (target.gameObject.TryGetComponent(out Entity targetEntity)) //if an entity is attacked
            {
                StartCoroutine(FrameHeal(targetEntity)); //then heal each damages it takes on this frame => not ideal since we would like to heal only our own damages
            }
        }

        private void HealOnHit(float damages)
        {
            var totalHeal = damages * (_healFactor);
            var holder = new SModif(totalHeal);
            Debug.Log($"Heal on hit : {holder.Value}");
            manager.ThisEntity.Events.OnHeal?.Invoke(holder);
        }

        private void HealOnKill()
        {
            if (_healKillFValue == 0 && _healKillPValue == 0) return;

            manager.ThisEntity.Events.OnHeal?.Invoke(new SModif(_healKillFValue));
            manager.ThisEntity.Events.OnHeal?.Invoke(new SModif(_healKillPValue, ValueAppliedMode.PercentageOfMax));
        }

        private IEnumerator FrameHeal(Entity targetEntity)
        {
            targetEntity.Events.OnTakeDamage += HealOnHit;
            targetEntity.Events.OnDeath += HealOnKill;
            yield return null;
            targetEntity.Events.OnTakeDamage -= HealOnHit;
            targetEntity.Events.OnDeath -= HealOnKill;
        }

        private void ImproveDamages()
        {
            var damagesValue = GetValues(UpgradeData.UpgradeValuesType.Damages);
            if (damagesValue == null) return;
            var fDamages = damagesValue.Value.fixedValue;
            var pDamages = damagesValue.Value.percentageValue;

            if (fDamages > 0)
            {
                manager.ThisEntity.Events.OnImproveDamagesFixed?.Invoke(fDamages - _memFDamages);
            }

            if (pDamages > 0)
            {
                manager.ThisEntity.Events.OnImproveDamages?.Invoke(pDamages - _memPDamages);
            }

            _memFDamages = fDamages;
            _memPDamages = pDamages;
        }
        
        private void SetValues()
        {
            var healValue = GetAllValues(UpgradeData.UpgradeValuesType.Heal);
            if (healValue.Count <= 0) return;

            //Set heal on hit
            var value = healValue[0];

            if (value.Equals(null)) return;
            
            var fHealValue = value.fixedValue;
            var pHealValue = value.percentageValue;

            _healFactor = fHealValue + pHealValue;
            
            //Set heal on kill
            if (healValue.Count < 2) return;

            var valueKill = healValue[1];
            if (valueKill.Equals(null)) return;
            
            _healKillFValue = valueKill.fixedValue;
            _healKillPValue = valueKill.percentageValue;
            
            Debug.Log($"Heal kill value = {_healKillFValue}, {_healKillPValue}");
        }
    }

}