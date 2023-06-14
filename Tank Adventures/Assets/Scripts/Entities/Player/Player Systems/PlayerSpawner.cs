using System;
using UnityEngine;

namespace Entities.Player.Player_Systems
{
    public class PlayerSpawner : EntitySystem
    {
        #region Attributes

        private Transform _spawnPoint;

        #endregion

        #region Methods

        private void OnEnable()
        {
            entity.GameManagerForced.Events.OnLevelStart += OnLevelStart;
        }

        private void OnDisable()
        {
            if (entity.GameManager == null) return;
            
            entity.GameManagerForced.Events.OnLevelStart -= OnLevelStart;
        }

        private void OnLevelStart()
        {
            SetSpawnPoint();
            SpawnPlayer();
        }

        private void SetSpawnPoint()
        {
            var obj = GameObject.FindGameObjectWithTag("Spawnpoint");
            if (obj != null)
            {
                _spawnPoint = obj.transform;
            }
            else
            {
                _spawnPoint = null;
                Debug.LogWarning("No spawn point found");
            }
        }

        private void SpawnPlayer()
        {
            var entityTransform = entity.transform;
            if (_spawnPoint != null)
            {
                entityTransform.position = _spawnPoint.position;
                entityTransform.rotation = _spawnPoint.rotation;
            }
            else
            {
                entityTransform.position = Vector3.zero;
                entityTransform.rotation = Quaternion.identity;
            }

            entity.Events.OnPlayerSpawned?.Invoke();
        }

        #endregion
    }
}