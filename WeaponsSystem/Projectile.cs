using System;
using System.Collections;
using HealthSystem;
using UnityEngine;

namespace WeaponsSystem
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 1.0f;

        private Rigidbody2D _rigidbody;
        private Damager _damager;

        public Action<Projectile> OnEndOfLife;
    
        public Vector2 Direction { get; set; }

        public float Speed { get; set; }

        public float Damage { get; set; }

        public LayerMask DamageableLayers { get; set; }

        public float LifeTime
        {
            get => lifeTime;
            set => lifeTime = value;
        }
        private void Awake()
        {
            _damager = GetComponent<Damager>();
            _rigidbody = GetComponent<Rigidbody2D>();
        
            Reset();
        }

        public void Reset()
        {
            StartCoroutine(DeathTimer());
            _damager.Damage = Damage;
            _rigidbody.velocity = Direction * Speed;
            transform.right = _rigidbody.velocity.normalized;
        }
    
        private IEnumerator DeathTimer()
        {
            yield return new WaitForSeconds(lifeTime);

            KillProjectile();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var healthSystem = other.GetComponent<HealthManager>();

            if (DamageableLayers == (DamageableLayers | (1 << other.gameObject.layer)) && healthSystem is not null)
            {
                _damager.DealDamage(healthSystem);
                KillProjectile();
            }
        }

        private void KillProjectile()
        {
            if (OnEndOfLife is null)
            {
                Destroy(gameObject);
            }
            else
            {
                if (isActiveAndEnabled)
                {
                    OnEndOfLife.Invoke(this);
                }
            }
        }
    }
}
 