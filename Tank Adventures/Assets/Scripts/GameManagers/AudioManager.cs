using System.Collections.Generic;
using Audio;
using UnityEngine;

namespace GameManagers
{
    public class AudioManager : Manager
    {
        [SerializeField] private List<MusicContainer> musics;
        private MusicContainer _currentMusic;
        public MusicContainer CurrentMusic => _currentMusic;
    }
}