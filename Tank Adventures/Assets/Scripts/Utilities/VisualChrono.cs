using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace Utilities
{
    public class VisualChrono : MonoBehaviour
    {
        [SerializeField] private Chrono chrono;

        //UI Components
        [SerializeField] private TextMeshProUGUI chronoText;

        //Internal values
        private int _min;
        private int _seconds;
        private int _centi;

        private void Awake()
        {
            chrono.OnChronoEnabled += ChronoEnabled;
            chrono.OnChronoDisabled += ChronoDisabled;
            gameObject.SetActive(false);
        }
        
        private void OnEnable()
        {
            chrono.OnUpdateCurrentTime += ChronoUpdated;
        }

        private void OnDisable()
        {
            chrono.OnUpdateCurrentTime -= ChronoUpdated;
        }

        private void OnDestroy()
        {
            chrono.OnChronoEnabled -= ChronoEnabled;
            chrono.OnChronoDisabled -= ChronoDisabled;
        }

        private void ChronoUpdated(float value)
        {
            Convert(value);
            Display();
        }
        
        private void Convert(float totalSeconds)
        {
            _min = (int) (totalSeconds / 60);
            totalSeconds -= _min*60;

            _seconds = (int) totalSeconds;
            totalSeconds -= _seconds;

            _centi = (int) (totalSeconds * 100);
        }

        private void Display()
        {
            chronoText.text = ToDisplayNumber(_min) + ":" + ToDisplayNumber(_seconds) + ":" + ToDisplayNumber(_centi);
        }

        private string ToDisplayNumber(float number)
        {
            if (number < 10)
            {
                return "0" + number;
            }
            else
            {
                number = Mathf.Floor(number * 100) / 100;
                return number.ToString(CultureInfo.InvariantCulture);
            }
            
        }

        private void ChronoEnabled()
        {
            gameObject.SetActive(true);
        }

        private void ChronoDisabled()
        {
            gameObject.SetActive(false);
        }
    }
}