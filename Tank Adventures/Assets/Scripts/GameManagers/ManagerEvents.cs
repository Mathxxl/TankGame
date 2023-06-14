using System;
using Entities.Player.Upgrades;
using World;

namespace GameManagers
{
    /// <summary>
    /// Struct for all events related to GameManagers
    /// </summary>
    public struct ManagerEvents
    {
        //World
        public Action<WorldType> OnWorldChanged;
        public Action<WorldType> OnWorldLeft;
        public Action<WorldType> OnWorldJoin;
        public Action<WorldType> OnWorldChosen;
        public Action OnBeforeFinalWorld;
        public Action OnFinalWorldReached;
        public Action OnFinalWorldEnd;

        //Free Zone
        public Action OnFreeZoneReached; //=> TODO separates events from setup (whichever scene), setup level (enemies etc.) and setup freezone (shops etc.)
        public Action OnFreeZoneQuit;
        
        //Upgrades
        public Action<Upgrade> OnUpgradeChosen;
        public Action<WorldType> OnAllUpgradesGottenForWorld;
        
        //Enemy
        public Action OnAllEnemyDead;
        
        //Goal
        public Action OnGoalPlaceReached;
        
        //Gameflow
        public Action OnLevelStart;
        public Action OnGoalAchieved;
        public Action OnFinalLevelAchieved;
        
        //Home
        public Action OnLevelBeforeHome;
    }
}