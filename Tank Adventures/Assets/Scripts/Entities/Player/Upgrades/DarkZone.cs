using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace Entities.Player.Upgrades
{
    public class DarkZone : MonoBehaviour
    {
        [SerializeField] private ParticleSystem pSystem;
        private Dictionary<GameObject, Coroutine> _damageRoutines;

        private float _size;
        private float _damages;
        private float _lifetime;
        private float _damageRhythm;
        private List<string> _excludeTags;

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

        private bool HasTags(Component col)
        {
            var go = col.gameObject;
            return _excludeTags.Any(t => go.CompareTag(t));
        }

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

        private IEnumerator LifeControl()
        {
            yield return new WaitForSeconds(_lifetime);

            pSystem.Stop();
            StopAllDamagesRoutines();
            
            yield return new WaitForSeconds(2f);
            
            Destroy(gameObject);
        }

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

        private void StopAllDamagesRoutines()
        {
            foreach (var (_, rout) in _damageRoutines)
            {
                StopCoroutine(rout);
            }
        }
    }
}