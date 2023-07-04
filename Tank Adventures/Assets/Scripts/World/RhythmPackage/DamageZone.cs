using Interfaces;
using UnityEngine;

namespace World.RhythmPackage
{
    [RequireComponent(typeof(Collider))]
    public class DamageZone : MonoBehaviour
    {
        [SerializeField] private float damages;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamages(damages);
            }
        }
    }
}