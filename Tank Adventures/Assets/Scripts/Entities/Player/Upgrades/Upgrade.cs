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
        #region Attributes
        
        [SerializeField] protected UpgradeData data;

        private World.WorldType _worldType;
        private bool _isStatic = true;
        protected int Level;
        protected UpgradeData.UpgradeStage CurrentStage;
        
        #endregion
        
        #region Properties
        
        [HideInInspector] public PlayerUpgradesManager manager;
        public World.WorldType ThisWorldType => _worldType;
        public bool ThisIsStatic => _isStatic;
        public UpgradeData Data => data;
        public int ThisLevel => Level;

        #endregion
        
        #region Methods
        
        #region Mono Behaviours
        protected void Awake()
        {
            if (data != null)
            {
                _worldType = data.worldType;
                if(data.stages.Count > 0) CurrentStage = data.stages[0];
                else Debug.LogWarning($"Upgrade {name} created without stages");
            }
            else
            {
                Debug.LogWarning("Upgrade created without data");
            }
        }

        #endregion 
        
        #region Public Methods
        
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
            Debug.Log($"Update {name} updating from level {Level} to {Level+1}");
            Level++;
            if (Level > data.stages.Count)
            {
                Debug.LogWarning("Try to level up while not enough stages");
                return;
            }
            
            CurrentStage = data.stages[Level];
            _isStatic = CurrentStage.isStatic;
            if(_isStatic) ApplyValues();
            
            UpgradeLevelUp();
        }
        
        #endregion

        #region Protected Methods

        

        

        protected abstract void UpgradeObtained();
        protected abstract void UpgradeUpdate();
        protected abstract void UpgradeLevelUp();

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

        #endregion
        
        #region Private Methods
        
        private void ApplyValues()
        {
            for (var i = 0; i < CurrentStage.values.Count; i++)
            {
                //Check for previous values
                var prevFvalue = 0f;
                var prevPvalue = 0f;
                if (Level > 0)
                {
                    var prevStage = data.stages[Level - 1];
                    if (prevStage.isStatic && i < prevStage.values.Count)
                    {
                        prevFvalue = prevStage.values[i].fixedValue;
                        prevPvalue = prevStage.values[i].percentageValue;
                    }
                }

                Debug.Log($"Apply Values : prev_values = {prevFvalue}, {prevPvalue}");
                
                //Setup current values
                var values = CurrentStage.values[i];
                var type = values.valuesType;
                var fvalue = values.fixedValue - prevFvalue;
                var pvalue = (1.0f + values.percentageValue)/(1.0f + prevPvalue) - 1.0f;

                //Apply depending on type
                switch (type)
                {
                    case UpgradeData.UpgradeValuesType.Damages:
                        if (fvalue > 0) manager.ThisEntity.Events.OnImproveDamagesFixed?.Invoke(fvalue);
                        if (pvalue > 0) manager.ThisEntity.Events.OnImproveDamages?.Invoke(pvalue);
                        break;
                    case UpgradeData.UpgradeValuesType.Heal:
                        if (fvalue > 0) manager.ThisEntity.Events.OnImproveHealFixed?.Invoke(fvalue);
                        if (pvalue > 0) manager.ThisEntity.Events.OnImproveHeal?.Invoke(pvalue);
                        break;
                    case UpgradeData.UpgradeValuesType.Defense:
                        if (fvalue > 0) manager.ThisEntity.Events.OnImproveDefenseFixed?.Invoke(fvalue);
                        if (pvalue > 0) manager.ThisEntity.Events.OnImproveDefense?.Invoke(pvalue);
                        break;
                    case UpgradeData.UpgradeValuesType.Speed:
                        Debug.Log($"Improve Speed by {pvalue*100}%");
                        if (fvalue > 0) manager.ThisEntity.Events.OnImproveSpeedFixed?.Invoke(fvalue);
                        if (pvalue > 0) manager.ThisEntity.Events.OnImproveSpeed?.Invoke(pvalue);
                        break;
                    case UpgradeData.UpgradeValuesType.Time:
                        break;
                    case UpgradeData.UpgradeValuesType.Other:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        #endregion
        #endregion
    }
}