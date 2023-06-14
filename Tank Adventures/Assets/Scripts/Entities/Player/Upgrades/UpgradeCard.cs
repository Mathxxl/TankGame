using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Entities.Player.Upgrades
{
    /// <summary>
    /// Manages Upgrade Cards that represent an upgrade in the UI
    /// </summary>
    public class UpgradeCard : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image iconImage;
        [SerializeField] private Animator animator;

        public Animator CardAnimator => animator;

        #region UI Setters

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }

        public void SetName(string setName)
        {
            nameText.text = setName;
        }

        public void SetDescription(string setDescription)
        {
            descriptionText.text = setDescription;
        }

        public void SetIconImage(Sprite setSprite)
        {
            if (setSprite == null) return;
            iconImage.sprite = setSprite;
        }

        public void AddListener(UnityAction action)
        {
            button.onClick.AddListener(action);
        }

        #endregion
    }
}