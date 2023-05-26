using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Player.Upgrades
{
    [CreateAssetMenu(menuName = "ScriptableObjects/UpgradeData")]
    public class UpgradeData : ScriptableObject
    {
        [Serializable]
        public struct UpgradeStage
        {
            public string upgradeName;
            public string description;
            public Sprite icon;
            public bool isStatic;
        }
        
        public World.WorldType worldType;
        public List<UpgradeStage> stages;

    }
}