using UnityEngine;

namespace Entities.Entity_Systems.Weapons
{
    [CreateAssetMenu(menuName = "ScriptableObjects/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        public float damages;
        public float cooldown;
        public float range;
    }
}