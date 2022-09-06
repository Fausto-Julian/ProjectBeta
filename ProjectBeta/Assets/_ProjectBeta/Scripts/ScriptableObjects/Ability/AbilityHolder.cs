using UnityEngine;

namespace _ProjectBeta.Scripts.ScriptableObjects.Ability
{
    public enum AbilityState
    {
        Active,
        Cooldown
    }
    
    public class AbilityHolder
    {
        private readonly Ability _ability;
        private AbilityData _currentData;

        private float _currentTime;
        private readonly PlayerModel _player;

        private AbilityState _state;

        public AbilityHolder(Ability ability, PlayerModel model)
        {
            _ability = ability;
            _currentData = ability.Data;
            _player = model;
        }
        
        /// <summary>
        /// Update the cooldown if the ability´s current state is AbilityState.Cooldown.
        /// </summary>
        public void Update()
        {
            if (_state != AbilityState.Cooldown) 
                return;
            
            _currentTime += _player.Runner.DeltaTime;
            
            if (_currentTime >= _currentData.cooldownTime)
            {
                _state = AbilityState.Active;
                _currentTime = 0;
            }
        }
        
        /// <summary>
        /// Activate the ability if the current state is AbilityState.Active.
        /// </summary>
        public void Activate()
        {
            if (_state != AbilityState.Active) 
                return;
            
            _ability.Activate(_player, _currentData);
            _state = AbilityState.Cooldown;
        }
        
        /// <summary>
        /// Upgrade damage data
        /// </summary>
        /// <param name="modifier">Value to add</param>
        public void UpgradeDamage(float modifier) => _currentData.damage += modifier;
        
        /// <summary>
        /// Upgrade damage data by multiplier 
        /// </summary>
        /// <param name="multiplier">value to multiply</param>
        public void UpgradeDamageByMultiplier(float multiplier) => _currentData.damage *= multiplier;
        
        /// <summary>
        /// Upgrade damageMagic data
        /// </summary>
        /// <param name="modifier">Value to add</param>
        public void UpgradeDamageMagic(float modifier) => _currentData.damageMagic += modifier;
        
        /// <summary>
        /// Upgrade damageMagic data by multiplier 
        /// </summary>
        /// <param name="multiplier">value to multiply</param>
        public void UpgradeDamageMagicByMultiplier(float multiplier) => _currentData.damageMagic *= multiplier;
        
        /// <summary>
        /// Upgrade damageMagic data
        /// </summary>
        /// <param name="modifier">Value to remove cooldown</param>
        public void UpgradeCooldown(float modifier) => _currentData.cooldownTime -= modifier;
    }
}