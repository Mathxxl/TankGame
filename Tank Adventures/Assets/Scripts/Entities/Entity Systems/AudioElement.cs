using System;
using UnityEngine;

namespace Entities.Entity_Systems
{
    [Serializable]
    public struct AudioElement
    {
        public string name;
        public AudioClip clip;
        public AudioSource source;
    }
}