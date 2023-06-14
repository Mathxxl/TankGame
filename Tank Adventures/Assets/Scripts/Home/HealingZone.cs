using System;
using Entities;
using Entities.Entity_Systems;
using UnityEngine;
using Utilities;

namespace Home
{
    public class HealingZone : MonoBehaviour
    {
        #region Attributes

        [SerializeField][Range(0f,1f)] private float healValue;
        [SerializeField][Range(0f,100f)] private float healValueFixed;
        [SerializeField] private PlayerDetector detector;
        [SerializeField] private ParticleSystem pSystem;
        private bool _used;
        
        #endregion
        
        #region Methods
        
        private void OnEnable()
        {
            detector.OnPlayerDetected += ToHeal;
        }

        private void OnDisable()
        {
            detector.OnPlayerDetected -= ToHeal;
        }
        
        private void Start()
        {
            if(pSystem != null) pSystem.Play();
        }

        private void ToHeal(Transform target)
        {
            if (_used) return;
            Heal(target);
        }
        
        private void Heal(Transform targetTransform)
        {
            if (!targetTransform.TryGetComponent(out MortalEntity targetEntity)) return;
            
            if(healValue != 0) targetEntity.Heal(healValue, ValueAppliedMode.PercentageOfMax);
            if(healValueFixed != 0) targetEntity.Heal(healValue, ValueAppliedMode.Fixed);

            UsedZone();
        }

        private void UsedZone()
        {
            _used = true;
            if(pSystem != null) pSystem.Stop();
        }

        #endregion
    }
}
