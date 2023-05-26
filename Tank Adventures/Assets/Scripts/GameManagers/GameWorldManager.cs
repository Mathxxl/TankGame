using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;

namespace GameManagers
{
    //Manages worlds
    public class GameWorldManager : Manager
    {
        [SerializeField] private List<World.World> gameWorlds;
        [SerializeField] private List<WorldData> worldsData;
        [Tooltip("Indicates after how many worlds the level goes up")][SerializeField] private int floor = 3;
        [Tooltip("Indicates the maximum number of levels in a game")] [SerializeField] private int maxLevels = 16;
        
        [Header("Debug")]
        [SerializeField] private World.World currentWorld;
        
        private int _sceneIdx;

        public World.World CurrentWorld => currentWorld;
        public int Idx => _sceneIdx;
        public IEnumerable<World.World> Worlds => gameWorlds;

        //TODO : génération pondérée de mondes possibles dans les portails, pondération à 0 si plus d'updates disponibles

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

        public void ChangeScene(WorldType type)
        {
            if (type != currentWorld.Type)
            {
                ChangeWorld(type);
            }
            
            //TODO : SceneManager for asynchronous loading and loading screen
            var nextLevel = currentWorld.GetSceneOfLevel(_sceneIdx / floor);
            //SceneManager.LoadScene(nextLevel);
            Debug.Log($"Next Level would be {nextLevel ?? ">null<"}");
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

        public WorldData GetDataFromWorld(WorldType type)
        {
            return worldsData.FirstOrDefault(data => data.type == type);
        }

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
            gameManager.Events.OnWorldChanged?.Invoke(type);
            currentWorld = w;

            //ENTER
            gameManager.Events.OnWorldJoin?.Invoke(type);
            currentWorld.OnWorldEnter();
        }

    }
}