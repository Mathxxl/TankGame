using Entities;
using UnityEngine;

namespace Physics.Explosion
{
    /// <summary>
    /// Represent an explosion
    /// </summary>
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private float explosionForce = 10f;
        [SerializeField] private float explosionRadius = 10f;
        [SerializeField] private ParticleSystem explosionEffect;

        private Collider[] _colliders = new Collider[20];

        private void Awake()
        {
            explosionEffect ??= GetComponent<ParticleSystem>();
            //ExplodeNonAlloc();
        }

        //Add force to colliders within range and play particle effect
        public void ExplodeNonAlloc()
        {
            if (explosionEffect != null)
            {
                explosionEffect.Play();
            }

            var nColliders = UnityEngine.Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, _colliders);
            if (nColliders <= 0) return;
            
            foreach (var col in _colliders)
            {
                if (col == null || !col.TryGetComponent(out Rigidbody rb)) continue;
                if (col.gameObject.TryGetComponent(out MortalEntity mortalEntity) && mortalEntity.Invincible) continue;
                
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                //Debug.Log($"Add force on {col.gameObject.name} of value {explosionForce}");
            }
        }
    }
}