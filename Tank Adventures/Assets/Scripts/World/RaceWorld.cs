using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace World
{
    public class RaceWorld : World
    {
        [Serializable]
        public struct LevelBonusParameters
        {
            public string sceneName;
            public bool hasChrono;
            public float startTime;
            public float goalTime;
        }
        [SerializeField] private List<LevelBonusParameters> bonusParameters;
        [SerializeField] private Chrono chrono;
        [SerializeField] private TextMeshProUGUI textPreparation;
        private PlayerDetector _goalPoint;

        protected override void OnAwake()
        {
            textPreparation.gameObject.SetActive(false);
            chrono.gameObject.SetActive(false);
        }
        
        protected override void OnEnter()
        {
            //Components
            chrono.gameObject.SetActive(true);
            
            //Events
            chrono.OnGoalTimeReached += PlayerFailedToReachGoal;
            manager.GManager.Events.OnLevelReached += LevelStart;
        }

        protected override void OnExit()
        {
            //Events
            chrono.OnGoalTimeReached -= PlayerFailedToReachGoal;
            if(_goalPoint != null) _goalPoint.OnPlayerDetected -= PlayerReachGoal;
            manager.GManager.Events.OnLevelReached -= LevelStart;
            
            //Components
            chrono.gameObject.SetActive(false);
        }

        protected override void OnUpdate() { }

        protected override void LevelReached() { } //should remain void

        private IEnumerator PrepareChrono()
        {
            if (textPreparation == null)
            {
                chrono.ResumeChrono();
                yield break;
            }
            
            textPreparation.gameObject.SetActive(true);

            for (var i = 3; i > 0; i--)
            {
                textPreparation.text = i.ToString();
                yield return new WaitForSeconds(1);
            }

            base.LevelReached();
            
            textPreparation.text = "Go !";
            chrono.ResumeChrono();

            yield return new WaitForSeconds(1);
            textPreparation.gameObject.SetActive(false);
        }

        private void LevelStart()
        {
            FindGoalPoint();
            SetChrono();
            StartCoroutine(PrepareChrono());
        }

        private void PlayerReachGoal(Transform player)
        {
            manager.GManager.Events.OnGoalAchieved?.Invoke();
            chrono.PauseChrono();
        }

        private void PlayerFailedToReachGoal()
        {
            manager.GManager.Events.OnGoalFailed?.Invoke();
            chrono.PauseChrono();
        }

        private void SetChrono()
        {
            var currentScene = SceneManager.GetActiveScene().name;

            foreach (var parameter in bonusParameters.Where(parameter => parameter.hasChrono).Where(parameter => parameter.sceneName == currentScene))
            {
                chrono.SetValues(parameter.startTime, parameter.goalTime);
                chrono.ResetChrono();
                return;
            }
            
            //TODO : no chrono => kill enemies
        }

        private void FindGoalPoint()
        {
            //Elements
            var obj = GameObject.FindGameObjectWithTag("GoalPoint"); //TODO ajouter condition si point non trouvé, faire en sorte que le niveau se termine si on termine la course et pas qu'on tue tous les ennemis
            if (obj != null)
            {
                if (obj.TryGetComponent(out PlayerDetector detector))
                {
                    _goalPoint = detector;
                    _goalPoint.OnPlayerDetected += PlayerReachGoal;
                }
                else
                {
                    Debug.LogWarning("No Player Detector found on goal point");
                }
            }
            else
            {
                Debug.LogWarning("the goal point object is null");
            }
        }
    }
}