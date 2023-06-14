namespace Utilities
{
    /// <summary>
    /// Encapsulate a modification on a float value with its mode
    /// </summary>
    public struct SModif
    {
        public float Value;
        public ValueAppliedMode Mode;

        public SModif(float v, ValueAppliedMode m = ValueAppliedMode.Fixed)
        {
            Value = v;
            Mode = m;
        }
    }
}