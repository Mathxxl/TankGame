using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Entities.Entity_Systems
{
    /// <summary>
    /// Manages health for entity
    /// </summary>
    public class EntityHealth : EntitySystem
    {
        private readonly SFloat _maxHealth = new();
        private readonly SFloat _health = new();
    
        public float Health
        {
            get => _health.Value;
            set
            {
                _health.Value = (value > _maxHealth.Value) ? _maxHealth.Value : value;
                entity.Events.OnHealthChanged?.Invoke(_health.Value);
            }
        }

        public float MaxHealth
        {
            get => _maxHealth.Value;
            set
            {
                _maxHealth.Value = value;
                entity.Events.OnMaxHealthChanged?.Invoke(_maxHealth.Value);
            }
        }

        protected void Start()
        {
            if (entity.id.lifePoint == 0) return;
            
            var value = entity.id.lifePoint;
            MaxHealth = value;
            Health = value;
        }

        public void AddHealth(float val, ValueAppliedMode mode)
        {
            var heal = _health.Add(val, mode, (mode == ValueAppliedMode.PercentageOfMax) ? MaxHealth : 0);
            Health = (Health > MaxHealth) ? MaxHealth : Health;
            
            if (heal > 0)
            {
                entity.Events.OnHealed?.Invoke(heal);
            }
            Debug.Log($"_health = {_health.Value}");
            entity.Events.OnHealthChanged?.Invoke(_health.Value);
        }

        public void AddHealth(IEnumerable<SModif> values)
        {
            foreach (var m in values)
            {
                AddHealth(m);
            }
        }
        
        public void AddHealth(SModif modif)
        {
            AddHealth(modif.Value, modif.Mode);
        }

        public void AddMaxHealth(float val, ValueAppliedMode mode)
        {
            _maxHealth.Add(val, mode);
            entity.Events.OnMaxHealthChanged?.Invoke(_maxHealth.Value);
        }
    }
}
