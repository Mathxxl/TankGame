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

        /// <summary>
        /// Add an upgrade to the player
        /// </summary>
        /// <param name="upgrade"></param>
        public void ObtainUpgrade(Upgrade upgrade)
        {
            _upgrades.Add(upgrade);
            upgrade.manager = this;
            upgrade.OnUpgradeObtained();
            entity.Events.OnUpgradeObtained?.Invoke(upgrade);
        }

        /// <summary>
        /// Level up an upgrade
        /// </summary>
        /// <param name="upgrade"></param>
        public void LevelUpUpgrade(Upgrade upgrade)
        {
            var idx = _upgrades.IndexOf(upgrade);
            if (idx > -1)
            {
                _upgrades[idx].OnUpgradeLevelUp();
                entity.Events.OnUpgradeLeveledUp?.Invoke(upgrade);
                if (upgrade.ThisLevel == upgrade.Data.stages.Count - 1) entity.Events.OnFullUpgradeOnRoad?.Invoke(upgrade.ThisWorldType);
            }
            else Debug.LogWarning("Try to level up upgrade that is not contained in the manager");
        }

        /// <summary>
        /// Returns the level of the given upgrade
        /// </summary>
        /// <param name="upgrade"></param>
        /// <returns></returns>
        public int GetLevelFromUpgrade(Upgrade upgrade)
        {
            var idx = _upgrades.IndexOf(upgrade);
            if (idx > -1) return _upgrades[idx].ThisLevel;
            return -1;
        }

        #endregion
    }
}
