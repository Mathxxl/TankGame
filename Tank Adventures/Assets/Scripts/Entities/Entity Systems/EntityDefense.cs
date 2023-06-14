using System;
using UnityEngine;

namespace Entities.Entity_Systems
{
    /// <summary>
    /// Manages Defense for entities
    /// </summary>
    public class EntityDefense : EntitySystem
    {
        private float _defense;

        public float Defense
        {
            get => _defense;
            set
            {
                _defense = value;
                entity.Events.OnDefenseChanged?.Invoke(_defense);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _defense = entity.id.defense;
        }

        private void OnEnable()
        {
            entity.Events.OnImproveDefenseFixed += ImproveDefenseFixed;
            entity.Events.OnImproveDefense += ImproveDefense;
        }

        private void OnDisable()
        {
            entity.Events.OnImproveDefenseFixed -= ImproveDefenseFixed;
            entity.Events.OnImproveDefense -= ImproveDefense;
        }

        private void ImproveDefenseFixed(float value)
        {
            Defense += value;
        }

        private void ImproveDefense(float value)
        {
            Defense *= 1.0f + value;
        }
    }
}
