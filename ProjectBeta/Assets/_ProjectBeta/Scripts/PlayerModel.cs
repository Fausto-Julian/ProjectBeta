using _ProjectBeta.Scripts.ScriptableObjects.Ability;
using Fusion;
using UnityEngine;

namespace _ProjectBeta.Scripts
{
    public class PlayerModel : NetworkBehaviour
    {
        [SerializeField] private Ability abilityQ;
        [SerializeField] private Ability abilityW;
        [SerializeField] private Ability abilityE;
        [SerializeField] private Ability abilityR;
        
        private AbilityHolder _abilityHolderQ;
        private AbilityHolder _abilityHolderW;
        private AbilityHolder _abilityHolderE;
        private AbilityHolder _abilityHolderR;

        private IPlayerController _playerController;

        public override void Spawned()
        {
            _abilityHolderQ = new AbilityHolder(abilityQ, this);
            _abilityHolderW = new AbilityHolder(abilityW, this);
            _abilityHolderE = new AbilityHolder(abilityE, this);
            _abilityHolderR = new AbilityHolder(abilityR, this);

            _playerController = GetComponent<PlayerController>();
            
            SubscribePlayerController();
        }

        private void OnDisable()
        {
            UnSubscribePlayerController();
        }
        
        
        public override void FixedUpdateNetwork()
        {
            _abilityHolderQ.Update();
            _abilityHolderW.Update();
            _abilityHolderE.Update();
            _abilityHolderR.Update();
        }

        private void SubscribePlayerController()
        {
            _playerController.OnActiveQ += _abilityHolderQ.Activate;
            _playerController.OnActiveW += _abilityHolderW.Activate;
            _playerController.OnActiveE += _abilityHolderE.Activate;
            _playerController.OnActiveR += _abilityHolderR.Activate;
        }
        
        private void UnSubscribePlayerController()
        {
            _playerController.OnActiveQ -= _abilityHolderQ.Activate;
            _playerController.OnActiveW -= _abilityHolderW.Activate;
            _playerController.OnActiveE -= _abilityHolderE.Activate;
            _playerController.OnActiveR -= _abilityHolderR.Activate;
        }

#if UNITY_EDITOR
        [ContextMenu("ActiveQ")]
        private void ActiveQ() => _abilityHolderQ.Activate();
        [ContextMenu("ActiveW")]
        private void ActiveW() => _abilityHolderW.Activate();
        [ContextMenu("ActiveE")]
        private void ActiveE() => _abilityHolderE.Activate();
        [ContextMenu("ActiveR")]
        private void ActiveR() => _abilityHolderR.Activate();
#endif
    }
}
