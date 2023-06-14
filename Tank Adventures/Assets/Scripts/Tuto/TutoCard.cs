using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tuto
{
    public class TutoCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button nextButton;
        [SerializeField] private Slider slider;
        [SerializeField] private float lifeDuration = 3.0f;

        private void Awake()
        {
            nextButton.onClick.AddListener(() => gameObject.SetActive(false));
            if(slider != null) slider.maxValue = lifeDuration;
        }

        public void Set(string newText)
        {
            text.text = newText;
        }

        public IEnumerator Lifetime()
        {
            if (slider == null) yield break;
            
            var t = 0f;
            while (t < lifeDuration)
            {
                slider.value = (lifeDuration - t);
                t += Time.deltaTime;
                yield return null;
            }
            gameObject.SetActive(false);
        }
    }
}