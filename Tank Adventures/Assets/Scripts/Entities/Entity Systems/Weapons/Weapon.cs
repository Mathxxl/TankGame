﻿using System.Collections;
using System.Collections.Generic;
using CustomEditor;
using UnityEngine;

namespace Entities.Entity_Systems.Weapons
{
    /// <summary>
    /// Encapsulate Weapons
    /// </summary>
    public abstract class Weapon : EntitySystem
    {
        #region Attributes

        [SerializeField] protected WeaponData data; //Base values as data
        [SerializeField][TagSelector] protected string[] attackableTags; //tags of objets that can be attacked by this weapon
        [SerializeField] protected float damages = 1.0f; //basic damages
        [SerializeField] protected float cooldown = 0.2f; //time to wait between each attack
        [SerializeField] protected float range = 1.0f; //range of the attack
        protected bool IsCoolingDown;
        protected string SelfTag; //tag of the entity that posses this weapon

        #endregion

        #region Properties

        public IEnumerable<string> AttackableTags => attackableTags;
        public string Tag => SelfTag;
        public Entity ThisEntity => entity;
        public float Range
        {
            get => range;
            set
            {
                range = value;
                entity.Events.OnRangeChange?.Invoke(value);
            }
        }

        public float Damages
        {
            get => damages;
            set
            {
                damages = value;
                entity.Events.OnDamagesChange?.Invoke(value);
            }
        }

        public float Cooldown
        {
            get => cooldown;
            set
            {
                cooldown = value;
                entity.Events.OnCooldownChange?.Invoke(value);
            }
        }

        #endregion


        #region Methods
        
        #region Mono Behaviours

        protected override void Awake()
        {
            base.Awake();
            SelfTag = entity.tag;

            if (data != null)
            {
                damages = data.damages;
                cooldown = data.cooldown;
                range = data.range;
            }
            
            if(attackableTags.Length == 0) Debug.LogWarning($"No tags setup for attack ({gameObject.name})");
        }

        #endregion

        //Called when an attack is to be performed, check if it can be done regarding weapon status
        public virtual void ToAttack()
        {
            if (!IsCoolingDown)
            {
                entity.Events.OnPerformingAttack?.Invoke();
                Attack();
                StartCoroutine(CoolingDown());
            }
        }
        
        //Perform the actual attack
        protected virtual void Attack(){}
        
        //Attack touch a target
        public bool TryToAttackTarget(Transform target)
        {
            //Cases not considered
            if (target.gameObject.CompareTag(SelfTag)) return false;  //hit on self

            AttackTarget(target);
            return true;
        }
        
        //Consequences of attacking a target
        protected virtual void AttackTarget(Transform target){}
        
        //Manages the attack cooldown
        private IEnumerator CoolingDown()
        {
            IsCoolingDown = true;
            yield return new WaitForSeconds(cooldown);
            IsCoolingDown = false;
        }
        
        #endregion

    }
}