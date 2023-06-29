using System;
using Entities.Enemy.States;
using Entities.State_Machine;
using UnityEngine;
using Utilities;

namespace Entities.Enemy
{
    public class DefenseTowerController : StateController
    {
        [SerializeField] private EntityDetector detector;
        [SerializeField] private GameObject movingTower;

        public TowerIdleState towerIdleState = new TowerIdleState();
        public TowerAttackState attackState = new TowerAttackState();
        
        public Transform Target { get; private set; }
        public GameObject MovingTower => movingTower;

        private void OnEnable()
        {
            if (detector == null) return;
            detector.OnEntityDetected += PlayerFound;
            detector.OnEntityLeft += PlayerLeft;
        }

        private void OnDisable()
        {
            if (detector == null) return;
            detector.OnEntityDetected -= PlayerFound;
            detector.OnEntityLeft -= PlayerLeft;
        }

        private void Start()
        {
            ChangeState(towerIdleState);
        }   

        private void PlayerFound(Transform detected)
        {
            Target = detected;
            ChangeState(attackState);
        }

        private void PlayerLeft(Transform lost)
        {
            Target = null;
            ChangeState(towerIdleState);
        }
    }
}