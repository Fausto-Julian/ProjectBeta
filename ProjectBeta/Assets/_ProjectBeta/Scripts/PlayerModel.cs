using _ProjectBeta.Scripts.ScriptableObjects.Ability;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

namespace _ProjectBeta.Scripts
{
    public class PlayerModel : NetworkBehaviour
    {
        //Todo: pasar estas 4 habilidades a playerData
        [SerializeField] private Ability abilityQ;
        [SerializeField] private Ability abilityW;
        [SerializeField] private Ability abilityE;
        [SerializeField] private Ability abilityR;
        [SerializeField] private float rotateSpeed;

        
        private AbilityHolder _abilityHolderQ;
        private AbilityHolder _abilityHolderW;
        private AbilityHolder _abilityHolderE;
        private AbilityHolder _abilityHolderR;

        private IPlayerController _playerController;
        private NavMeshAgent _agent;
        private float _rotateVelocity;

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

        private void Movement(Vector2 mousePos)
        {
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out var hit, Mathf.Infinity)) 
                return;
            
            _agent.SetDestination(hit.point);

            var rotationToLook = Quaternion.LookRotation(hit.point - transform.position);

            var rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLook.eulerAngles.y, ref _rotateVelocity, rotateSpeed * (Time.deltaTime * 5));

            transform.eulerAngles = new Vector3(0, rotationY, 0);
        }

        private void SubscribePlayerController()
        {
            _playerController.OnActiveQ += _abilityHolderQ.Activate;
            _playerController.OnActiveW += _abilityHolderW.Activate;
            _playerController.OnActiveE += _abilityHolderE.Activate;
            _playerController.OnActiveR += _abilityHolderR.Activate;
            _playerController.onRightClick += Movement;

        }
        
        private void UnSubscribePlayerController()
        {
            _playerController.OnActiveQ -= _abilityHolderQ.Activate;
            _playerController.OnActiveW -= _abilityHolderW.Activate;
            _playerController.OnActiveE -= _abilityHolderE.Activate;
            _playerController.OnActiveR -= _abilityHolderR.Activate;
            _playerController.onRightClick -= Movement;

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
