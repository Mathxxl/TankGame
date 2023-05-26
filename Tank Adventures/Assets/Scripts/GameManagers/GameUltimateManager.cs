using System.Collections.Generic;
using System.Linq;
using Entities.Player.Ultimate;
using UnityEngine;
using World;

namespace GameManagers
{
    public class GameUltimateManager : Manager
    {
        [SerializeField] private List<Ultimate> availableUltimates;

        public Ultimate GetUltimateFromType(WorldType type)
        {
            return availableUltimates.FirstOrDefault(ult => ult.Type == type);
        }
    }
}