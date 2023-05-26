using UnityEngine;
using World;

namespace Entities.Player.Ultimate
{
    [CreateAssetMenu(menuName = "ScriptableObjects/UltimateData")]
    public class UltimateData : ScriptableObject
    {
        #region Data

        [Header("Category")] [Space(10)] 
        
        public WorldType type;
        
        [Header("Main Data")][Space(10)]
        
        [Range(0,600)] public float cooldown;
        [Range(0,10000)] public float damages;
        [Range(0, 1)] public float damagesPercentage;
        [Range(0,600)] public float duration;

        [Space(20)] [Header("Additional Data")] [Space(10)] 
        
        [Range(0, 10000)] public float heal;
        [Range(0, 1)] public float healPercentage;
        
        #endregion
    }
}