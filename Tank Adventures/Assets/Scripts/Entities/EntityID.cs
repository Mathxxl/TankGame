using UnityEngine;

namespace Entities
{
    /// <summary>
    /// Stores data for entity
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/EntityID")]
    public class EntityID : ScriptableObject
    {
        public string entityName;
        public float lifePoint = 10f;
        public float attack = 1f;
        [Tooltip("Cooldown between attacks")] public float attackSpeed = 0.2f;
        public float defense = 0f;
        public float speed = 200f;
    }
}
