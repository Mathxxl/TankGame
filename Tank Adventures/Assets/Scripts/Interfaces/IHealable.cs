using Utilities;

namespace Interfaces
{
    /// <summary>
    /// Interface for objects that can be healed or repaired
    /// </summary>
    public interface IHealable
    {
        public void Heal(float value, ValueAppliedMode mode);
    }
}