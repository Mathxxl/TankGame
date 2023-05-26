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
            
        }

        private void OnEnable()
        {
            gameManager.Events.OnUpgradeChosen += _ => OnCallStart();
        }

        private void OnDisable()
        {
            gameManager.Events.OnUpgradeChosen -= _ => OnCallStart();
        }

        private void SetTypeList()
        {
            _types = new List<WorldType>();
            foreach (var world in worldManager.Worlds.Where(world => !_types.Contains(world.Type)))
            {
                _types.Add(world.Type);
            }
        }

        private void OnCallStart()
        {
            GeneratePortals();
        }
        
        public void GeneratePortals()
        {
            for (var i = 0; i < numberOfChoices; i++)
            {
                var portalObject = Instantiate(portalPrefab, transform);
                
                //Add to go world to the portal

                var randomValue = typesPonderation.GetRandom(); //TODO : Have only one choice per world
                var portal = portalObject.GetComponent<Portal>();
                var type = _types[randomValue];
                Debug.Log($"Portal type = {type}");
                
                portal.manager = this;
                portal.LinkTo(type, worldManager.GetDataFromWorld(type));
                
                //TODO : Increase ponderation for chosen world
            }
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

        #endregion
    }
}