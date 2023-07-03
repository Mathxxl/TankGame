using System;
using System.Collections;
using System.Collections.Generic;
using Camera;
using GameManagers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace World
{
    /// <summary>
    /// Manages specific behaviours of worlds
    /// </summary>
    public abstract class World : MonoBehaviour
    {
        [Serializable]
        public struct WorldLevel
        {
            public string sceneName;
            public int level;
        }
        
        [SerializeField] protected WorldType worldType;
        [SerializeField] protected GameWorldManager manager;
        [SerializeField] protected List<WorldLevel> scenes;
        private List<List<WorldLevel>> _scenesByLevel;
        public WorldType Type => worldType;

        private void Awake()
        {
            SetLevels();
            Debug.Log($"{_scenesByLevel.Count} levels found for world {name}");
            OnAwake();
        }

        /// <summary>
        /// Called when entering any world
        /// </summary>
        public void OnWorldEnter()
        {
            manager.GManager.Events.OnLevelReached += LevelReached;
            OnEnter();
        }

        /// <summary>
        /// Called each frame for any world
        /// </summary>
        public void OnWorldUpdate()
        {
            OnUpdate();
        }

        /// <summary>
        /// Called on leaving any world
        /// </summary>
        public void OnWorldExit()
        {
            OnExit();
            manager.GManager.Events.OnLevelReached -= LevelReached;
        }

        /// <summary>
        /// Returns the name of a scene associated to this world with given level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public string GetSceneOfLevel(int level)
        {
            if (level >= _scenesByLevel.Count) return null;
            
            var availableLevels = _scenesByLevel[level];
            var rand = Random.Range(0, availableLevels.Count);
            return availableLevels[rand].sceneName;
        }

        protected abstract void OnEnter();
        protected abstract void OnUpdate();
        protected abstract void OnExit();
        
        protected virtual void OnAwake(){}

        protected virtual void LevelReached()
        {
            StartCoroutine(EnsureStart());
        }

        private IEnumerator EnsureStart()
        {
            yield return null;
            manager.GManager.Events.OnZoneStart?.Invoke();
            manager.GManager.Events.OnLevelStart?.Invoke();
            Debug.Log("World Start Events");
        }

        /// <summary>
        /// Set the list of list of levels of this world ordered by level
        /// </summary>
        private void SetLevels()
        {
            _scenesByLevel = new List<List<WorldLevel>>();
            foreach (var scene in scenes)
            {
                var level = scene.level;
                if (level >= _scenesByLevel.Count)
                {
                    while (level >= _scenesByLevel.Count)
                    {
                        _scenesByLevel.Add(new List<WorldLevel>());
                    }
                }
                _scenesByLevel[level].Add(scene);
            }
        }

        protected void ChangeCameraMode(CameraMode newMode)
        {
            Debug.Log("ChangeCameraMode");
            
            if (manager.GManager.playerCamera != null &&
                manager.GManager.playerCamera.TryGetComponent(out CameraBehaviour cameraBehaviour))
            {
                cameraBehaviour.Mode = newMode;
                Debug.Log($"New mode = {newMode}");
            }
        }
    }
}