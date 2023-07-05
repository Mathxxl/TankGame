using System;
using System.Collections.Generic;
using UnityEngine;

namespace World.RhythmPackage
{
    public class RhythmReferencesSystem : MonoBehaviour
    {
        [Serializable]
        private struct RhythmObj
        {
            public RhythmMapObjectType type;
            public GameObject obj;
        }
        [SerializeField] private List<RhythmObj> objectsWithType;

        public Dictionary<RhythmMapObjectType, GameObject> DicObj { get; private set; }

        private void Awake()
        {
            SetDic();
        }

        private void SetDic()
        {
            DicObj = new Dictionary<RhythmMapObjectType, GameObject>();
            foreach (var ro in objectsWithType)
            {
                DicObj.Add(ro.type, ro.obj);
            }
        }
    }
}