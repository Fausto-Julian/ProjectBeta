using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using _ProjectBeta.Scripts.ScriptableObjects.Player;
using _ProjectBeta.Scripts.Classes;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

namespace _ProjectBeta.Scripts
{
    public class PlayerModel : NetworkBehaviour
    {
        public static PlayerModel Local;
        
        [SerializeField] private PlayerData data;
        [SerializeField] private float rotateSpeed;

        private AbilityHolder _abilityHolderOne;
        private AbilityHolder _abilityHolderTwo;
        private AbilityHolder _abilityHolderThree;

        private IPlayerController _playerController;
        private NavMeshAgent _agent;
        private float _rotateVelocity;

        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                Local = this;
            }

            _abilityHolderOne = new AbilityHolder(data.AbilityOne, this);
            _abilityHolderTwo = new AbilityHolder(data.AbilityTwo, this);
            _abilityHolderThree = new AbilityHolder(data.AbilityThree, this);

            _playerController = GetComponent<PlayerController>();
            _agent = GetComponent<NavMeshAgent>();
           
            
            
            SubscribePlayerController();
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
            _playerController.OnActiveOne += _abilityHolderOne.Activate;
            _playerController.OnActiveTwo += _abilityHolderTwo.Activate;
            _playerController.OnActiveThree += _abilityHolderThree.Activate;
            _playerController.OnRightClick += Movement;
        }
        
        private void UnSubscribePlayerController()
        {
            _playerController.OnActiveOne -= _abilityHolderOne.Activate;
            _playerController.OnActiveTwo -= _abilityHolderTwo.Activate;
            _playerController.OnActiveThree -= _abilityHolderThree.Activate;
            _playerController.OnRightClick -= Movement;
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
