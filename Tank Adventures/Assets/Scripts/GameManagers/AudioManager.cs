using System.Collections.Generic;
using Audio;
using UnityEngine;

namespace GameManagers
{
    /// <summary>
    /// Manages in game sounds //=> séparer music manager et audio manager pour les bruitages ?
    /// </summary>
    public class AudioManager : Manager
    {
        [SerializeField] private List<MusicContainer> musics;
        private MusicContainer _currentMusic;
        public MusicContainer CurrentMusic => _currentMusic;
    }
}