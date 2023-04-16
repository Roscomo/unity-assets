using System.Collections.Generic;
using UnityEngine;
using WeaponsSystem;

namespace PlayerComponents
{
    public class WeaponInventory : MonoBehaviour
    {
        [SerializeField] private Weapon[] weaponsArray;
        private LinkedList<Weapon> weapons;
        private LinkedListNode<Weapon> _currentWeapon;

        public WeaponData CurrentWeaponData => _currentWeapon.Value.WeaponData;
        public FloatingWeapon CurrentWeaponObject => _currentWeapon.Value.WeaponObject;

        public int CurrentAmmo
        {
            get => _currentWeapon.Value.RemainingAmmo;
            set => _currentWeapon.Value.RemainingAmmo = value;
        }
        
        private void Awake()
        {
            weapons = weaponsArray.Length > 0 ? new LinkedList<Weapon>(weaponsArray) : new LinkedList<Weapon>();
        
            _currentWeapon = weapons.First;
            WeaponInventorySetup();
        }
        
        // Intended to be used only once when this component awakens
        private void WeaponInventorySetup()
        {
            foreach(var weapon in weapons)
            {
                weapon.WeaponObject = Instantiate(weapon.WeaponData.WeaponPrefab, transform);
                weapon.RemainingAmmo = weapon.WeaponData.MaxAmmo;
            
                if (weapon != _currentWeapon.Value)
                {
                    weapon.WeaponObject.gameObject.SetActive(false);
                }
            }
        }

        public void NextWeapon()
        {
            if (_currentWeapon is null)
            {
                return;
            }
        
            _currentWeapon.Value.WeaponObject.gameObject.SetActive(false);
        
            if (weapons.Count > 1)
            {
                _currentWeapon = _currentWeapon.Next ?? weapons.First;
            }

            _currentWeapon?.Value.WeaponObject.gameObject.SetActive(true);
        }
    
        public void PreviousWeapon()
        { 
            if (_currentWeapon is null)
            {
                return;
            }
        
            _currentWeapon.Value.WeaponObject.gameObject.SetActive(false);
        
            if (weapons.Count > 1)
            {
                _currentWeapon = _currentWeapon.Previous ?? weapons.Last;
            }

            _currentWeapon?.Value.WeaponObject.gameObject.SetActive(true);
        }
    
        [System.Serializable]
        private class Weapon
        {
            [SerializeField] private WeaponData weaponData; 
            private FloatingWeapon weaponObject; 
            private int remainingAmmo;

            public WeaponData WeaponData
            {
                get => weaponData;
                set => weaponData = value;
            }

            public FloatingWeapon WeaponObject
            {
                get => weaponObject;
                set => weaponObject = value;
            }

            public int RemainingAmmo
            {
                get => remainingAmmo;
                set => remainingAmmo = value;
            }
        }
    }
}