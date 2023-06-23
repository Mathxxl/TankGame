using System;
using Entities.Entity_Systems.Weapons;
using UnityEngine;

namespace Entities.Player.Upgrades
{
    /// <summary>
    /// Upgrade 02 of Horror World
    /// </summary>
    /// <remarks>
    /// Stage 1 : Add a zone of damages on projectile explosion <br/>
    /// Stage 2 : Increase zone size and damages/sec <br/>
    /// Stage 3 : Increase zone size and damages/sec
    /// </remarks>
    public class HorrorUpgrade02 : Upgrade
    {
        [SerializeField] private GameObject darkZonePrefab;

        private float _currentSize;
        private float _currentDamages;
        private float _currentLifetime;
        private float _currentRhythm;

        private WeaponManager _thisWeaponManager;

        protected override void UpgradeObtained()
        {
            //Components
            GetWeaponManager();
            
            //Events
            manager.ThisEntity.Events.OnAttack += AttackTarget;
            
            //Values
            GetCurrentParameters();
        }

        protected override void UpgradeUpdate()
        {
            
        }

        protected override void UpgradeLevelUp()
        {
            GetCurrentParameters();
        }

        private void OnDestroy()
        {
            if(manager!= null && manager.ThisEntity != null) manager.ThisEntity.Events.OnAttack -= AttackTarget;
        }

        /// <summary>
        /// Called on attacking something, create a dark zone with correct parameters
        /// </summary>
        private void AttackTarget(Transform target)
        {
            var dzobj = Instantiate(darkZonePrefab, target.transform.position, darkZonePrefab.transform.rotation);
            if (!dzobj.TryGetComponent(out DarkZone dz))
            {
                Debug.LogWarning("The darkzone prefab has no dark zone script");
                return;
            }
            dz.SetParameters(_currentSize, _currentDamages, _currentLifetime, _currentRhythm, new[] {manager.ThisEntity.tag});
            dz.StartDarkZone();
        }

        /// <summary>
        /// Setup the current dark zone parameters using upgrade data
        /// </summary>
        private void GetCurrentParameters()
        {
            var sizeHolder = GetValues(UpgradeData.UpgradeValuesType.Other);
            if (sizeHolder != null) _currentSize = sizeHolder.Value.fixedValue;

            var damagesHolder = GetValues(UpgradeData.UpgradeValuesType.Damages);
            if (damagesHolder != null)
            {
                _currentDamages = (damagesHolder.Value.percentageValue * _thisWeaponManager.CurrentWeapon.Damages) + damagesHolder.Value.fixedValue;
            }

            var timeHolder = GetAllValues(UpgradeData.UpgradeValuesType.Time);
            if (timeHolder == null) return;
            
            if (timeHolder.Count > 0)
            {
                _currentLifetime = timeHolder[0].fixedValue;
            }

            if (timeHolder.Count > 1)
            {
                _currentRhythm = timeHolder[1].fixedValue;
            }
        }

        private void GetWeaponManager()
        {
            var component = manager.ThisEntity.GetComponentInChildren<WeaponManager>();
            if (component == null)
            {
                Debug.LogWarning($"Entity {manager.ThisEntity.name} has no weapon manager");
                return;
            }

            _thisWeaponManager = component;
        }
        
    }
}