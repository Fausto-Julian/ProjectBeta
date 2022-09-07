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
        
        private readonly int _maxLevel;
        private int _currentLevel;

        private float _currentTime;
        private readonly PlayerModel _player;

        private AbilityState _state;

        public AbilityHolder(Ability ability, PlayerModel model)
        {
            _ability = ability;
            _currentData = ability.LevelsData[0];
            _maxLevel = ability.LevelsData.Length - 1;
            _currentLevel = 0;
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

        public void UpgradeAbility()
        {
            if (_currentLevel >= _maxLevel) 
                return;
            
            _currentLevel++;
            _currentData = _ability.LevelsData[_currentLevel];
        }
    }
}