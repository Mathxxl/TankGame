using System;
using GameManagers;
using Unity.VisualScripting;
using UnityEngine;

namespace Entities
{
    /// <summary>
    /// Describe in-game entities. An entity is an element that can receive multiple complex systems.
    /// </summary>
    public class Entity : MonoBehaviour
    {
        public EntityID id;
        public EntityEvents Events;

        [SerializeField] private GameManager gameManager;
        
        //Return the GameManager and look for it if null | Should be called when the manager is needed (OnEnable...)
        public GameManager GameManagerForced
        {
            get
            {
                if(gameManager == null) GetGameManager();
                return gameManager;
            }
        }

        //Return the GameManager without null check | Can be called alongside a null check (OnDisable...)
        public GameManager GameManager => gameManager;

        protected virtual void Awake()
        {
            if (gameManager != null) return;
            GetGameManager();
        }

        private void GetGameManager()
        {
            var obj = FindObjectOfType(typeof(GameManager));
            if (obj == null)
            {
                Debug.LogWarning("No GameManager found on scene");
                return;
            }
            gameManager = obj.GetComponent<GameManager>(); 
        }
    }
}
