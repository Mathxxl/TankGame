using System;
using LoadingScreen;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FillBar : MonoBehaviour
    {
        [SerializeField] private Gradient color;
        [SerializeField] private float fadeDuration;
        [SerializeField] private Image image;
        private float _max = 1;
        private float _min = 0;
        private float _value;
        [SerializeField] private Fading fading;

        private void Start()
        {
            if (fading == null)
            {
                var thisGameObject = gameObject;
                fading = thisGameObject.AddComponent<Fading>();
                fading.Set(thisGameObject, null);
                fading.Duration = fadeDuration;
            }
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void ChangeBarValue(float newValue)
        {
            if (newValue > _max || newValue < _min)
            {
                Debug.LogWarning($"UI.FillBar : ChangeBarValue : la nouvelle valeur rentrée n'est pas comprise ({newValue}) entre {_max} et {_min}");
            }
            _value = newValue;
            
            var valNormalize = Mathf.InverseLerp(_min,_max,newValue);
            var newColor = color.Evaluate(valNormalize);
            image.fillAmount = valNormalize;
            image.color = newColor;
        }


        public void ChangeBarMax(float newMax)
        {
            _max = newMax;
            ChangeBarValue(_value);
        }


        public void ChangeBarMin(float newMin)
        {
            _min = newMin;
            ChangeBarValue(_value);
        }

        public void Fade()
        {
            if (fading != null)
            {
                fading.Fade();
            }
        }
    }
}
