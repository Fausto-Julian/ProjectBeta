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
        private PlayerModel _player;

        private AbilityState _state;

        public AbilityHolder(Ability ability, PlayerModel model)
        {
            _ability = ability;
            _currentData = ability.Data;
        }

        public void Update()
        {
            if (_state != AbilityState.Cooldown) 
                return;
            
            _currentTime += _player.Runner.DeltaTime;
            
            if (_currentTime >= _currentData.CooldownTime)
            {
                _state = AbilityState.Active;
                _currentTime = 0;
            }
        }
        
        public void Activate()
        {
            if (_state != AbilityState.Active) 
                return;
            
            _ability.Activate(_player, _currentData);
            _state = AbilityState.Cooldown;
        }

        public void UpgradeDamage(float modifier) => _currentData.Damage += modifier;
        public void UpgradeDamageByMultiplier(float multiplier) => _currentData.Damage *= multiplier;
        
        public void UpgradeDamageMagic(float modifier) => _currentData.DamageMagic += modifier;
        public void UpgradeDamageMagicByMultiplier(float multiplier) => _currentData.DamageMagic *= multiplier;
        
        public void UpgradeCooldown(float modifier) => _currentData.CooldownTime -= modifier;
    }
}