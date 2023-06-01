using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Entities.Player.Upgrades
{
    [CreateAssetMenu(menuName = "ScriptableObjects/UpgradeData")]
    public class UpgradeData : ScriptableObject
    {
        [Serializable]
        public enum UpgradeValuesType
        {
            Damages, Heal, Defense, Speed, Time, Other
        } 
        
        [Serializable]
        public struct StageValuesHolder
        {
            public UpgradeValuesType valuesType;
            public float fixedValue;
            [Range(0f,2f)] public float percentageValue;

            public void Deconstruct(out UpgradeValuesType type, out float fValue, out float pValue)
            {
                type = valuesType;
                fValue = percentageValue;
                pValue = percentageValue;
            }
        }
        
        [Serializable]
        public struct UpgradeStage
        {
            [Header("Infos")]
            
            public string upgradeName;
            [TextArea] public string description;
            [TextArea] public string technicalDescription;
            public Sprite icon;
            [Tooltip("Indicate if the values aren't added dynamically")] public bool isStatic;

            public List<StageValuesHolder> values;

        }
        
        public World.WorldType worldType;
        public List<UpgradeStage> stages;
    }
}