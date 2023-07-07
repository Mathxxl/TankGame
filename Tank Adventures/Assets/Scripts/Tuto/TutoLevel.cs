using System;
using System.Linq;
using UnityEngine;
using Utilities;

namespace Tuto
{
    public class TutoLevel : MonoBehaviour
    {
        [Serializable]
        private struct TutoZone
        {
            public PlayerDetector detector;
            public string tutoName;
        }
        
        public TutoEvents Events = new TutoEvents();

        [SerializeField] private TutoZone[] tutoTriggers;
        
        private PlayerDetector GetDetectorWithName(string searchedName)
        {
            return (from trigger in tutoTriggers where trigger.tutoName == searchedName select trigger.detector).FirstOrDefault();
        }

        private void OnEnable()
        {
            PlayerDetector detector;
            detector = GetDetectorWithName("TutoAttack");
            if (detector != null) detector.OnPlayerDetected += (_) => Events.OnTutoAttack?.Invoke();
            detector = GetDetectorWithName("TutoUltimate");
            if (detector != null) detector.OnPlayerDetected += (_) => Events.OnTutoUltimate?.Invoke();
            detector = GetDetectorWithName("TutoMenu");
            if (detector != null) detector.OnPlayerDetected += (_) => Events.OnTutoMenu?.Invoke();
        }

        private void OnDestroy()
        {
            SubUnlink("TutoAttack");
            SubUnlink("TutoGameflow");
            SubUnlink("TutoUltimate");
            SubUnlink("TutoMenu");
            SubUnlink("TutoUpgrade");
        }

        /*
        private void SubLink(string actionName, Action action)
        {
            var detector = GetDetectorWithName(actionName);
            if (detector != null)
            {
                Debug.Log($"Sublink {actionName} to {action}");
                detector.OnPlayerDetected += (_) => action?.Invoke();

                action += () => Debug.Log($"action {action} called");
            }
        }*/
        
        private void SubUnlink(string actionName)
        {
            var detector = GetDetectorWithName(actionName);
            if (detector != null) Destroy(detector.gameObject);
        }
    }
}
