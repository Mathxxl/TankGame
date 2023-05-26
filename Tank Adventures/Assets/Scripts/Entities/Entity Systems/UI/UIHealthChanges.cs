using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Entities.Entity_Systems.UI
{
    /// <summary>
    /// Display changes in health
    /// </summary>
    public class UIHealthChanges : UISystem
    {
        #region Attributes

        [Header("Components")]
        
        [SerializeField] private GameObject healthChangeObject;
        [SerializeField] private Animator animator;

        [Header("Parameters")] 
        
        [SerializeField] private float timeVisible = 1f;
        [SerializeField] private Color onTakeDamageColor = Color.red;
        [SerializeField] private Color onHealedColor = Color.green;
        
        private TextMeshProUGUI _healthChangeText;
        private Image _healthChangeBackground;
        private static readonly int OnHealthChangeTrigger = Animator.StringToHash("OnHealthChangeTrigger");

        #endregion

        #region Methods

        protected override void Awake()
        {
            base.Awake();
            
            if(healthChangeObject == null) return;
            
            healthChangeObject.gameObject.SetActive(false);
            _healthChangeText = healthChangeObject.GetComponentInChildren<TextMeshProUGUI>();
            _healthChangeBackground = healthChangeObject.GetComponentInChildren<Image>();
        }

        private void OnEnable()
        {
            controller.Entity.Events.OnTakeDamage += DisplayDamages;
            controller.Entity.Events.OnHealed += DisplayHeal;
        }

        private void OnDisable()
        {
            controller.Entity.Events.OnTakeDamage -= DisplayDamages;
            controller.Entity.Events.OnHealed -= DisplayHeal;
        }

        private void DisplayDamages(float value)
        {
            Display(value, true);
        }

        private void DisplayHeal(float value)
        {
            Display(value, false);
        }

        private void Display(float value, bool isDamages)
        {
            StartCoroutine(IconDisappear());
            healthChangeObject.gameObject.SetActive(true);

            if (_healthChangeText == null) return;
            _healthChangeText.text = (isDamages ? "-" : "+") + value;

            if (_healthChangeBackground == null) return;
            _healthChangeBackground.color = (isDamages ?  onTakeDamageColor : onHealedColor);
            
            if(animator == null) return;
            animator.SetTrigger(OnHealthChangeTrigger);
        }

        private IEnumerator IconDisappear()
        {
            yield return new WaitForSeconds(timeVisible);
            healthChangeObject.gameObject.SetActive(false);
        }

        #endregion
    }
}