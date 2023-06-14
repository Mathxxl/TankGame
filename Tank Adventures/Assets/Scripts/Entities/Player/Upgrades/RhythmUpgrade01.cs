using System;
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
        private int _currentBpm;
        private RhythmDescriptor _rhythm;

        private void OnDisable()
        {
            StopCoroutine(OnRhythm());
            
            if(manager != null) manager.ThisEntity.Events.OnPerformingAttack -= OnShoot;
        }

        protected override void UpgradeObtained()
        {
            //Values
            audioManager ??= FindObjectOfType<AudioManager>();
            _currentBpm = GetBpm();
            
            //Events linkage
            manager.ThisEntity.Events.OnPerformingAttack += OnShoot;
            
            //Behaviours
            StartCoroutine(OnRhythm());
            //TODO : Ajouter représentation graphique
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
            //TODO : vague d'énergie
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

                _rhythm = RhythmDescriptor.TooSoon;
                yield return new WaitForSeconds(60f/(_currentBpm*2f));
                
                //PERFECT

                _rhythm = RhythmDescriptor.Perfect;
                yield return new WaitForSeconds(rhythmFloor);
                
                //TOO LATE

                _rhythm = RhythmDescriptor.TooLate;
                yield return new WaitForSeconds(60f/(_currentBpm*2f));
                
            }
        }
    }
}