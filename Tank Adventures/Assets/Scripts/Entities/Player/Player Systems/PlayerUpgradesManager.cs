using System.Collections.Generic;
using System.Linq;
using Entities.Player.Upgrades;
using UnityEngine;

namespace Entities.Player.Player_Systems
{
    /// <summary>
    /// Manages the upgrades of the player
    /// </summary>
    public class PlayerUpgradesManager : EntitySystem
    {
        #region Attributes

        private List<Upgrade> _upgrades;
        [HideInInspector] public UpgradeEvents Events = new UpgradeEvents();
        public Entity ThisEntity => entity;

        #endregion

        #region Methods

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        protected override void Awake()
        {
            base.Awake();
            _upgrades = new List<Upgrade>();
        }

        protected void Update()
        {
            foreach (var upgrade in _upgrades.Where(upgrade => !upgrade.ThisIsStatic))
            {
                upgrade.OnUpgradeUpdate();
            }
        }

        public void ObtainUpgrade(Upgrade upgrade)
        {
            _upgrades.Add(upgrade);
            upgrade.manager = this;
            upgrade.OnUpgradeObtained();
        }

        public void LevelUpUpgrade(Upgrade upgrade)
        {
            var idx = _upgrades.IndexOf(upgrade);
            if (idx > -1)
            {
                _upgrades[idx].OnUpgradeLevelUp();
                if (upgrade.ThisLevel == upgrade.Data.stages.Count - 1) entity.Events.OnFullUpgradeOnRoad?.Invoke(upgrade.ThisWorldType);
            }
            else Debug.LogWarning("Try to level up upgrade that is not contained in the manager");
        }

        public int GetLevelFromUpgrade(Upgrade upgrade)
        {
            var idx = _upgrades.IndexOf(upgrade);
            if (idx > -1) return _upgrades[idx].ThisLevel;
            return -1;
        }

        #endregion
    }
}