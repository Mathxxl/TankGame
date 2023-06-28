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
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        }

        private void Start()
        {
            //Events.OnFirstGameStart?.Invoke();
            //Events.OnLevelReached?.Invoke(); //should be done by th sceneManagerOnSceneLoaded
            Events.OnZoneStart?.Invoke();
            Events.OnLevelStart?.Invoke();
        }

        private void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            Debug.Log($"Scene loaded : {scene.name}");
            Events.OnLevelReached?.Invoke();
        }

        //TESTING
        public void EndLevel()
        {
            Events.OnGoalAchieved?.Invoke();
        }
    }
}