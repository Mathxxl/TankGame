using System;
using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace GameManagers
{
    //Manages the enemy of worlds
    public class EnemyManager : Manager
    {
        [SerializeField] private GameObject enemiesHolder;
        private List<MortalEntity> _enemies;
        private int _enemiesCount;

        private void Awake()
        {
            _enemies = new List<MortalEntity>();
            _enemiesCount = 0;
        }

        private void Start()
        {
            FindEnemies();
        }

        private void FindEnemies()
        {
            if (enemiesHolder != null)
            {
                var bis = GetComponentsInChildren<MortalEntity>();
                foreach (var me in bis)
                {
                    _enemies.Add(me);
                    me.Events.OnDeath += CheckOnDeath;
                }
            }
            else
            {
                var found = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (var obj in found)
                {
                    if (!obj.TryGetComponent(out MortalEntity me)) continue;
                    _enemies.Add(me);
                    me.Events.OnDeath += CheckOnDeath;
                }
            }

            _enemiesCount = _enemies.Count;
            Debug.Log($"{_enemiesCount} enemies found");
        }

        private void CheckOnDeath()
        {
            _enemiesCount--;
            Debug.Log($"Enemy has died : {_enemiesCount} enemies remaining");
            if (_enemiesCount <= 0)
            {
                gameManager.Events.OnGoalAchieved?.Invoke();
                Debug.Log("Everyone is DEADDDD");
            }
        }
    }
}