using System;
using Entities.Entity_Systems.Weapons;
using Entities.State_Machine;
using UnityEngine;

namespace Entities.Enemy.States
{
    [Serializable]
    public class TowerAttackState : State
    {
        private Transform _target;
        private GameObject _tower;

        [SerializeField] private Weapon weapon;
        [SerializeField] private GameObject realCanon;

        protected override void OnEnter()
        {
            if (Controller is DefenseTowerController dController)
            {
                _target = dController.Target;
                _tower = dController.MovingTower;
            }
            else
            {
                Debug.LogWarning("Tower Controller does not have the correct inherited class");
            }
        }

        protected override void OnExit()
        {
            
        }

        protected override void UpdateState()
        {
            if (_target == null) return;
            RotateTower();
            if (weapon == null) return;
            weapon.ToAttack();
        }

        private void RotateTower()
        {
            //Visuals
            var pos = _target.position;
            var subPosition = new Vector3(pos.x, _tower.transform.position.y, pos.z);
            _tower.transform.LookAt(subPosition);
            
            //Real canon
            realCanon.transform.LookAt(_target);
        }
    }
}