using System;
using Entities.Player.Ultimate;
using Entities.Player.Upgrades;
using UnityEngine;
using Utilities;

namespace Entities
{
    /// <summary>
    /// Struct for all events related to entities
    /// </summary>
    public struct EntityEvents
    {
        //Movement Events
        public Action OnStartMoving; //The entity start moving
        public Action OnStopMoving; //The entity stop moving

        //Health Events
        public Action<float> OnHealthChanged; //The health has changed
        public Action<float> OnTakeDamage; //Damages have been taken
        public Action OnDeath; //The entity is dead
        public Action<float> OnMaxHealthChanged; //Max health has changed
        public Action<SModif> OnHeal; //TODO : Ajouter OnHealed pour la valeur fixe ajoutée et OnHeal pour dire qu'il faut soigner cette valeur
        public Action<float> OnHealed;
    
        //Collision Events
        public Action<Transform> OnCollision; //Collision with something
    
        //UI Events
    
        //Defense Events
        public Action<float> OnDefenseChanged; //Defense value has changed
        
        //Invincible Events
        public Action<bool> OnInvincible; //The entity in invincible (true/false)
    
        //Attack Events
        public Action OnPerformingAttack; //The attack is made (for animations etc.)
        public Action<Transform> OnAttack; //A valid attack is performed on a target
        public Action<Transform> OnAttacked; //A target attack us
        public Action OnAttackHit; //Our attack hit something
        public Action<float> OnAttackChanged;
        public Action<float> OnAttackSpeedChanged;
        
        //Weapon Events
        public Action<float> OnDamagesChange;
        public Action<float> OnRangeChange;
        public Action<float> OnCooldownChange;
        
        //Position Events
        public Action OnFall;
        
        //Ultimate Events
        public Action<Ultimate> OnUltimateUsed;
        public Action<Ultimate> OnUltimateEnd;
        public Action<float> OnUltimateCoolingDown;
        
        //Upgrades
        public Action<Upgrade> OnUpgradeChosen;
        public Action<Upgrade> OnUpgradeObtained;
        public Action<Upgrade> OnUpgradeRemoved;
        public Action<Upgrade> OnUpgradeSelected;
        public Action<Upgrade> OnUpgradeLeveledUp;
        public Action<World.WorldType> OnFullUpgradeOnRoad;
        
        //Upgrades Specifics
        public Action<float> OnImproveDamages;
        public Action<float> OnReduceDamages;
        public Action<float> OnImproveHeal;
        public Action<float> OnReduceHeal;
        public Action<float> OnImproveDamageForOneHit;
        public Action<float> OnImproveDamagesFixed;
        public Action<float> OnReduceDamagesFixed;
        public Action<float> OnImproveHealFixed;
        public Action<float> OnReduceHealFixed;
        public Action<float> OnImproveDamageForOneHitFixed;
        public Action<float> OnImproveDefense;
        public Action<float> OnImproveDefenseFixed;
        public Action<float> OnImproveSpeed;
        public Action<float> OnImproveSpeedFixed;
        public Action<float> OnChangeSpeed;
        
        //Ground Events
        public Action OnGroundTouched;
        public Action OnGroundLeft;
        
        //Spawn
        public Action OnPlayerSpawned;
        
        //Targeting
        public Action<Transform> OnTargetAcquired;
        public Action OnTargetLost;
    }
}
