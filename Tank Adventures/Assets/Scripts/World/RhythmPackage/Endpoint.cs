using GameManagers;
using UnityEngine;
using Utilities;

namespace World.RhythmPackage
{
    public class Endpoint : MonoBehaviour
    {
        [SerializeField] private PlayerDetector detector;
        private GameManager _gameManager;
        
        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();

            if (_gameManager == null)
            {
                Debug.LogWarning("No game manager found");
                return;
            }

            detector.OnPlayerDetected += PlayerDetected;
        }

        private void OnDisable()
        {
            if (_gameManager == null || detector == null) return;
            detector.OnPlayerDetected -= PlayerDetected;
        }

        private void PlayerDetected(Transform player)
        {
            _gameManager.Events.OnGoalAchieved?.Invoke();
        }
    }
}