using System.Collections;
using UnityEngine;
using World;

namespace Entities.Player.Ultimate
{
    /// <summary>
    /// Abstract class describing ultimates
    /// </summary>
    public abstract class Ultimate : EntitySystem
    { 
        [SerializeField] protected UltimateData data;
        protected bool IsCoolingDown;
        public WorldType Type => data.type;

       protected void OnUltimate()
       {
           if (IsCoolingDown) return;
           entity.Events.OnUltimateUsed?.Invoke(this);
           DoUltimate();
           StartCoroutine(CoolingDown());
       }
       
       protected virtual void DoUltimate(){}

       private IEnumerator CoolingDown()
       {
           IsCoolingDown = true;
           entity.Events.OnUltimateCoolingDown?.Invoke(data.cooldown);
           yield return new WaitForSeconds(data.cooldown);
           IsCoolingDown = false;
       }
    }
}
