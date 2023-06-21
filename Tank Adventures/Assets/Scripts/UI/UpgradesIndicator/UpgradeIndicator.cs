using System;
using Entities.Player.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.UpgradesIndicator
{
    public class UpgradeIndicator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject descriptionHolder;
        [SerializeField] private TextMeshProUGUI textDescription;
        [SerializeField] private Image icon;

        private UpgradeData _data;
        private int _level;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            descriptionHolder.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            descriptionHolder.SetActive(false);
        }

        public void Setup(Upgrade upgrade)
        {
            descriptionHolder.SetActive(false);
            _data = upgrade.Data;
            _level = upgrade.ThisLevel;

            SetAll();
        }

        public void LevelUp()
        {
            _level++;
            SetAll();
        }

        private void SetAll()
        {
            SetIcon();
            SetDescription();
        }

        private void SetIcon()
        {
            if (_data == null || icon == null) return;

            icon.sprite = _data.stages[_level].icon;
        }

        private void SetDescription()
        {
            if (_data == null || textDescription == null) return;

            textDescription.text = _data.stages[_level].description;
        }
    }
}