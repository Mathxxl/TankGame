using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Entities.Player.Player_Systems
{
    /// <summary>
    /// Manages the player UI //=> DEPRECATED
    /// </summary>
    public class PlayerUI : EntitySystem
    {
        #region UI Elements

        [SerializeField] private Slider slider;
        [SerializeField] private Image ultimateWindowCooldown;

        #endregion

        #region Attributes

        [SerializeField] private float lifeChangeDuration = 0.2f;
        private float _refWidthSlider;
        private RectTransform _sliderRectTransform;

        #endregion
        
        

        #region Mono Behaviours

        private void OnEnable()
        {
            entity.Events.OnHealthChanged += UpdateSliderHealth;
            entity.Events.OnMaxHealthChanged += UpdateMaxHealthSlider;
            entity.Events.OnUltimateCoolingDown += UpdateUltimateWindow;
        }

        private void OnDisable()
        {
            entity.Events.OnHealthChanged -= UpdateSliderHealth;
            entity.Events.OnMaxHealthChanged -= UpdateMaxHealthSlider;
            entity.Events.OnUltimateCoolingDown -= UpdateUltimateWindow;
        }

        protected override void Awake()
        {
            base.Awake();
            
            //Slider
            _sliderRectTransform = slider.transform as RectTransform;
            if (_sliderRectTransform != null) _refWidthSlider = _sliderRectTransform.sizeDelta.x;
            
            gameObject.SetActive(false);
            gameObject.SetActive(true);
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
            StartCoroutine(UpdateSliderHealthSmooth(value));
        }

        private IEnumerator UpdateSliderHealthSmooth(float value)
        {
            var diff = value - slider.value;
            var step = diff / lifeChangeDuration; //step with duration of change
            
            while (Math.Abs(slider.value - value) > 0.1f)
            {
                slider.value += step * Time.deltaTime;
                yield return null;
            }
        }

        private void UpdateUltimateWindow(float cooldown)
        {
            if (ultimateWindowCooldown == null) return;

            StartCoroutine(UpdateUltimateWindowCooldown(cooldown));
        }   
        
        private IEnumerator UpdateUltimateWindowCooldown(float cooldown)
        {
            ultimateWindowCooldown.fillAmount = 1;
            var step = 1 / cooldown;
            while (ultimateWindowCooldown.fillAmount > 0)
            {
                ultimateWindowCooldown.fillAmount -= step * Time.deltaTime;
                yield return null;
            }
        }

        #endregion
        
    }
}
