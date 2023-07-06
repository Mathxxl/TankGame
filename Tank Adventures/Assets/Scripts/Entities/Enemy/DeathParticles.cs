using UnityEngine;

namespace Entities.Enemy
{
    public class DeathParticles : EntitySystem
    {
        [SerializeField] private ParticleSystem pSystem;
        
        private void OnEnable()
        {
            entity.Events.OnDying += ParticleOnDeath;
        }

        private void OnDisable()
        {
            entity.Events.OnDying -= ParticleOnDeath;
        }

        private void ParticleOnDeath()
        {
            if (pSystem == null) return;
            pSystem.Play();   
        }
    }
}