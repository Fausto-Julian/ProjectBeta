using System;
using _ProjectBeta.Scripts.Classes;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using _ProjectBeta.Scripts.ScriptableObjects.Player;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

namespace _ProjectBeta.Scripts
{
    public class PlayerModel : NetworkBehaviour, IDamageable
    {
        [SerializeField] private PlayerData data;
        
        //Todo: pasar estas 4 habilidades a playerData

        private AbilityHolder _abilityHolderOne;
        private AbilityHolder _abilityHolderTwo;
        private AbilityHolder _abilityHolderThree;

        private IPlayerController _playerController;

        private HealthController _healthController;

        public override void Spawned()
        {
            _abilityHolderOne = new AbilityHolder(data.AbilityOne, this);
            _abilityHolderTwo = new AbilityHolder(data.AbilityTwo, this);
            _abilityHolderThree = new AbilityHolder(data.AbilityThree, this);

            _playerController = GetComponent<PlayerController>();
            
            SubscribePlayerController();
            
            _healthController = new HealthController(data.MaxHealth);

        }

        private void OnDisable()
        {
            UnSubscribePlayerController();
        }
        
        
        public override void FixedUpdateNetwork()
        {
            _abilityHolderOne.Update();
            _abilityHolderTwo.Update();
            _abilityHolderThree.Update();
        }

        private void SubscribePlayerController()
        {
            _playerController.OnActiveQ += _abilityHolderOne.Activate;
            _playerController.OnActiveW += _abilityHolderTwo.Activate;
            _playerController.OnActiveE += _abilityHolderThree.Activate;
        }
        
        private void UnSubscribePlayerController()
        {
            _playerController.OnActiveQ -= _abilityHolderOne.Activate;
            _playerController.OnActiveW -= _abilityHolderTwo.Activate;
            _playerController.OnActiveE -= _abilityHolderThree.Activate;
        }
        
        public void DoDamage(float damage)
        {
            _healthController.TakeDamage(damage);
        }

#if UNITY_EDITOR
        [ContextMenu("ActiveQ")]
        private void ActiveQ() => _abilityHolderOne.Activate();
        [ContextMenu("ActiveW")]
        private void ActiveW() => _abilityHolderTwo.Activate();
        [ContextMenu("ActiveE")]
        private void ActiveE() => _abilityHolderThree.Activate();
#endif
        
    }
}
