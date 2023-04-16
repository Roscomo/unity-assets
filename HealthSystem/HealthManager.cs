using System;
using UnityEngine;

namespace HealthSystem
{
    public class HealthManager : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 10.0f;
        [SerializeField] private float invulnerabilityTime = 0.2f;
    
        private float _health;

    public event Action<float> OnHealthChanged;
    public event Action<float, float> OnHealthPercentageChanged;

        public bool Invulnerable { get; set; }

        public float InvulnerabilityTime => invulnerabilityTime;
        public float MaxAmount => 1.0f;

        public float Health
        {
            get => _health;
         
            set
            {
                if (Invulnerable)
                {
                    return;
                }
            
                _health = value > 0.0f ? value : 0.0f;

                OnHealthChanged?.Invoke(_health);
                OnHealthPercentageChanged?.Invoke(_health, maxHealth);
            }
        }

        public string StatName => "Health";

        private void Awake()
        { 
            _health = maxHealth;
        }

    }
}
