using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Entities.Entity_Systems.UI
{
    public class UltimateUI : UISystem
    {
         #region UI Element
        
         [SerializeField] private Image ultimateWindowCooldown;

        #endregion

        #region Mono Behaviours

        protected override void Awake()
        {
            UpdateUltimateWindow(0);
        }

        private void OnEnable()
        {
            controller.Entity.Events.OnUltimateCoolingDown += UpdateUltimateWindow;
        }

        private void OnDisable()
        {
            controller.Entity.Events.OnUltimateCoolingDown -= UpdateUltimateWindow;
        }

        #endregion

        #region Methods

        private void UpdateUltimateWindow(float cooldown)
        {
            if (ultimateWindowCooldown == null) return;

            StartCoroutine(UpdateUltimateWindowCooldown(cooldown));
        }   
        
        private IEnumerator UpdateUltimateWindowCooldown(float cooldown)
        {
            ultimateWindowCooldown.fillAmount = 1;
            var step = 1 / cooldown;
            while (ultimateWindowCooldown.fillAmount > 0)
            {
                ultimateWindowCooldown.fillAmount -= step * Time.deltaTime;
                yield return null;
            }
        }

        #endregion
        
    }
}