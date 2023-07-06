using System;
using Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;

namespace GameManagers
{
    /// <summary>
    /// Main manager for the game, allows managers to communicate between them and with the player
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Entity player;
        public Entity Player => player;
        public UnityEngine.Camera playerCamera;
        public ManagerEvents Events = new();

        private void Awake()
        {
            if (playerCamera == null) playerCamera = UnityEngine.Camera.main;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
            player.Events.OnDeath += PlayerKilled;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
            player.Events.OnDeath -= PlayerKilled;
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

        private void PlayerKilled()
        {
            Events.OnPlayerKilled?.Invoke();
        }

        //TESTING
        public void EndLevel()
        {
            Events.OnGoalAchieved?.Invoke();
        }

        public void FinalBoss()
        {
            Events.OnWorldChosen?.Invoke(WorldType.Final);
            Events.OnFinalWorldReached?.Invoke();
        }
    }
}