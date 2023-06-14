using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Entities.Entity_Systems.UI
{
    public class EntityHealthBarUI : UISystem
    {
        #region UI Elements

        [SerializeField] protected Slider slider;

        #endregion

        #region Attributes

        [SerializeField] private float lifeChangeDuration = 0.2f;
        private float _refWidthSlider;
        private RectTransform _sliderRectTransform;
        //private bool _isUpdating;
        private Coroutine _currentCoroutine;

        #endregion

        #region Mono Behaviours

        protected virtual void OnEnable()
        {
            controller.Entity.Events.OnHealthChanged += UpdateSliderHealth;
            controller.Entity.Events.OnMaxHealthChanged += UpdateMaxHealthSlider;
        }

        protected virtual void OnDisable()
        {
            controller.Entity.Events.OnHealthChanged -= UpdateSliderHealth;
            controller.Entity.Events.OnMaxHealthChanged -= UpdateMaxHealthSlider;
        }

        protected override void Awake()
        {
            base.Awake();
            
            //Slider
            _sliderRectTransform = slider.transform as RectTransform;
            if (_sliderRectTransform != null) _refWidthSlider = _sliderRectTransform.sizeDelta.x;
        }

        #endregion

        #region Methods

        private void UpdateMaxHealthSlider(float value)
        {
            if (Math.Abs(value - slider.maxValue) < 0.01f) return;
            if (value < 0) return;

            _sliderRectTransform.sizeDelta = new Vector2(_refWidthSlider * (value/((slider.maxValue > 0) ? slider.maxValue : value)), _sliderRectTransform.sizeDelta.y);
            
            slider.maxValue = value;
        }

        private void UpdateSliderHealth(float value)
        {
            if (slider == null) return;

            value = (value > 0) ? value : 0;
            value = (value > slider.maxValue) ? slider.maxValue : value;
            
            if(_currentCoroutine != null) StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(UpdateSliderHealthSmooth(value));
        }

        private IEnumerator UpdateSliderHealthSmooth(float value)
        {
            var diff = value - slider.value;
            var step = diff / lifeChangeDuration; //step with duration of change
            var goOut = false;
            //_isUpdating = true;
            var ename = controller.Entity.name; //debug

            //Debug.Log($"/!\\ STEP == {step * Time.deltaTime} for value = {value}, slider.value = {slider.value}");
            
            while (Math.Abs(slider.value - value) > 0.1f)
            {
                //Debug.Log("<<<< Dbut de boucle >>>>>");
                
                for (var i = 0; i < 20; i++)
                {
                    slider.value += 0.05f * step * Time.deltaTime;
                    
                    //Debug.Log($"[{ename}] slidervalue = {slider.value}, rstep = {0.01f * step * Time.deltaTime}");
                    
                    if (Math.Abs(slider.value - slider.maxValue) < 0.001f || slider.value < 0.005f)
                    {
                        //Debug.LogWarning("MIN/MAX");
                        goOut = true;
                        break;
                    }
                    if (Math.Abs(slider.value - value) < 0.1f)
                    {
                        //Debug.LogWarning("Value attained");
                        goOut = true;
                        break;
                    }
                }
                if (Math.Abs(slider.value - value) > slider.maxValue || goOut) break;
                
                yield return null;
            }
            
            slider.value = value;
            //Debug.Log($"[{ename}] ****** slider value = {slider.value}, value = {value} ************");
            //_isUpdating = false;
        }

        #endregion
    }
}