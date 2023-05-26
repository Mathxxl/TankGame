using System.Collections.Generic;
using Entities.Player.Upgrades;
using UnityEngine;

namespace GameManagers
{
    //Manages the Upgrades of the game
    public class GameUpgradeManager : Manager
    {
        [SerializeField] private List<Upgrade> gameAvailableUpgrades;
        private Dictionary<World.WorldType, List<Upgrade>> _upgradeDictionary;

        private void Awake()
        {
            SetupDictionary();
        }

        //TODO : ajouter event quand une upgrade est niveau max pour ne plus la prendre en compte -> envoyer un event quand il n'y a plus d'upgrades pour un monde donné
        
        private void SetupDictionary()
        {
            _upgradeDictionary = new Dictionary<World.WorldType, List<Upgrade>>();

            foreach (var up in gameAvailableUpgrades)
            {
                if (!_upgradeDictionary.ContainsKey(up.ThisWorldType))
                {
                    _upgradeDictionary.Add(up.ThisWorldType, new List<Upgrade>());
                }
                _upgradeDictionary[up.ThisWorldType].Add(up);
            }
        }
        
        public List<Upgrade> GetUpgradeFromWorld(World.WorldType worldType)
        {
            if (_upgradeDictionary.ContainsKey(worldType)) return _upgradeDictionary[worldType];
            
            Debug.LogWarning($"Try to get upgrade from undefined world {worldType}");
            return null;
        }
    }
}