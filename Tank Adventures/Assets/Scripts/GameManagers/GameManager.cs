using System;
using Entities;
using UnityEngine;

namespace GameManagers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Entity player;
        public Entity Player => player;
        public ManagerEvents Events = new();

        private void Start()
        {
            Events.OnLevelStart?.Invoke();
        }
        
        //TESTING
        public void EndLevel()
        {
            Events.OnGoalAchieved?.Invoke();
        }
    }
}