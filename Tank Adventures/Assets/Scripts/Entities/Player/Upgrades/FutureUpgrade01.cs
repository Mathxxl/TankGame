using UnityEngine;

namespace Entities.Player.Upgrades
{
    /// <summary>
    /// Upgrade 01 of the Future World
    /// </summary>
    /// <remarks>
    /// Stage 1 : Improve Defense <br/>
    /// Stage 2 : Improve Defense <br/>
    /// Stage 3 : Improve Defense + May reflect some attacks
    /// </remarks>
    public class FutureUpgrade01 : Upgrade
    {
        [SerializeField] private GameObject deflectZonePrefab;
        
        protected override void UpgradeObtained(){}

        protected override void UpgradeUpdate(){}

        protected override void UpgradeLevelUp()
        {
            switch (Level)
            {
                case 2:
                    SetupSendBackAttacks();
                    break;
            }
        }
        
        private void SetupSendBackAttacks()
        {
            Instantiate(deflectZonePrefab, manager.ThisEntity.transform);
        }
    }
}