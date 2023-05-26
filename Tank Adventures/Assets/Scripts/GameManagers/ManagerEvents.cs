using System;
using Entities.Player.Upgrades;
using World;

namespace GameManagers
{
    public struct ManagerEvents
    {
        //World
        public Action<WorldType> OnWorldChanged;
        public Action<WorldType> OnWorldLeft;
        public Action<WorldType> OnWorldJoin;
        public Action<WorldType> OnWorldChosen;

        //Upgrades
        public Action<Upgrade> OnUpgradeChosen;
        
        //Enemy
        public Action OnAllEnemyDead;
        
        //Goal
        public Action OnGoalReached;
        
        //Gameflow
        public Action OnLevelStart;
        public Action OnGoalAchieved;
    }
}