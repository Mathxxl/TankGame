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
        public float lifePoint = 0f;
    }
}
