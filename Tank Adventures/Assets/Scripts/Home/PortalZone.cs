using System;
using GameManagers;
using UnityEngine;
using Utilities;

namespace Home
{
    public class PortalZone : MonoBehaviour
    {
        [SerializeField] private PlayerDetector detector;
        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            
            if(_gameManager == null) Debug.LogWarning("No Game Manager found for PortalZone");
        }

        private void OnEnable()
        {
            detector.OnPlayerDetected += PlayerIntoPortal;
        }

        private void OnDisable()
        {
            detector.OnPlayerDetected -= PlayerIntoPortal;
        }

        private void PlayerIntoPortal(Transform t)
        {
            if (_gameManager == null) return;
            
            _gameManager.Events.OnGoalAchieved?.Invoke();
        }
    }
}
