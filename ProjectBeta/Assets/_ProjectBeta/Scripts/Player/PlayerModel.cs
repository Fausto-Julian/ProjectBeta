using _ProjectBeta.Scripts.Classes;
using _ProjectBeta.Scripts.Player.Interface;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using _ProjectBeta.Scripts.ScriptableObjects.Player;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

namespace _ProjectBeta.Scripts.Player
{
    public class PlayerModel : NetworkBehaviour
    {
        public static PlayerModel Local;
        
        [SerializeField] private PlayerData data;
        [SerializeField] private float rotateSpeed;
        [SerializeField] private Transform view;

        private AbilityHolder _abilityHolderOne;
        private AbilityHolder _abilityHolderTwo;
        private AbilityHolder _abilityHolderThree;

        private Stats _stats;
        private HealthController _healthController;
        private IPlayerController _playerController;
        private NavMeshAgent _agent;
        private float _rotateVelocity;
        
        private Vector3 _destination;

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
            _stats = new Stats(data);
            _healthController = new HealthController(_stats);

            SubscribePlayerController();
        }

        public override void FixedUpdateNetwork()
        {
            if (Vector3.Distance(_destination, transform.position) < 0.05f)
            {
                _agent.isStopped = true;
            }
            
            _abilityHolderOne.Update();
            _abilityHolderTwo.Update();
            _abilityHolderThree.Update();
        }

        private void Movement(Vector3 destination)
        {
            _destination = destination;
            _agent.isStopped = false;
            _agent.SetDestination(_destination);

            var rotationToLook = Quaternion.LookRotation(destination - transform.position);

            var rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLook.eulerAngles.y, ref _rotateVelocity, rotateSpeed * (Time.deltaTime * 5));

            view.eulerAngles = new Vector3(0, rotationY, 0);
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
