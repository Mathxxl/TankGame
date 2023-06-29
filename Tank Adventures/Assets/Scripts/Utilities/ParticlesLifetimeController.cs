using UnityEngine;

namespace Utilities
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlesLifetimeController : MonoBehaviour
    {
        [SerializeField] private GameObject rootObject;
        private void OnParticleSystemStopped()
        {
            Destroy(rootObject);
        }
    }
}