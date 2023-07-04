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

        public Dictionary<RhythmMapObjectType, GameObject> _dicObj { get; private set; }

        private void Awake()
        {
            SetDic();
        }

        private void SetDic()
        {
            _dicObj = new Dictionary<RhythmMapObjectType, GameObject>();
            foreach (var ro in objectsWithType)
            {
                _dicObj.Add(ro.type, ro.obj);
            }
        }
    }
}