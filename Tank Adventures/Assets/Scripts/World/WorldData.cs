using System.Collections.Generic;
using UnityEngine;

namespace World
{
    /// <summary>
    /// Data for worlds type
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/WorldData")]
    public class WorldData : ScriptableObject
    {
        public WorldType type;
        public string worldName;
        [TextArea] public string description;
        [Tooltip("Type of upgrades that the player may get in this world")] public List<string> possibleUpgrades;
        public Sprite icon;
    }
}