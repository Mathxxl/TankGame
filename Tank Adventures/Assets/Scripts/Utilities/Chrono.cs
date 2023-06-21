using System;
using System.Collections;
using UnityEngine;

namespace Utilities
{
    public class Chrono : MonoBehaviour
    {
        [SerializeField] private float startTime;
        [SerializeField] private float goalTime;
        private float _currentTime;
        private bool _running;
        private int _dir;
        private Coroutine _currentRoutine;

        private float CurrentTime
        {
            get => _currentTime;
            set
            {
                if(Math.Abs(_currentTime - value) > 0.001f) OnUpdateCurrentTime?.Invoke(value); 
                _currentTime = value;
            }
        }

        public void OnEnable()
        {
            ResetChrono();
        }

        public void ResumeChrono()
        {
            if (_running) return;
            _currentRoutine = StartCoroutine(Timing());
            _running = true;
        }

        public void PauseChrono()
        {
            if (!_running) return;
            StopCoroutine(_currentRoutine);
            _running = false;
        }

        public void ResetChrono()
        {
            PauseChrono();
            CurrentTime = startTime;
            _dir = (goalTime - startTime) > 0 ? 1 : -1;
        }

        private IEnumerator Timing()
        {
            while (Mathf.Abs(CurrentTime - goalTime) > 0.01f)
            {
                yield return null;
                CurrentTime += _dir * Time.deltaTime;
            }
            OnGoalTimeReached?.Invoke();
            Debug.Log("Goal Time Reached");
        }

        public event Action<float> OnUpdateCurrentTime;
        public event Action OnGoalTimeReached;
    }
}