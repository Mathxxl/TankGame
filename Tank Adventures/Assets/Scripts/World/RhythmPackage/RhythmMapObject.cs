using System;
using UnityEngine;

namespace World.RhythmPackage
{
    [Serializable]
    public struct RhythmMapObject
    {
        public RhythmMapObjectType type;
        [Range(0, 1)] public float timing;
        [Range(0, 1)] public float x;
        [Range(0, 1)] public float y;
    }
}