using System;
using System.Collections.Generic;
using Entities.Player.Player_Systems;
using GameManagers;
using UnityEngine;

namespace Entities.Player.Upgrades
{
    /// <summary>
    /// Manages the UI for getting upgrades and the connexion to components
    /// </summary>
    public class GetUpgradeUI : Manager
    {
        [Header("UI Elements")] 
        
        [SerializeField] private GameObject cardPrefab;
        
        [Header("Components")]
        
        [SerializeField] private GameUpgradeManager upgradeManager;
        [SerializeField] private GameWorldManager worldManager;

        private PlayerUpgradesManager _playerUpgradesManager; 
        private static readonly int OnEnterTrigger = Animator.StringToHash("OnEnterTrigger");

        private void Awake()
        {
            _playerUpgradesManager = gameManager.Player.GetComponentInChildren<PlayerUpgradesManager>();
        }

        private void OnEnable()
        {
            gameManager.Events.OnGoalAchieved += OnCallStart;
        }

        private void OnDisable()
        {
            gameManager.Events.OnGoalAchieved -= OnCallStart;
        }

        private void OnCallStart()
        {
            //Check player manager for null
            if (_playerUpgradesManager == null)
            {
                Debug.LogWarning("No player upgrade manager found");
                return;
            }
            
            //Get World Upgrades
            var upgrades = upgradeManager.GetUpgradeFromWorld(worldManager.CurrentWorld.Type);
            if (upgrades == null || upgrades.Count == 0)
            {
                gameManager.Events.OnUpgradeChosen?.Invoke(null);
                return;
            }
            Debug.Log($"{upgrades.Count} upgrades found");
            
            //Creation of Cards
            foreach (var upgrade in upgrades)
            {
                //Get level
                var level = _playerUpgradesManager.GetLevelFromUpgrade(upgrade);
                if (level + 1 >= upgrade.Data.stages.Count) continue; //Exit if already at max level
                
                //Instantiate object
                var newCard = Instantiate(cardPrefab, transform);
                Debug.Log($"New card instantiated {newCard}");
                
                //Set values
                if (newCard.TryGetComponent(out UpgradeCard upgradeCard))
                {
                    SetCard(upgradeCard, upgrade, (level > -1) ? level+1 : 0);
                }
                else
                {
                    Debug.LogWarning("Prefab has no upgrade card component");
                    break;
                }
            }
        }


        private void OnCallEnd()
        {
            //Animation ?
            
            //Remove Cards
            for (var i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        private void SetCard(UpgradeCard card, Upgrade upgrade, int stage = 0)
        {
            var data = upgrade.Data;
            
            if (stage >= data.stages.Count)
            {
                Debug.LogWarning($"Try to Set a card with a stage id higher than available stage : {stage} > {data.stages.Count} for {upgrade.name}");
                return;
            }
            var stageData = data.stages[stage];
            
            //Set name
            card.SetName(stageData.upgradeName);

            //Set description
            card.SetDescription(stageData.description);

            //Set Icon
            card.SetIconImage(stageData.icon);
            
            //Set button
            card.AddListener(() => Debug.Log($"Get Upgrade {stageData.upgradeName}"));

            if (stage == 0)
            {
                card.AddListener(() => _playerUpgradesManager.ObtainUpgrade(upgrade));
            }
            else
            {
                card.AddListener(() => _playerUpgradesManager.LevelUpUpgrade(upgrade));
            }
            
            card.AddListener(OnCallEnd);
            card.AddListener(() => gameManager.Events.OnUpgradeChosen?.Invoke(upgrade));
            
            //Do the animation
            card.CardAnimator.SetTrigger(OnEnterTrigger);
        }
    }
    
}