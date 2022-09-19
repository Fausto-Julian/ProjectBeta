using System;
using _ProjectBeta.Scripts.ScriptableObjects.Player;
using UnityEngine;

namespace _ProjectBeta.Scripts.Classes
{
    public class HealthController:MonoBehaviour
    {
        [SerializeField] private PlayerData data;

        private float _maxHp;
        private float _baseMovementSpeed;
        private float _baseDefense;
        private float _magicDefense;
        private float _currentHealth;

        private event Action OnDie;
        private event Action OnTakeDamage;


        private void Awake()
        {
            InitializeHealthController(data);
        }

        

        public void InitializeHealthController( PlayerData data )
        {
            _maxHp = data.MaxHealth;
            _baseMovementSpeed = data.BaseMovementSpeed;
            _baseDefense = data.BaseDefense;
            _currentHealth = _maxHp;
        }

        //ver como manejar el daño magico
        public void TakeDamage(float damage)
        {
            float mitigationDamage = (damage / (1 + (_baseDefense / 100)));
            _currentHealth -= mitigationDamage;

            OnTakeDamage?.Invoke();
            
            if (_currentHealth <= 0)
                OnDie?.Invoke();
        }

        public void Heal(float healAmount)
        {
            _currentHealth += healAmount;

            if (_currentHealth > _maxHp)
            {
                _currentHealth = _maxHp;
            }
        }

        public void RestoreMaxHealth()
        {
            _currentHealth += _maxHp;
        }

        public float GetCurrentHealth() => _currentHealth;
    }
}