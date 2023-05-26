using System.Collections.Generic;
using UnityEngine;

namespace Entities.Entity_Systems.Weapons
{
    /// <summary>
    /// Class for managing Weapons
    /// </summary>
    public class WeaponManager : EntitySystem
    {
        #region Attributes

        [SerializeField] private Weapon currentWeapon;
        private List<Weapon> _obtainedWeapons;

        public Weapon CurrentWeapon => currentWeapon;
        public List<Weapon> ObtainedWeapon => _obtainedWeapons;

        #endregion
        #region Methods

        #region MonoBehaviours

        protected override void Awake()
        {
            base.Awake();

            _obtainedWeapons = new List<Weapon>();
            if(currentWeapon != null) _obtainedWeapons.Add(currentWeapon);
        }

        //The entity has one more weapon possessed
        public void ObtainWeapon(Weapon newWeapon)
        {
            _obtainedWeapons.Add(newWeapon);
            if (currentWeapon == null)
            {
                currentWeapon = newWeapon;
            }
        }

        public void RemoveWeapon(Weapon toRemove)
        {
            _obtainedWeapons.Remove(toRemove);
            if (currentWeapon == toRemove) currentWeapon = null;
        }

        //Change current weapon
        public void SwitchWeapon(Weapon toSwitch)
        {
            if (!_obtainedWeapons.Contains(toSwitch))
            {
                _obtainedWeapons.Add(toSwitch);
            }
            
            currentWeapon = toSwitch;
        }

        #endregion
        

        #endregion
    }
}