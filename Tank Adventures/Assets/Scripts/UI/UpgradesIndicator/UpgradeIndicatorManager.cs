using System;
using System.Collections.Generic;
using Entities;
using Entities.Player.Upgrades;
using UnityEngine;

namespace UI.UpgradesIndicator
{
    public class UpgradeIndicatorManager : EntitySystem
    {
        [SerializeField] private GameObject upgradeIndicatorPrefab;
        [SerializeField] private GameObject layout;
        private Dictionary<Upgrade, GameObject> _indicators;

        protected override void Awake()
        {
            base.Awake();
            _indicators = new Dictionary<Upgrade, GameObject>();
        }
        
        private void OnEnable()
        {
            entity.Events.OnUpgradeObtained += UpgradeObtained;
            entity.Events.OnUpgradeLeveledUp += UpgradeLeveledUp;
        }

        private void OnDestroy()
        {
            entity.Events.OnUpgradeObtained -= UpgradeObtained;
            entity.Events.OnUpgradeLeveledUp -= UpgradeLeveledUp;
        }

        private void UpgradeObtained(Upgrade upgrade)
        {
            Debug.Log("UpgradeObtained call");
            
            var up = Instantiate(upgradeIndicatorPrefab, layout.transform);

            if (up.TryGetComponent(out UpgradeIndicator indicator))
            {
                indicator.Setup(upgrade);
                _indicators.Add(upgrade, up);
            }
            else
            {
                Debug.LogError("Prefab for indicators has no script named Upgrade Indicator");
                Destroy(up);
                return;
            }
        }

        private void UpgradeLeveledUp(Upgrade upgrade)
        {
            var obj = _indicators[upgrade];
            if (obj == null) return;

            if (obj.TryGetComponent(out UpgradeIndicator indicator))
            {
                indicator.LevelUp();
            }
        }
        
        
        
    }
}