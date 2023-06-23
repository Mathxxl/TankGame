using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace Entities.Player.Upgrades
{
    /// <summary>
    /// Zones that inflict damages on entities who are in contact with it
    /// </summary>
    public class DarkZone : MonoBehaviour
    {

        #region Attributes

        [SerializeField] private ParticleSystem pSystem;
        private Dictionary<GameObject, Coroutine> _damageRoutines;

        private float _size; //size = radius of the zone
        private float _damages; //how many damages are inflicted each hit
        private float _lifetime; //how long the zone is active
        private float _damageRhythm; //how ofter the zone hits per second
        private List<string> _excludeTags; //object tags that should not be hit

        #endregion

        #region Methods

        #region MonoBehaviours

        private void Awake()
        {
            _excludeTags = new List<string>();
            _damageRoutines = new Dictionary<GameObject, Coroutine>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (HasTags(other)) return;
            if (!other.TryGetComponent(out IDamageable damageable)) return;

            var newC = StartCoroutine(DamageElement(damageable));
            _damageRoutines.Add(other.gameObject, newC);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_damageRoutines.ContainsKey(other.gameObject)) return;
            StopCoroutine(_damageRoutines[other.gameObject]);
        }

        #endregion
        
        #region Private Methods

        /// <summary>
        /// Returns true if the component has a tag in the exclude list
        /// </summary>
        private bool HasTags(Component col)
        {
            var go = col.gameObject;
            return _excludeTags.Any(t => go.CompareTag(t));
        }

        /// <summary>
        /// Coroutine that damages the element in parameter at a regular pace
        /// </summary>
        private IEnumerator DamageElement(IDamageable damageable)
        {
            var fact = (_damageRhythm != 0) ? (1 / _damageRhythm) : 1;
            var counterSecurity = 0; //security to avoid infinite loops
            
            while (counterSecurity < 10000)
            {
                yield return new WaitForSeconds(fact);
                damageable.TakeDamages(_damages);
                counterSecurity++;
            }
            
            Debug.LogWarning($"Coroutine DamageElement for {damageable} stopped after {counterSecurity} loops");
        }

        /// <summary>
        /// Coroutine that managed the lifetime of the zone, make it disappear at the end
        /// </summary>
        private IEnumerator LifeControl()
        {
            yield return new WaitForSeconds(_lifetime);

            pSystem.Stop();
            StopAllDamagesRoutines();
            
            yield return new WaitForSeconds(2f);
            
            Destroy(gameObject);
        }
        
        private void StopAllDamagesRoutines()
        {
            foreach (var (_, rout) in _damageRoutines)
            {
                StopCoroutine(rout);
            }
        }

        #endregion

        #region Public Methods

        public void SetParameters(float size, float damages, float lifetime, float rhythm, IEnumerable<string> tags)
        {
            _size = size;
            transform.localScale = Vector3.one * _size;
            
            _damages = damages;
            _lifetime = lifetime;
            _damageRhythm = rhythm;

            foreach (var str in tags)
            {
                _excludeTags.Add(str);
            }
        }

        public void StartDarkZone()
        {
            pSystem.Play();
            StartCoroutine(LifeControl());
        }

        #endregion

        #endregion
    }
}