using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace WeaponsSystem
{
    public class Shooter : MonoBehaviour
    {
        //Serialized Fields
        [SerializeField] private WeaponData weapon;
        [SerializeField] private LayerMask damageableLayers;

        //Other Fields
        private int remainingAmmo;
        private bool _shooting;
        private bool _reloading;
        private ObjectPool<Projectile> _projectilePool;
    
        //Events
        public event Action<float> OnAmmoChanged; 
        public event Action<float, float> OnAmmoPercentageChanged;
        public event Action<WeaponData> OnWeaponChanged;
        public event Action<float, ReloadMode> OnBeginReload;
        public event Action OnReloadInterrupted;
        
        //Properties
        public int MaxAmmo => weapon.MaxAmmo;

        private bool CanReload => RemainingAmmo < MaxAmmo && !_reloading;

        public bool FireButtonHeld { get; set; }
        
        public int RemainingAmmo
        {
            get => remainingAmmo;
            set
            {
                remainingAmmo = value > 0 ? value : 0;

                OnAmmoChanged?.Invoke(remainingAmmo);
                OnAmmoPercentageChanged?.Invoke(remainingAmmo, weapon.MaxAmmo);
            }
        }

        public WeaponData Weapon
        {
            get => weapon;
            set
            {
                weapon = value;
                _shooting = false;
                _reloading = false;
                
                OnWeaponChanged?.Invoke(weapon);
                OnReloadInterrupted?.Invoke();
                StopAllCoroutines();
            }
        }

        private void Start()
        {
            ProjectilePoolSetup();
        }
    

        public IEnumerator Shoot(Transform firingPoint, Transform target)
        {
            if ((weapon.FiringMode is not FiringMode.Automatic && FireButtonHeld) 
                || _shooting 
                || (_reloading && weapon.ReloadMode is ReloadMode.AllAtOnce) 
                || remainingAmmo <= 0)
            {
                yield break;
            }

            _shooting = true;

            for (var i = 0; i < (weapon.FiringMode == FiringMode.Burst ? weapon.BurstSize : 1); i++)
            {
                if (remainingAmmo <= 0)
                {
                    break;
                }

                var position = transform.position;
                var direction = (target.position - position).normalized;

                RemainingAmmo -= 1;

                for (var j = 0; j < (weapon.SpreadShot ? weapon.NumBullets : 1); j++)
                {
                    Vector3 newDirection;
                    var newProjectile = _projectilePool.Get();

                    if (weapon.SpreadShot)
                    {
                        newDirection =
                            Quaternion.Euler(Vector3.forward * (weapon.Spread * (j - (weapon.NumBullets - 1) / 2.0f))) *
                            direction;
                    }
                    else
                    {
                        newDirection = Quaternion.Euler(Vector3.forward * Random.Range(-weapon.Spread, weapon.Spread)) *
                                       direction;
                    }

                    newProjectile.transform.position = firingPoint.position;
                    newProjectile.Speed = weapon.ProjectileSpeed;
                    newProjectile.Direction = newDirection;
                    newProjectile.Damage = weapon.ProjectileDamage;
                    newProjectile.DamageableLayers = damageableLayers;
                    newProjectile.Reset();
                }

                if (weapon.FiringMode == FiringMode.Burst)
                {
                    yield return new WaitForSeconds(weapon.BurstSpeed);
                }
            }
            
            yield return new WaitForSeconds(weapon.ShootingSpeed);
            
            _shooting = false;
        }

        public IEnumerator Reload()
        {
            if (!CanReload)
            {
                yield break;
            }
            
            _reloading = true;
            _shooting = false;
            switch (weapon.ReloadMode)
            {
                case ReloadMode.AllAtOnce:
                    OnBeginReload?.Invoke(weapon.ReloadSpeed, weapon.ReloadMode);
                    yield return new WaitForSeconds(weapon.ReloadSpeed);
                    RemainingAmmo = MaxAmmo;
                    
                    break;
                
                case ReloadMode.OneByOne:
                    while (remainingAmmo < weapon.MaxAmmo)
                    {
                        OnBeginReload?.Invoke(weapon.ReloadSpeed, weapon.ReloadMode);
                        var reloadTimer = 0.0f;
                        while (reloadTimer < weapon.ReloadSpeed)
                        {
                            if (_shooting)
                            {
                                OnReloadInterrupted?.Invoke();
                                _reloading = false;
                                yield break;
                            }
                            
                            yield return new WaitForEndOfFrame();
                            reloadTimer += Time.deltaTime;
                        }
                        
                        RemainingAmmo++;
                    }
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _reloading = false;
        }

        private void ProjectilePoolSetup()
        {
            _projectilePool = new ObjectPool<Projectile>(
                () =>
                {
                    var newProjectile = Instantiate(weapon.Projectile);
                    newProjectile.OnEndOfLife += _projectilePool.Release;
                    newProjectile.gameObject.layer = gameObject.layer;

                    return newProjectile;
                },
                projectile => projectile.gameObject.SetActive(true),
                projectile => projectile.gameObject.SetActive(false),
                projectile =>
                {
                    projectile.OnEndOfLife -= _projectilePool.Release;
                    Destroy(projectile.gameObject);
                },
                true, 1000
            );
        }
    
    }
}
        
