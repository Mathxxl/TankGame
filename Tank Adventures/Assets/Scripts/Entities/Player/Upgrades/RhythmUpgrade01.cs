﻿using System;
using System.Collections;
using GameManagers;
using UnityEngine;

namespace Entities.Player.Upgrades
{
    /// <summary>
    /// Upgrade 01 of Rhythm World : Increase damages when shoots are in rhythm
    /// </summary>
    /// <remarks>
    /// Stage 1 : Increase damages when shooting in rhythm <br/>
    /// Stage 2 : Increase rhythm damages <br/>
    /// Stage 3 : Increase rhythm damages and add a wave of energy when shooting on rhythm
    /// </remarks>
    public class RhythmUpgrade01 : Upgrade
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField][Range(0f,1f)] private float rhythmFloor = 0.5f;
        [SerializeField] private GameObject visualRepresentationPrefab;
        [SerializeField] private GameObject visualRepresentationParent;
        [SerializeField] private GameObject metronomePrefab;
        [SerializeField] private GameObject metronomeParent;
        [SerializeField] private GameObject explosionPrefab;
        private int _currentBpm;
        private RhythmDescriptor _rhythm;
        private GameObject _visualRepresentation;
        private Coroutine _onRhythmRoutine;

        public RhythmDescriptor Rhythm
        {
            get => _rhythm;
            private set
            {
                _rhythm = value;
                OnRhythmChange?.Invoke(_rhythm);
            }
        }

        public float Floor => rhythmFloor;

        private void OnDisable()
        {
            StopCoroutine(OnRhythm());

            if (_visualRepresentation != null && _visualRepresentation.TryGetComponent(out VisualRhythm vr))
            {
                manager.ThisEntity.Events.OnPerformingAttack -= vr.AttackPerformed;
            }
            
            if (manager == null) return;
            manager.ThisEntity.Events.OnPerformingAttack -= OnShoot;
            manager.ThisEntity.GameManager.Events.OnLevelReached -= LevelChanged;
        }

        protected override void UpgradeObtained()
        {
            //Values
            audioManager ??= FindObjectOfType<AudioManager>();

            //Events linkage
            manager.ThisEntity.Events.OnPerformingAttack += OnShoot;
            manager.ThisEntity.GameManager.Events.OnLevelReached += LevelChanged;
            
            //Behaviours
            StartCoroutine(OnRhythm());
            
            //Visual
            if (visualRepresentationPrefab != null)
            {
                _visualRepresentation = Instantiate(visualRepresentationPrefab, (visualRepresentationParent != null) ? visualRepresentationParent.transform : transform);
                var metro = Instantiate(metronomePrefab, (metronomeParent != null) ? metronomeParent.transform : transform);
                if (_visualRepresentation.TryGetComponent(out VisualRhythm vr))
                {
                    vr.rUpgrade = this;
                    vr.metronomePivot = metro.transform.GetChild(1).gameObject;
                    Debug.Log($"Pivot = {vr.metronomePivot.name}");
                    manager.ThisEntity.Events.OnPerformingAttack += vr.AttackPerformed;
                }
            }
        }

        protected override void UpgradeUpdate()
        {
            
        }

        protected override void UpgradeLevelUp()
        {
            switch (Level)
            {
                case 1:
                    LevelOne();
                    break;
                case 2:
                    LevelTwo();
                    break;
            }
        }

        private void LevelOne()
        {
            Debug.Log("Rhythm Upgrade 01 : Level Up - Damages improved");
        }

        private void LevelTwo()
        {
            
        }
        
        /// <summary>
        /// Called when the player attack, checks for the rhythm and call event to improve damages if perfect
        /// </summary>
        private void OnShoot()
        {
            Debug.Log("Rhythm : shoot");
            
            if (_rhythm == RhythmDescriptor.Perfect)
            {
                Debug.Log("PERFECT");
                
                var values = GetValues(UpgradeData.UpgradeValuesType.Damages);
                if (values == null) return;
                manager.ThisEntity.Events.OnImproveDamageForOneHit?.Invoke(values.Value.percentageValue);
                Debug.Log("[Improve Damage event]");
                
                if (Level >= 2 && explosionPrefab != null)
                {
                    Instantiate(explosionPrefab, manager.transform);
                }
            }
            else
            {
                Debug.Log("FAIL");
            }
        }
        
        private int GetBpm()
        {
            if (audioManager == null || audioManager.CurrentMusic == null) return -1;
            return audioManager.CurrentMusic.bpm;
        }

        private void SetBpm()
        {
            _currentBpm = GetBpm();
            if (_visualRepresentation != null && _visualRepresentation.TryGetComponent(out VisualRhythm vr))
            {
                vr.StartMetronome(_currentBpm);
            }
        }

        private void LevelChanged()
        {
            SetBpm();
            if (_onRhythmRoutine == null) return;
            StopCoroutine(_onRhythmRoutine);
            _onRhythmRoutine = StartCoroutine(OnRhythm());
        }
        
        /// <summary>
        /// Coroutine that changes the _rhythm value over time
        /// </summary>
        /// <returns></returns>
        private IEnumerator OnRhythm()
        {
            while (true)
            {
                //Test on value
                if (_currentBpm <= 0)
                {
                    yield return null;
                    continue;
                }

                //TOO SOON

                //Debug.Log("********** TOO SOON **************");
                Rhythm = RhythmDescriptor.TooSoon;
                yield return new WaitForSeconds(60f / (_currentBpm * 2f));
                
                //PERFECT

                //Debug.Log("********** PERFECT **************");
                Rhythm = RhythmDescriptor.Perfect;
                yield return new WaitForSeconds(rhythmFloor);
                
                //TOO LATE

                //Debug.Log("********** TOO LATE **************");
                Rhythm = RhythmDescriptor.TooLate;
                yield return new WaitForSeconds(60f / (_currentBpm * 2f));
                
            }
        }

        public event Action<RhythmDescriptor> OnRhythmChange;
    }
}