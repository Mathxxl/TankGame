using System;
using Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagers
{
    /// <summary>
    /// Main manager for the game, allows managers to communicate between them and with the player
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Entity player;
        public Entity Player => player;
        public ManagerEvents Events = new();

        private void OnEnable()
        {
            SceneManager.sceneLoaded += (_,_) => { Debug.Log("SceneManager.sceneLoaded event callback"); };
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        }

        private void Start()
        {
            //Events.OnFirstGameStart?.Invoke();
            Events.OnLevelReached?.Invoke();
            Events.OnZoneStart?.Invoke();
            Events.OnLevelStart?.Invoke();
        }

        private void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            Debug.Log($"Scene loaded : {scene.name} in mode {loadSceneMode}");
            Events.OnLevelReached?.Invoke();
        }

        //TESTING
        public void EndLevel()
        {
            Events.OnGoalAchieved?.Invoke();
        }
    }
}