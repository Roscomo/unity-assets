using UnityEngine;

namespace HealthSystem
{
    public class Damager : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private float areaRadius = 2.5f;
        [SerializeField] private LayerMask damageLayers;
        
        public float AreaRadius => areaRadius;
        protected LayerMask DamageLayers => damageLayers;
        
        public float Damage
        {
            get => damage;
            set => damage = value;
        }
        
        public void DealDamage(HealthManager toDamage)
        {
            if (toDamage.Health > 0)
            {
                toDamage.Health -= Damage;
            }
        }
    }
}
