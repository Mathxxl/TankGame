using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using World;

namespace GameManagers
{
    /// <summary>
    /// Manages the generations of portal in the game, to go to another world
    /// </summary>
    public class GamePortalManager : Manager
    {
        #region Attributes

        [Header("Components")]
        
        [SerializeField] private GameWorldManager worldManager;
        [SerializeField] private GameObject portalPrefab;
        [SerializeField][Range(0,10)] private int numberOfChoices = 3;
        
        [Header("Will be created if null")]
        [SerializeField] private RandomPondered typesPonderation;
        
        private List<WorldType> _types;
        private int _availableTypesCount;
        private bool _goHome;
        private bool _goFinal;
        
        public ManagerEvents Events => gameManager.Events;

        #endregion

        #region Methods

        private void Awake()
        {
            SetTypeList();
            if (typesPonderation == null)
            {
                typesPonderation = RandomPondered.CreateComponent(gameObject, 10, _types.Count);
            }
            //Remove Special Types
            RemoveTypeFromSelection(WorldType.Home);
            RemoveTypeFromSelection(WorldType.Final);
        }

        private void OnEnable()
        {
            if (gameManager == null) return;
            
            gameManager.Events.OnUpgradeChosen += _ => OnCallStart();
            gameManager.Events.OnAllUpgradesGottenForWorld += RemoveTypeFromSelection;
            gameManager.Events.OnLevelBeforeHome += () => { _goHome = true; };
            gameManager.Events.OnBeforeFinalWorld += () => { _goFinal = true; };
        }

        private void OnDisable()
        {
            if (gameManager == null) return;
            
            gameManager.Events.OnUpgradeChosen -= _ => OnCallStart();
            gameManager.Events.OnAllUpgradesGottenForWorld -= RemoveTypeFromSelection;
            gameManager.Events.OnLevelBeforeHome -= () => { _goHome = true; };
            gameManager.Events.OnBeforeFinalWorld -= () => { _goFinal = true; };
        }

        private void SetTypeList()
        {
            _types = new List<WorldType>();
            foreach (var world in worldManager.Worlds.Where(world => !_types.Contains(world.Type)))
            {
                _types.Add(world.Type);
            }

            _availableTypesCount = _types.Count;
        }

        private void OnCallStart()
        {
            if (_goFinal)
            {
                GeneratePortal(WorldType.Final);
                _goFinal = false;
            }
            else if (_goHome)
            {
                GeneratePortal(WorldType.Home);
                _goHome = false;
            }
            else
            {
                GeneratePortals();
            }
        }
        
        /// <summary>
        /// Instantiates the portals and link them to a random available world
        /// </summary>
        private void GeneratePortals()
        {
            var memList = new List<int>();
            
            for (var i = 0; i < numberOfChoices; i++)
            {
                //Get random type

                var randomValue = typesPonderation.GetRandomWithExclusion(memList);
                typesPonderation.AddWeight(randomValue);
                var type = _types[randomValue];
                Debug.Log($"Portal type = {type}");

                //Generate portal
                
                GeneratePortal(type);
            }
        }

        /// <summary>
        /// Generates a portal of given type
        /// </summary>
        /// <param name="type"></param>
        private void GeneratePortal(WorldType type)
        {
            var portalObject = Instantiate(portalPrefab, transform);
                
            //Add to go world to the portal
            var portal = portalObject.GetComponent<Portal>();
            portal.manager = this;
            portal.LinkTo(type, worldManager.GetDataFromWorld(type));
        }

        public void OnCallEnd()
        {
            DestroyPortals();
        }
        
        private void DestroyPortals()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        private void RemoveTypeFromSelection(WorldType type)
        {
            var idx = _types.IndexOf(type);
            if (idx < 0) return;
            
            typesPonderation.AddAt(idx, 0);
            _availableTypesCount--;
            if (numberOfChoices > _availableTypesCount) numberOfChoices = _availableTypesCount;
        }

        #endregion
    }
}