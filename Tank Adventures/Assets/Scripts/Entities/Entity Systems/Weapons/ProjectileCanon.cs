using System.Linq;
using Interfaces;
using Projectile;
using UnityEngine;

//TODO : Augmenter les valeurs d'attaque/vie pour que le changement smooth d'UI ne soit pas bloqué par son threshold

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
        
        //To Improve Damages
        private Projectile.Projectile _lastProjectile;
        private Projectile.Projectile _improvedProjectile;
        private float _improvedTotal;
        
        #endregion

        #region Methods

        protected override void Awake()
        {
            base.Awake();
            _totalDamages = damages;
        }

        protected void OnEnable()
        {
            if (entity == null) return;
            
            entity.Events.OnImproveDamageForOneHitFixed += ImproveDamagesForOneHitFixed;
            entity.Events.OnImproveDamageForOneHit += ImproveDamagesForOneHit;

            entity.GameManagerForced.Events.OnLevelReached += OnLevelStart;
        }

        protected void OnDisable()
        {
            if (entity == null) return;
            
            entity.Events.OnImproveDamageForOneHitFixed -= ImproveDamagesForOneHitFixed;
            entity.Events.OnImproveDamageForOneHit -= ImproveDamagesForOneHit;

            if (entity.GameManager == null) return;
            
            entity.GameManager.Events.OnLevelReached -= OnLevelStart;

        }

        protected void OnLevelStart()
        {
            //Setup the object pool
            _pPool = new ProjectilePool(projectilePrefab, transform, 1000);
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
            
            _lastProjectile = temp;
        }

        protected override void AttackTarget(Transform target, Subweapon subweapon = null)
        {
            //Impact - check if damageable by projectile
            if (!target.gameObject.TryGetComponent(out IDamageable damageable) || !AttackableTags.Any(t => target.gameObject.CompareTag(t))) return;
            
            entity.Events.OnAttack?.Invoke(target);
            
            //If improved projectile improve damages
            if (subweapon != null && _improvedProjectile == subweapon)
            {
                Debug.Log($"subweapon found and improve damages; impact with {subweapon.name}");
                damageable.TakeDamages(_improvedTotal);
            }
            else
            {
                damageable.TakeDamages(_totalDamages);
            }

            //if linked to an entity, summon the OnAttacked event on self
            if (target.gameObject.TryGetComponent(out Entity targetEntity))
            {
                targetEntity.Events.OnAttacked?.Invoke(entity.transform);
            }
        }

        protected void ImproveDamagesForOneHit(float value)
        {
            _improvedProjectile = _lastProjectile;
            _improvedTotal = _totalDamages * (1.0f + value);
            Debug.Log($"improved projectile is {_improvedProjectile.name}");
        }

        protected void ImproveDamagesForOneHitFixed(float value)
        {
            _improvedProjectile = _lastProjectile;
            _improvedTotal = _totalDamages += value;
        }

        #endregion
        
    }
}
