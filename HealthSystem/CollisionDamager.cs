using UnityEngine;

namespace HealthSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class CollisionDamager : Damager
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            var healthSystem = other.GetComponent<HealthManager>();

            if (DamageLayers == (DamageLayers | (1 << other.gameObject.layer)) && healthSystem is not null)
            {
                DealDamage(healthSystem);
            }
        }
    }
}
