using System.Linq;
using Interfaces;
using Projectile;
using UnityEngine;

namespace Entities.Entity_Systems.Weapons
{
    /// <summary>
    /// Weapon that fires projectiles
    /// </summary>
    public class ProjectileCanon : Weapon
    {
        #region Attributes

        [SerializeField] private GameObject projectilePrefab;
        private ProjectilePool _pPool;
        private float _totalDamages;

        #endregion

        #region Methods

        protected override void Awake()
        {
            base.Awake();
            _pPool = new ProjectilePool(projectilePrefab, transform, 1000); //Setup the object pool
            _totalDamages = damages;
            if (projectilePrefab.TryGetComponent(out Projectile.Projectile p))
            {
                _totalDamages *= p.Multiplier;
            }
        }

        public void ChangeProjectile(GameObject newPrefab)
        {
            projectilePrefab = newPrefab;
            Awake();
        }

        //NOTE : OnShoot is sent by a PlayerInput component
        protected void OnShoot()
        {
            ToAttack();
        }

        protected override void Attack()
        {
            var temp = _pPool.Pool.Get();
            temp.weapon = this;
        }

        protected override void AttackTarget(Transform target)
        {
            //Impact - check if damageable by projectile
            if (!target.gameObject.TryGetComponent(out IDamageable damageable) || !AttackableTags.Any(t => target.gameObject.CompareTag(t))) return;
            
            entity.Events.OnAttack?.Invoke(target);
            damageable.TakeDamages(_totalDamages);

            //if linked to an entity, summon the OnAttacked event on self
            if (target.gameObject.TryGetComponent(out Entity targetEntity))
            {
                targetEntity.Events.OnAttacked?.Invoke(entity.transform);
            }
        }

        #endregion
        
    }
}
