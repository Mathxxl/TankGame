using System;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using UnityEngine.Events;

namespace GameManagers
{
    /// <summary>
    /// Manages the enemies
    /// </summary>
    public class EnemyManager : Manager
    {
        [SerializeField] private GameObject enemiesHolder;
        private List<MortalEntity> _enemies;
        private int _enemiesCount;
        private bool _isBoss;

        private void OnEnable()
        {
            gameManager.Events.OnFinalWorldReached += () => { _isBoss = true; };
            gameManager.Events.OnLevelReached += FindEnemies;
            
        }

        private void OnDisable()
        {
            gameManager.Events.OnFinalWorldReached -= () => { _isBoss = true; };
            gameManager.Events.OnLevelReached -= FindEnemies;
        }

        /// <summary>
        /// Look for any object tagged with "Enemy" in the scene and count them
        /// </summary>
        private void FindEnemies()
        {
            _enemies = new List<MortalEntity>();
            _enemiesCount = 0;
            
            if (enemiesHolder != null)
            {
                var bis = GetComponentsInChildren<MortalEntity>();
                foreach (var me in bis)
                {
                    AddEnemyChecks(me);
                }
            }
            else
            {
                var found = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (var obj in found)
                {
                    if (!obj.TryGetComponent(out MortalEntity me)) continue;
                    AddEnemyChecks(me);
                }
            }

            _enemiesCount = _enemies.Count;
            Debug.Log($"{_enemiesCount} enemies found");
        }

        private void AddEnemyChecks(MortalEntity me)
        {
            _enemies.Add(me);
            if (me.Events.OnDeath != null) me.Events.OnDeath -= CheckOnDeath;
            me.Events.OnDeath += CheckOnDeath;
        }

        /// <summary>
        /// Called whenever an enemy is destroyed to check how many are remaining 
        /// </summary>
        private void CheckOnDeath()
        {
            _enemiesCount--;
            Debug.Log($"Enemy has died : {_enemiesCount} enemies remaining");
            if (_enemiesCount <= 0)
            {
                if (_isBoss)
                {
                    gameManager.Events.OnFinalLevelAchieved?.Invoke();
                    return;
                }
                gameManager.Events.OnGoalAchieved?.Invoke();
                Debug.Log("Everyone is DEAD");
            }
        }
    }
}