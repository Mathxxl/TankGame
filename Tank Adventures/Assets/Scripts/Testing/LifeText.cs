using System;
using Entities;
using TMPro;
using UnityEngine;

namespace Testing
{
    public class LifeText : MonoBehaviour
    {
        [SerializeField] private Entity entity;
        [SerializeField] private TextMeshProUGUI displaytext;

        private void OnEnable()
        {
            entity.Events.OnHealthChanged += UpdateText;
        }

        private void OnDisable()
        {
            entity.Events.OnHealthChanged -= UpdateText;
        }

        private void UpdateText(float value)
        {
            displaytext.text = value.ToString();
        }
        
    }
}