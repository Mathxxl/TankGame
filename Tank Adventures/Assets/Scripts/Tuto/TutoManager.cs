using System;
using System.Collections.Generic;
using System.Linq;
using GameManagers;
using UnityEngine;

namespace Tuto
{
    [Serializable]
    public struct TutoData
    {
        public string nameID;
        [TextArea] public string description;
        public Vector2 position;
        [HideInInspector] public bool hasBeenSeen;
    }
    
    public class TutoManager : Manager
    {
        [SerializeField] private TutoCard card;
        [SerializeField] private List<TutoData> data;
        private TutoData _currentData;

        private void OnEnable()
        {
            gameManager.Events.OnLevelStart += () => Subscribe("MovementTuto");
        }

        private void OnDisable()
        {
            gameManager.Events.OnLevelStart -= () => Subscribe("MovementTuto");
        }

        private void Subscribe(string tutoName)
        {
            if(!GetDataByName(tutoName)) return;
            if (_currentData.hasBeenSeen) return;
            
            card.Set(_currentData.description);
            card.gameObject.transform.localPosition = _currentData.position;
            _currentData.hasBeenSeen = true;

            card.gameObject.SetActive(true);
            StartCoroutine(card.Lifetime());
        }

        private bool GetDataByName(string tutoName)
        {
            foreach (var d in data.Where(d => d.nameID == tutoName))
            {
                _currentData = d;
                return true;
            }
            return false;
        }
    }
}