using System;

namespace _ProjectBeta.Scripts.Classes
{
    public class HealthController
    {
        //todo: implement armor and resistance reduce
        private float _maxHealth;
        private float _currentHealth;

        private event Action OnDie;
        private event Action OnTakeDamage;
        
        public HealthController(float maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;

            OnTakeDamage?.Invoke();
            
            if (_currentHealth <= 0)
                OnDie?.Invoke();
        }

        public void Heal(float healAmount)
        {
            _currentHealth += healAmount;

            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
        }

        public void RestoreMaxHealth()
        {
            _currentHealth += _maxHealth;
        }

        public float GetCurrentHealth() => _currentHealth;
    }
}