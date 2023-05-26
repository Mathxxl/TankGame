using System;
using System.Collections.Generic;
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
        }

        public void OnWorldEnter()
        {
            OnEnter();
        }

        public void OnWorldUpdate()
        {
            OnUpdate();
        }

        public void OnWorldExit()
        {
            OnExit();
        }

        public string GetSceneOfLevel(int level)
        {
            if (level >= _scenesByLevel.Count) return null;
            
            var availableLevels = _scenesByLevel[level];
            var rand = Random.Range(0, availableLevels.Count);
            return availableLevels[rand].sceneName;
        }
        
        protected virtual void OnEnter(){}
        protected virtual void OnUpdate(){}
        protected virtual void OnExit(){}

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
    }
}