using UnityEngine;
using UnityEngine.Serialization;

namespace WeaponsSystem
{
    [CreateAssetMenu]
    public class WeaponData : ScriptableObject
    {   
        //Weapon UI Data
        [SerializeField] private string weaponName;
        [SerializeField] private Texture unusedAmmoSprite;
        [SerializeField] private Texture usedAmmoSprite;
    
        //Weapon object Properties
        [SerializeField] private FloatingWeapon weaponPrefab;
    
        //Projectile Properties
        [SerializeField] private Projectile projectile;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float projectileDamage;

        //Shooting Properties
        [SerializeField] private float spread;
        [SerializeField] private float shootingSpeed;
        [SerializeField] private float numBullets;
        [SerializeField] private FiringMode firingMode;
        [SerializeField] private int burstSize;
        
        //Reloading Properties
        [SerializeField] private float reloadSpeed;
        [SerializeField] private ReloadMode reloadMode;

        //Other Properties
        [SerializeField] private int maxAmmo;
        [SerializeField] private float burstSpeed;
        [SerializeField] private bool spreadShot;

        //Properties for get access
        public string WeaponName => weaponName;

        public Texture UnusedAmmoSprite => unusedAmmoSprite;

        public Texture UsedAmmoSprite => usedAmmoSprite;

        public Projectile Projectile => projectile;
    
        public float ProjectileSpeed => projectileSpeed;
    
        public float ProjectileDamage => projectileDamage;
    
        public float Spread => spread;
    
        public float ShootingSpeed => shootingSpeed;
    
        public float NumBullets => numBullets;
    
        public FiringMode FiringMode => firingMode;
    
        public int MaxAmmo => maxAmmo;
    
        public float BurstSpeed => burstSpeed;
    
        public bool SpreadShot => spreadShot;
    
        public FloatingWeapon WeaponPrefab => weaponPrefab;
    
        public int BurstSize => burstSize;

        public float ReloadSpeed => reloadSpeed;

        public ReloadMode ReloadMode => reloadMode;
    }

    public enum FiringMode
    {
        SingleFire,
        Automatic,
        Burst,
    }

    public enum ReloadMode
    {
        AllAtOnce,
        OneByOne
    }
}