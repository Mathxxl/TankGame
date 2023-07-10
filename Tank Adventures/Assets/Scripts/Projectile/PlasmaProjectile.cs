using System.Collections;
using UnityEngine;

namespace Projectile
{
    public class PlasmaProjectile : Projectile
    {
        [SerializeField] private GameObject plasmaLaser;
        private GameObject _plasma;
        public GameObject Plasma => _plasma;
        
        
        protected override void OnEnable()
        {
            base.OnEnable();
            StartCoroutine(GeneratePlasma());
        }

        public override void DestroyObject()
        {
            Destroy(_plasma);
            base.DestroyObject();
        }

        private void SetParticleSpeed()
        {
            if (_plasma == null || !_plasma.TryGetComponent(out ParticleSystem pSystem)) return;

            var pSystemMain = pSystem.main;
            pSystemMain.startSpeed = speed;
        }

        private IEnumerator GeneratePlasma()
        {
            yield return null;
            var wTransform = weapon.transform;
            _plasma = Instantiate(plasmaLaser, wTransform.position, wTransform.rotation);
            SetParticleSpeed();
        }
        
    }
}