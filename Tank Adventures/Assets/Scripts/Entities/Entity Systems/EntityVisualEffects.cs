using System;
using System.Collections;
using UnityEngine;

namespace Entities.Entity_Systems
{
    public class EntityVisualEffects : EntitySystem
    {
        #region Attributes

        [SerializeField] private ParticleSystem invincibleParticles;
        [SerializeField] private GameObject model;

        [SerializeField] private float flashRythm;
        [SerializeField] private float flashTime;

        #endregion

        #region Methods

        private void OnEnable()
        {
            entity.Events.OnInvincible += OnInvincibleVisual;
            entity.Events.OnTakeDamage += f => StartCoroutine(InvincibleFlash());
        }

        private void OnDisable()
        {
            entity.Events.OnInvincible -= OnInvincibleVisual;
            entity.Events.OnTakeDamage -= f => StartCoroutine(InvincibleFlash());
        }

        private void OnInvincibleVisual(bool isInvincible)
        {
            if (isInvincible)
            {
                invincibleParticles.Play();
            }
            else
            {
                invincibleParticles.Stop();
            }
        }

        private IEnumerator InvincibleFlash()
        {
            if (model == null) yield break;

            var i = 0f;
            while (i < flashTime)
            {
                model.SetActive(false);
                yield return new WaitForSeconds(flashRythm);
                model.SetActive(true);
                yield return new WaitForSeconds(flashRythm);
                i += 2*flashRythm;
            }
        }

        #endregion
    }
}