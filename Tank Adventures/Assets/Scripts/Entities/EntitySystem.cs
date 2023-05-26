using UnityEngine;

namespace Entities
{
    /// <summary>
    /// Class for Entity Systems. Entity Systems are linked to an entity and can be freely added or remove without impacting other functionalities
    /// </summary>
    public abstract class EntitySystem : MonoBehaviour
    {
        [SerializeField] protected Entity entity;

        protected virtual void Awake()
        {
            if(entity == null) entity = transform.root.GetComponentInChildren<Entity>();
        }
    }
}
