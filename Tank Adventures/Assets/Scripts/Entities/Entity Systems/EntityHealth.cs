namespace Entities.Entity_Systems
{
    /// <summary>
    /// Manages health for entity
    /// </summary>
    public class EntityHealth : EntitySystem
    {
        private float _maxHealth;
        private float _health;
    
        public float Health
        {
            get => _health;
            set
            {
                _health = value;
                entity.Events.OnHealthChanged?.Invoke(_health);
            }
        }

        public float MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
                entity.Events.OnMaxHealthChanged?.Invoke(_maxHealth);
            }
        }

        protected void Start()
        {
            if (entity.id.lifePoint != 0)
            {
                var value = entity.id.lifePoint;
                MaxHealth = value;
                Health = value;
            }
        }
    }
}
