using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using UnityEngine;
using World;

namespace GameManagers
{
    /// <summary>
    /// Manages in game sounds //=> séparer music manager et audio manager pour les bruitages ?
    /// </summary>
    public class AudioManager : Manager
    {
        [SerializeField] private AudioSource audioSource;
        
        [SerializeField] private List<MusicContainer> musics;
        private MusicContainer _currentMusic;
        public MusicContainer CurrentMusic => _currentMusic;

        private void Start()
        {
            if (audioSource == null) return;
            audioSource.Play();
            Debug.Log($"Playing : {audioSource.isPlaying}");
            
        }

        /*private void Update()
        {
            Debug.Log($"Playing : {audioSource.isPlaying}");
        }*/

        private void OnEnable()
        {
            gameManager.Events.OnWorldChanged += GetMusicOfWorld;
            gameManager.Events.OnLevelReached += PlayCurrentMusic;
        }

        private void OnDisable()
        {
            gameManager.Events.OnWorldChanged -= GetMusicOfWorld;
            gameManager.Events.OnLevelReached -= PlayCurrentMusic;
        }

        private void PlayCurrentMusic()
        {
            if (audioSource == null) return;
            audioSource.Stop();

            if (_currentMusic == null) return;
            audioSource.clip = _currentMusic.music;

            audioSource.Play();
        }

        private void GetMusicOfWorld(WorldType type)
        {
            Debug.Log($"Get music of type {type}");
            
            foreach (var m in musics.Where(m => m.worldType == type))
            {
                _currentMusic = m;
                return;
            }
            _currentMusic = null;
        }
    }
}