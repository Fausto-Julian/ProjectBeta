using System;
using _ProjectBeta.Scripts.PlayerScrips;
using UnityEngine;

namespace _ProjectBeta.Scripts.ScriptableObjects.Abilities
{
    public enum AbilityState
    {
        Active,
        Cooldown
    }

    public class AbilityHolder
    {
        private readonly Ability _ability;

        private float _currentTime;
        private readonly PlayerModel _player;

        private AbilityState _state;

        public event Action<float> OnChangeCooldownTime;
        public event Action OnActiveAbility;
        public AbilityHolder(Ability ability, PlayerModel model)
        {
            _ability = ability;
            _player = model;
        }

        /// <summary>
        /// Update the cooldown if the ability´s current state is AbilityState.Cooldown.
        /// </summary>
        public void Update()
        {
          

            if (_state != AbilityState.Cooldown)
                return;

            _currentTime -= Time.deltaTime;

            OnChangeCooldownTime?.Invoke(_currentTime);
            
            if (!(_currentTime < 0)) 
                return;
            
            _state = AbilityState.Active;
            
        }

        /// <summary>
        /// Activate the ability if the current state is AbilityState.Active.
        /// </summary>
        public void Activate()
        {
            if (_state != AbilityState.Active)
                return;

            if (!_ability.TryActivate(_player))
                return;

            _state = AbilityState.Cooldown;
            _currentTime = _ability.CooldownTime;
        }

        public Sprite GetAbilitySprite() => _ability.Sprite;
    }
}