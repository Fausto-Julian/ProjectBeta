using System;
using _ProjectBeta.Scripts.PlayerScrips;
using UnityEngine;


namespace _ProjectBeta.Scripts.Classes
{
    public class HealthController
    {
        private readonly Stats _stats;
        private float _currentHealth; //TODO
        private float _healthRegBase;

        public event Action OnDie;
        public event Action<float, float> OnChangeHealth;
        public event Action<float> OnTakeDamage;
        
        public HealthController (Stats stats )
        {
            _stats = stats;
            _currentHealth = _stats.MaxHealth;
            _healthRegBase = 0;
        }

        public void TakeDamage(float damage)
        {
            var mitigationDamage = (damage / (1 + (_stats.defense / 100)));
            _currentHealth -= mitigationDamage;

            OnTakeDamage?.Invoke(damage);
            OnChangeHealth?.Invoke(_stats.maxHealth, _currentHealth);
            
            if (_currentHealth <= 0)
                OnDie?.Invoke();
        }

        public void Heal(float healAmount)
        {
            _currentHealth += healAmount;

            if (_currentHealth > _stats.maxHealth)
            {
                _currentHealth = _stats.maxHealth;
            }
            
            OnChangeHealth?.Invoke(_stats.maxHealth, _currentHealth);
        }

        public void RestoreMaxHealth()
        {
            _currentHealth = _stats.maxHealth;
            OnChangeHealth?.Invoke(_stats.maxHealth, _currentHealth);
        }

        public float GetCurrentHealth() => _currentHealth;

        public void HealthRegen()
        {
            //float regPercent = (_stats.MaxHealth * percentHealth) / 100;             
            if (_currentHealth == _stats.MaxHealth) return;

            if (_currentHealth < _stats.MaxHealth)
            {
                _currentHealth += _healthRegBase; // = 1
                OnChangeHealth?.Invoke(_stats.MaxHealth, _currentHealth);
            }
            else
            {
                _currentHealth = _stats.MaxHealth;
            }
        }

        public void ModifyRegen(float modifiedReg)
        {
            _healthRegBase += modifiedReg;
        }
    }
}
