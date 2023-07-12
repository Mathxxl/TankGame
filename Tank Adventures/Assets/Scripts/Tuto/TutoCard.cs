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
        private bool _isEnded = false;

        private void Awake()
        {
            nextButton.onClick.AddListener(ButtonEnd);
            if(slider != null) slider.maxValue = lifeDuration;
        }

        private void Start()
        {
            StartCoroutine(TutoEnabled());
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

            if (!_isEnded)
            {
                Destroy(gameObject);
            }
        }

        private void ButtonEnd()
        {
            if (_isEnded) return;
            Destroy(gameObject);
            _isEnded = true;
        }

        private IEnumerator TutoEnabled()
        {
            yield return null;
            StartCoroutine(Lifetime());
        }
        
    }
}