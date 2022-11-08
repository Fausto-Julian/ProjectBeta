using System;
using _ProjectBeta.Scripts.Player;

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

            _currentTime += _player.Runner.DeltaTime;

            if (!(_currentTime >= _ability.CooldownTime)) 
                return;
            
            _state = AbilityState.Active;
            _currentTime = 0;
        }

        /// <summary>
        /// Activate the ability if the current state is AbilityState.Active.
        /// </summary>
        public void Activate()
        {
            if (_state != AbilityState.Active)
                return;

            _ability.Activate(_player);
            _state = AbilityState.Cooldown;
        }
    }
}