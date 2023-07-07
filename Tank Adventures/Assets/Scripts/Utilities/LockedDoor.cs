using System;
using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Utilities
{
    public abstract class LockedDoor : MonoBehaviour
    {
        [SerializeField] protected List<MortalEntity> entities;

        protected void OnEnable()
        {
            foreach (var entity in entities)
            {
                entity.Events.OnDeath += () => OnEntityDeath(entity);
            }
        }
        
        protected void OnDisable()
        {
            foreach (var entity in entities)
            {
                entity.Events.OnDeath -= () => OnEntityDeath(entity);
            }
        }

        protected void OnEntityDeath(MortalEntity entity)
        {
            entities.Remove(entity);
            if (entities.Count == 0)
            {
                OpenDoor();
            }
        }

        protected abstract void OpenDoor();
    }
}