using System.Collections.Generic;
using UnityEngine;

namespace World.RhythmPackage
{
    [CreateAssetMenu(menuName = "ScriptableObjects/RhythmMap")]
    public class RhythmMap : ScriptableObject
    {
        public float speed;
        public float totalTime;
        public List<RhythmMapObject> objects;
    }
}