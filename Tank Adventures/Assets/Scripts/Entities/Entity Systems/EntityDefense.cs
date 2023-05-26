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
    }
}
