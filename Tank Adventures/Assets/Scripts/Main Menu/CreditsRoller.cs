using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Main_Menu
{
    public class CreditsRoller : CreditsMenuElement
    {
        [SerializeField] [TextArea] private string[] credits;
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private float timeDifference = 0.2f;
        [SerializeField] private GameObject textPrefab;

        private List<GameObject> _activeCredits;

        private void Start()
        {
            Roll();
        }

        private void Roll()
        {
            _activeCredits = new List<GameObject>();
            manager.OnCreditsStart?.Invoke();
            StartCoroutine(Rolling());
        }
        
        private IEnumerator Rolling()
        {
            yield return null;

            foreach (var credit in credits)
            {
                yield return new WaitForSeconds(timeDifference);
                InstantiateText(credit);
            }
        }

        private void InstantiateText(string text)
        {
            var objText = Instantiate(textPrefab, transform);
            if (objText.TryGetComponent(out TextMeshProUGUI tm))
            {
                tm.text = text;
            }
            _activeCredits.Add(objText);

            StartCoroutine(RollElement(objText));
        }
        
        private IEnumerator RollElement(GameObject obj)
        {
            yield return null;

            while (obj.transform.localPosition.y >= -1200)
            {
                obj.transform.Translate(0,-1*Time.deltaTime*speed,0);
                yield return null;
            }

            _activeCredits.Remove(obj);
            Destroy(obj);

            if (CheckVoid())
            {
                manager.OnCreditsEnd?.Invoke();
                Debug.Log("End Credits");
            }
        }

        private bool CheckVoid()
        {
            return _activeCredits.Count == 0;
        }
    }
}