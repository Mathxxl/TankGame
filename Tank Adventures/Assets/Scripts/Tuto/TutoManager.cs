using System;
using System.Collections.Generic;
using System.Linq;
using GameManagers;
using UnityEngine;
using World;

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
        [SerializeField] private TutoCard cardPrefab;
        [SerializeField] private GameObject parentUI;
        [SerializeField] private List<TutoData> data;
        [SerializeField] private TutoLevel tutoLevel;
        private Dictionary<TutoData, bool> _dataChecked;
        private TutoData _currentData;

        private void Awake()
        {
            //card.gameObject.SetActive(false);
            CreateDic();
        }

        private void OnEnable()
        {
            if (gameManager == null) return;
            
            gameManager.Events.OnZoneStart += () => Subscribe("MovementTuto");
            gameManager.Events.OnFreeZoneReached += () => Subscribe("HomeTuto");
            gameManager.Events.OnGoalAchieved += () => Subscribe("GameflowTuto");
            gameManager.Events.OnUpgradeChosen += (_) => Subscribe("UpgradeTuto");
            
            if(tutoLevel == null) return;
            
            Debug.Log("Subscribe to tutoLevels events");

            tutoLevel.Events.OnTutoAttack += () => Subscribe("AttackTuto");
            tutoLevel.Events.OnTutoUltimate += () => Subscribe("UltimateTuto");
            tutoLevel.Events.OnTutoMenu += () => Subscribe("StatsTuto");
        }

        private void OnDisable()
        {
            if (gameManager == null) return;
        
            gameManager.Events.OnZoneStart -= () => Subscribe("MovementTuto");
            gameManager.Events.OnFreeZoneReached -= () => Subscribe("HomeTuto");
            gameManager.Events.OnGoalAchieved -= () => Subscribe("GameflowTuto");
            gameManager.Events.OnUpgradeChosen -= (_) => Subscribe("UpgradeTuto");
            
            if(tutoLevel == null) return;
            
            tutoLevel.Events.OnTutoAttack -= () => Subscribe("AttackTuto");
            tutoLevel.Events.OnTutoUltimate -= () => Subscribe("UltimateTuto");
            tutoLevel.Events.OnTutoUpgrade -= () => Subscribe("UpgradeTuto");
        }

        private void Subscribe(string tutoName)
        {
            if(!GetDataByName(tutoName)) return;

            var card = Instantiate(cardPrefab, parentUI.transform);
            
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

        #region Specific Tuto Methods
        

        #endregion
    }
}