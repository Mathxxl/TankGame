using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Player.Player_Systems;
using UnityEngine;

namespace Entities.Player.Upgrades
{
    /// <summary>
    /// Represent upgrades
    /// NOTE : les upgrades seront probablements des prefabs à terme
    /// </summary>
    public abstract class Upgrade : MonoBehaviour
    {
        [SerializeField] protected UpgradeData data;

        private World.WorldType _worldType;
        private bool _isStatic = true;
        protected int Level;
        protected UpgradeData.UpgradeStage CurrentStage;
        
        [HideInInspector] public PlayerUpgradesManager manager;
        public World.WorldType ThisWorldType => _worldType;
        public bool ThisIsStatic => _isStatic;
        public UpgradeData Data => data;
        public int ThisLevel => Level;

        protected void Awake()
        {
            if (data != null)
            {
                _worldType = data.worldType;
                if(data.stages.Count > 0) CurrentStage = data.stages[0];
                else Debug.LogWarning("Upgrade created without stages");
            }
            else
            {
                Debug.LogWarning("Upgrade created without data");
            }
        }

        public void OnUpgradeObtained()
        {
            UpgradeObtained();
            
            CurrentStage = data.stages[Level];
            _isStatic = CurrentStage.isStatic;
            if (_isStatic)
            {
                ApplyValues();
            }
        }

        public void OnUpgradeUpdate()
        {
            UpgradeUpdate();
        }

        public void OnUpgradeLevelUp()
        {
            Level++;
            if (Level > data.stages.Count)
            {
                Debug.LogWarning("Try to level up while not enough stages");
                return;
            }
            
            CurrentStage = data.stages[Level];
            _isStatic = CurrentStage.isStatic;
            
            UpgradeLevelUp();
        }
        
        protected virtual void UpgradeObtained(){}
        protected virtual void UpgradeUpdate(){}
        protected virtual void UpgradeLevelUp(){}

        protected UpgradeData.StageValuesHolder? GetValues(UpgradeData.UpgradeValuesType type)
        {
            foreach (var val in CurrentStage.values.Where(val => val.valuesType == type))
            {
                return val;
            }
            return null;
        }

        protected List<UpgradeData.StageValuesHolder> GetAllValues(UpgradeData.UpgradeValuesType type)
        {
            return CurrentStage.values.Where(val => val.valuesType == type).ToList();
        }

        protected void ApplyValues()
        {
            foreach(var (type, fvalue, pvalue) in CurrentStage.values)
            {
                switch (type)
                {
                    case UpgradeData.UpgradeValuesType.Damages:
                        if (fvalue > 0) manager.ThisEntity.Events.OnImproveDamagesFixed?.Invoke(fvalue);
                        if (pvalue > 0) manager.ThisEntity.Events.OnImproveDamages?.Invoke(pvalue);
                        break;
                    case UpgradeData.UpgradeValuesType.Heal:
                        if (fvalue > 0) manager.ThisEntity.Events.OnImproveHealFixed?.Invoke(fvalue);
                        if (pvalue > 0) manager.ThisEntity.Events.OnImproveHeal?.Invoke(pvalue);;
                        break;
                    case UpgradeData.UpgradeValuesType.Defense:
                        if (fvalue > 0) manager.ThisEntity.Events.OnImproveDefenseFixed?.Invoke(fvalue);
                        if (pvalue > 0) manager.ThisEntity.Events.OnImproveDefense?.Invoke(pvalue);;
                        break;
                    case UpgradeData.UpgradeValuesType.Speed:
                        if (fvalue > 0) manager.ThisEntity.Events.OnImproveSpeedFixed?.Invoke(fvalue);
                        if (pvalue > 0) manager.ThisEntity.Events.OnImproveSpeed?.Invoke(pvalue);;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}