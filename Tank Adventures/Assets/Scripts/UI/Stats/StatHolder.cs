using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Stats
{
    public class StatHolder : MonoBehaviour //TODO : Ajouter max health pour l'affichage de la vie
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI textName;
        [SerializeField] private TextMeshProUGUI textValue;

        public void SetTextName(string newTextName)
        {
            textName.text = newTextName;
        }

        public void SetValue(float value)
        {
            Debug.Log($"Set value for {name} to {value}");
            value = TruncateValue(value, 2);
            SetTextValue(value);
            SetSliderValue(value);
        }

        public void SetValueInv(float value)
        {
            value = TruncateValue(value, 2);
            SetTextValue(value);
            SetSliderValue(slider.maxValue - value);
        }

        private void SetTextValue(float value)
        {
            textValue.text = $"{value}";
        }

        private void SetSliderValue(float value)
        {
            slider.value = value;
        }

        private static float TruncateValue(float value, int power)
        {
            var pow = Mathf.Pow(10, power);
            value *= pow;
            value = Mathf.Floor(value);
            value /= pow;
            return value;
        }
    }
}
