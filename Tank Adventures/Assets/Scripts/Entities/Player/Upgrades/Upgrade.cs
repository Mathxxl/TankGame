using Entities.Player.Player_Systems;
using UnityEngine;

namespace Entities.Player.Upgrades
{
    /// <summary>
    /// Represent upgrades
    /// NOTE : les upgrades seront probablements des prefabs à terme
    /// </summary>
    public abstract class Upgrade : MonoBehaviour
    {
        [SerializeField] protected UpgradeData data;

        private World.WorldType _worldType;
        private bool _isStatic = true;
        protected int Level;
        protected UpgradeData.UpgradeStage CurrentStage;
        
        //NOTE : maybe should be hidden in inspector
        [HideInInspector] public PlayerUpgradesManager manager;
        public World.WorldType ThisWorldType => _worldType;
        public bool ThisIsStatic => _isStatic;
        public UpgradeData Data => data;
        public int ThisLevel => Level;

        protected void Awake()
        {
            if (data != null)
            {
                _worldType = data.worldType;
                if(data.stages.Count > 0) CurrentStage = data.stages[0];
                else Debug.LogWarning("Upgrade created without stages");
            }
            else
            {
                Debug.LogWarning("Upgrade created without data");
            }
        }

        public void OnUpgradeObtained()
        {
            UpgradeObtained();
        }

        public void OnUpgradeUpdate()
        {
            UpgradeUpdate();
        }

        public void OnUpgradeLevelUp()
        {
            Level++;
            if (Level > data.stages.Count)
            {
                Debug.LogWarning("Try to level up while not enough stages");
                return;
            }
            
            CurrentStage = data.stages[Level];
            _isStatic = CurrentStage.isStatic;
            
            UpgradeLevelUp();
        }
        
        protected virtual void UpgradeObtained(){}
        protected virtual void UpgradeUpdate(){}
        protected virtual void UpgradeLevelUp(){}
    }
}