using UnityEngine;
using World;

namespace Audio
{
    [CreateAssetMenu(menuName = "ScriptableObjects/MusicContainer")]
    public class MusicContainer : ScriptableObject
    {
        public AudioClip music;
        public int bpm;
        public string musicName;
        public WorldType worldType;
    }
}