using System;
using Entities.Entity_Systems;
using Entities.Entity_Systems.Weapons;
using UnityEngine;

namespace Entities.Enemy.EnemySystems
{
    /// <summary>
    /// UNUSED - DEPRECATED
    /// </summary>
    public class EnemyProjectileCanon : ProjectileCanon
    {
        [SerializeField] private EnemyStateController controller;

        protected override void Awake()
        {
            base.Awake();
            controller ??= GetComponent<EnemyStateController>();
        }
    }
}