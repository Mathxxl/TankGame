using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using World;

namespace GameManagers
{
    public class Portal : MonoBehaviour
    {
        private WorldType _linkedType;
        [HideInInspector] public GamePortalManager manager;

        [Header("UI Components")] 
        
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI upgradesText;
        [SerializeField] private Button button;

        public void LinkTo(WorldType type, WorldData data)
        {
            _linkedType = type;
            SetUIElements(data);
            button.onClick.AddListener(manager.OnCallEnd);
        }

        //Add to the choose button
        public void ChooseThisPortal()
        {
            manager.Events.OnWorldChosen?.Invoke(_linkedType);
        }

        private void SetUIElements(WorldData data)
        {
            //Test null
            if (data == null)
            {
                Debug.LogWarning("Data is null for portal");
                return;
            }
            
            //Name
            if (nameText != null)
            {
                nameText.text = data.worldName;
            }
            
            //Desc
            if (descriptionText != null)
            {
                descriptionText.text = data.description;
            }
            
            //Upgrades
            if (upgradesText != null)
            {
                var s = data.possibleUpgrades.Aggregate("Possible Upgrades : ", (current, up) => current + (up + ", "));
                upgradesText.text = s;
            }
            
            //Image
            if (image != null)
            {
                image.sprite = data.icon;
            }
        }
    }
}