using System;
using _ProjectBeta.Scripts.ScriptableObjects.Player;
using UnityEngine;

namespace _ProjectBeta.Scripts.Classes
{
    public class HealthController: MonoBehaviour
    {

        private Stats _stats;
        private float _currentHealth;
        private event Action OnDie;
        private event Action OnTakeDamage;
        
        public HealthController ( Stats stats )
        {
            this._stats = stats;
        }

       
        public void TakeDamage(float damage)
        {
            Debug.Log("recibi daño");
            float mitigationDamage = (damage / (1 + (_stats.BaseDefense / 100)));
            _currentHealth -= mitigationDamage;

            OnTakeDamage?.Invoke();
            
            if (_currentHealth <= 0)
                OnDie?.Invoke();
        }

        public void Heal(float healAmount)
        {
            _currentHealth += healAmount;

            if (_currentHealth > _stats.MaxHealth)
            {
                _currentHealth = _stats.MaxHealth;
            }
        }

        public void RestoreMaxHealth()
        {
            _currentHealth += _stats.MaxHealth;
        }

        public float GetCurrentHealth() => _currentHealth;
    }
}