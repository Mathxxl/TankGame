using System;
using System.Collections.Generic;
using GameManagers;
using UnityEngine;
using World;

namespace Entities.Player.Player_Systems
{
    public class PlayerUltimateManager : EntitySystem
    {
        [SerializeField] private GameUltimateManager manager; 
        
        private Ultimate.Ultimate _currentUltimate;
        private List<Ultimate.Ultimate> _obtainedUltimates;

        protected override void Awake()
        {
            base.Awake();
            manager ??= entity.GameManager.GetComponentInChildren<GameUltimateManager>();
            _obtainedUltimates = new List<Ultimate.Ultimate>();
        }
        
        private void OnEnable()
        {
            entity.Events.OnFullUpgradeOnRoad += GetUltimate;
        }

        private void OnDisable()
        {
            entity.Events.OnFullUpgradeOnRoad -= GetUltimate;
        }

        private void GetUltimate(WorldType type)
        {
            Debug.Log($"Get Ultimate for type {type}");
            
            var ult = manager.GetUltimateFromType(type);
            if (ult == null)
            {
                Debug.LogWarning($"No ultimate found for type {type}");
                return;
            }
            _currentUltimate ??= ult;
            if(!_obtainedUltimates.Contains(ult)) _obtainedUltimates.Add(ult);
        }

        //TODO WHEN NEEDED
        public void ChangeCurrentUltimate()
        {
            
        }
    }
}