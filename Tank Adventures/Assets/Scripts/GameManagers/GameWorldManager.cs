using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;

namespace GameManagers
{
    /// <summary>
    /// Manager for the Worlds
    /// </summary>
    public class GameWorldManager : Manager
    {
        [SerializeField] private List<World.World> gameWorlds;
        [SerializeField] private List<WorldData> worldsData;
        [Tooltip("Indicates after how many worlds the level goes up")][SerializeField] private int floor = 3;
        [Tooltip("Indicates the maximum number of levels in a game")] [SerializeField] private int maxLevels = 16; //TODO : Call end game when reaching max level
        
        [Header("Debug")]
        [SerializeField] private World.World currentWorld;
        
        private int _sceneIdx;

        public World.World CurrentWorld => currentWorld;
        public int Idx => _sceneIdx;
        public IEnumerable<World.World> Worlds => gameWorlds;
        public GameManager GManager => gameManager;

        private void Awake()
        {
            _sceneIdx = 0;
        }

        private void OnEnable()
        {
            gameManager.Events.OnWorldChosen += ChangeScene;
        }

        private void OnDisable()
        {
            gameManager.Events.OnWorldChosen -= ChangeScene;
        }

        private void Update()
        {
            if (currentWorld == null) return;
            
            currentWorld.OnWorldUpdate();
        }

        /// <summary>
        /// Go to a random scene of given WorldType
        /// </summary>
        /// <param name="type"></param>
        public void ChangeScene(WorldType type)
        {
            //Change world if needed
            if (type != currentWorld.Type)
            {
                ChangeWorld(type);
            }

            //Check index to launch final level if needed
            IdxProgress();
            
            //Load next scene based on level
            //TODO : SceneManager for asynchronous loading and loading screen
            var nextLevel = currentWorld.GetSceneOfLevel(_sceneIdx / floor);
            var buildIndex = SceneUtility.GetBuildIndexByScenePath(nextLevel);
            
            if (buildIndex < 0) //Scene not found : free the player as in a free zone
            {
                Debug.LogWarning($"Scene {nextLevel} doesn't exists or has not been added to the build");
                gameManager.Events.OnFreeZoneReached?.Invoke();
            }
            else
            {
                SceneManager.LoadScene(nextLevel);
            }
            Debug.Log($"Next Level would be {nextLevel ?? ">null<"}");
        }

        /// <summary>
        /// Increase the idx that keep track of how many levels were visited and call events related when needed
        /// </summary>
        private void IdxProgress()
        {
            _sceneIdx++;
            Debug.Log($"Reaching level {_sceneIdx}");
            
            var idDiff = maxLevels - _sceneIdx;
            if (idDiff < 3)
            {
                switch (idDiff)
                {
                    case 2:
                        gameManager.Events.OnBeforeFinalWorld?.Invoke();
                        break;
                    case 1:
                        gameManager.Events.OnFinalWorldReached?.Invoke();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (_sceneIdx % floor == floor - 1)
                {
                    gameManager.Events.OnLevelBeforeHome?.Invoke();
                }
            }

        }
        
        private void ChangeWorld(WorldType type)
        {
            var world = GetWorldByType(type);
            
            if (world != null)
            {
                ChangingWorld(world);
            }
            else
            {
                Debug.LogWarning($"Try to change to world not found {world} at level {_sceneIdx/floor}");
            }
        }

        /// <summary>
        /// Returns WorldData associated with given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public WorldData GetDataFromWorld(WorldType type)
        {
            return worldsData.FirstOrDefault(data => data.type == type);
        }

        /// <summary>
        /// Returns the World associated with given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private World.World GetWorldByType(WorldType type)
        {
            return gameWorlds.FirstOrDefault(world => world.Type == type);
        }

        private void ChangingWorld(World.World w)
        {
            var type = currentWorld.Type;
            
            //EXIT
            gameManager.Events.OnWorldLeft?.Invoke(type);
            currentWorld.OnWorldExit();

            //CHANGE
            gameManager.Events.OnWorldChanged?.Invoke(w.Type);
            currentWorld = w;

            //ENTER
            gameManager.Events.OnWorldJoin?.Invoke(w.Type);
            currentWorld.OnWorldEnter();
        }

    }
}