using System;

namespace Utilities
{
    /// <summary>
    /// Act as a float with several functions to add a value with different
    /// </summary>
    /// <remarks>
    /// - Fixed mode : the value is added as a normal addition ( x = x + a ) <br/>
    /// - Percentage mode : the value is added as a percentage of the original value ( x = x + a% * x)
    /// </remarks>
    public class SFloat
    {
        public float Value;

        private float AddFixed(float v)
        {
            return Value += v;
        }

        private float AddPercentage(float v)
        {
            return Value *= (1.0f + v);
        }

        private float AddPercentageOfMax(float v, float max)
        {
            return Value += v * max;
        }

        public float Add(float addValue, ValueAppliedMode mode = ValueAppliedMode.Fixed, float max = 0)
        {
            switch (mode)
            {
                case ValueAppliedMode.Fixed:
                    return AddFixed(addValue);
                case ValueAppliedMode.Percentage:
                    return AddPercentage(addValue);
                case ValueAppliedMode.Inverse:
                    break;
                case ValueAppliedMode.Negative:
                    break;
                case ValueAppliedMode.PercentageOfMax:
                    return AddPercentageOfMax(addValue, max);
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

            return 0;
        }
    }
}