using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Player.Upgrades;
using UnityEngine;
using World;

namespace GameManagers
{
    /// <summary>
    /// Manages the Upgrades of the game
    /// </summary>
    public class GameUpgradeManager : Manager
    {
        [SerializeField] private List<Upgrade> gameAvailableUpgrades;
        private Dictionary<WorldType, List<Upgrade>> _upgradeDictionary;

        private void Awake()
        {
            StartCoroutine(ToSetupDictionary());
        }

        private void OnEnable()
        {
            if (gameManager == null) return;

            gameManager.Events.OnUpgradeChosen += CheckUpgradeLevel;
        }

        private void OnDisable()
        {
            if (gameManager == null) return;
            
            gameManager.Events.OnUpgradeChosen -= CheckUpgradeLevel;
        }

        private void SetupDictionary()
        {
            _upgradeDictionary = new Dictionary<WorldType, List<Upgrade>>();

            foreach (var up in gameAvailableUpgrades)
            {
                if (!_upgradeDictionary.ContainsKey(up.ThisWorldType))
                {
                    _upgradeDictionary.Add(up.ThisWorldType, new List<Upgrade>());
                }
                _upgradeDictionary[up.ThisWorldType].Add(up);
            }
        }

        private void CheckUpgradeLevel(Upgrade up)
        {
            if (up == null) return;
            
            Debug.Log($"Checking Upgrade {up.name} : Level = {up.ThisLevel} / Count = {up.Data.stages.Count}");
            
            if (up.ThisLevel+1 != up.Data.stages.Count) return;
            
            _upgradeDictionary[up.ThisWorldType].Remove(up);
            CheckUpgradeCount(up.ThisWorldType);
        }
        
        private void CheckUpgradeCount(WorldType type)
        {
            Debug.Log($"Check Upgrade Count : count = {_upgradeDictionary[type].Count} for type {type}");
            
            if (_upgradeDictionary[type].Count == 0)
            {
                gameManager.Events.OnAllUpgradesGottenForWorld?.Invoke(type);
            }
        }
        
        /// <summary>
        /// Returns all possible upgrades for worldType
        /// </summary>
        /// <param name="worldType"></param>
        /// <returns></returns>
        public List<Upgrade> GetUpgradeFromWorld(WorldType worldType)
        {
            if (_upgradeDictionary.ContainsKey(worldType)) return _upgradeDictionary[worldType];
            
            Debug.LogWarning($"No upgrades found for world {worldType}");
            return null;
        }

        private IEnumerator ToSetupDictionary()
        {
            yield return null;
            SetupDictionary();
        }
    }
}