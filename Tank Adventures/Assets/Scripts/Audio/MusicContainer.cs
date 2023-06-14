using UnityEngine;
using World;

namespace Audio
{
    /// <summary>
    /// Scriptable Container for musics, includes useful data
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/MusicContainer")]
    public class MusicContainer : ScriptableObject
    {
        public AudioClip music;
        public int bpm;
        public string musicName;
        public WorldType worldType;
    }
}