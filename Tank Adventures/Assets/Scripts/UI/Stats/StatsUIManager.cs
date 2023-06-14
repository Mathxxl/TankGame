using System;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Stats
{
    public class StatsUIManager : EntitySystem
    {
        [SerializeField] private GameObject statsHolder;
        private bool _isActive;
        
        [SerializeField] private StatHolder healthHolder;
        [SerializeField] private StatHolder attackHolder;
        [SerializeField] private StatHolder defenseHolder;
        [SerializeField] private StatHolder speedHolder;
        [SerializeField] private StatHolder attackSpeedHolder;

        protected override void Awake()
        {
            base.Awake();
            
            SetTextValues();
            SetInitialValues();
            statsHolder.SetActive(_isActive);
        }

        private void OnEnable()
        {
            entity.Events.OnHealthChanged += healthHolder.SetValue;
            entity.Events.OnAttackChanged += attackHolder.SetValue;
            entity.Events.OnDefenseChanged += defenseHolder.SetValue;
            entity.Events.OnChangeSpeed += speedHolder.SetValue;
            entity.Events.OnAttackSpeedChanged += attackSpeedHolder.SetValueInv;
        }

        private void OnDisable()
        {
            entity.Events.OnHealthChanged -= healthHolder.SetValue;
            entity.Events.OnAttackChanged -= attackHolder.SetValue;
            entity.Events.OnDefenseChanged -= defenseHolder.SetValue;
            entity.Events.OnChangeSpeed -= speedHolder.SetValue;
            entity.Events.OnAttackSpeedChanged -= attackSpeedHolder.SetValueInv;
        }

        private void OnStatsMenu()
        {
            _isActive = !_isActive;
            statsHolder.SetActive(_isActive);
        }

        private void SetTextValues()
        {
            if(healthHolder != null) healthHolder.SetTextName("Health");
            if(attackHolder != null) attackHolder.SetTextName("Attack");
            if(defenseHolder != null) defenseHolder.SetTextName("Defense");
            if(speedHolder != null) speedHolder.SetTextName("Speed");
            if(attackSpeedHolder != null) attackSpeedHolder.SetTextName("Attack Speed");
        }

        private void SetInitialValues()
        {
            if(healthHolder != null) healthHolder.SetValue(entity.id.lifePoint);
            if(attackHolder != null) attackHolder.SetValue(entity.id.attack);
            if(defenseHolder != null) defenseHolder.SetValue(entity.id.defense);
            if(speedHolder != null) speedHolder.SetValue(entity.id.speed);
            if(attackSpeedHolder != null) attackSpeedHolder.SetValueInv(entity.id.attackSpeed);
        }
    }
}
