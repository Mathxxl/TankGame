using UnityEngine;

namespace Entities.Entity_Systems.UI
{
    public class EnemyHealthBarUI : EntityHealthBarUI
    {
        private void Start()
        {
            slider.gameObject.SetActive(false);
        }

        protected override void OnEnable()
        {
            controller.Entity.Events.OnTakeDamage += IsAttacked;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            controller.Entity.Events.OnTakeDamage -= IsAttacked;
            base.OnDisable();
        }

        private void IsAttacked(float damagesTaken)
        {
            //Active Slider
            if (!slider.gameObject.activeSelf)
            {
                slider.gameObject.SetActive(true);
            }
        }
    }
}