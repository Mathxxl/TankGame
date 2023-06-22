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
    }
    
    public class TutoManager : Manager
    {
        [SerializeField] private TutoCard card;
        [SerializeField] private List<TutoData> data;
        private Dictionary<TutoData, bool> _dataChecked;
        private TutoData _currentData;

        private void Awake()
        {
            card.gameObject.SetActive(false);
            CreateDic();
        }

        private void OnEnable()
        {
            gameManager.Events.OnZoneStart += () => Subscribe("MovementTuto");
        }

        private void OnDisable()
        {
            gameManager.Events.OnZoneStart -= () => Subscribe("MovementTuto");
        }

        private void Subscribe(string tutoName)
        {
            if(!GetDataByName(tutoName)) return;

            card.Set(_currentData.description);
            card.gameObject.transform.localPosition = _currentData.position;

            card.gameObject.SetActive(true);
            StartCoroutine(card.Lifetime());
        }

        private bool GetDataByName(string tutoName)
        {
            foreach (var pair in _dataChecked.Where(pair => pair.Key.nameID == tutoName && !pair.Value))
            {
                _currentData = pair.Key;
                _dataChecked[_currentData] = true;
                return true;
            }

            return false;
        }

        private void CreateDic()
        {
            _dataChecked = new Dictionary<TutoData, bool>();

            foreach (var d in data)
            {
                _dataChecked.Add(d, false);
            }
        }
    }
}