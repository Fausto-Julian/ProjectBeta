using System;
using _ProjectBeta.Scripts.Classes;
using _ProjectBeta.Scripts.Player.Interface;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using _ProjectBeta.Scripts.ScriptableObjects.Player;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace _ProjectBeta.Scripts.Player
{
    public class PlayerModel : NetworkBehaviour, IDamageable, IPlayerUIInvoker
    {
        public static PlayerModel Local;

        [SerializeField] private PlayerData data;
        [SerializeField] private Slider healthBar;

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
                
                FindObjectOfType<PlayerUI>()?.Initialized(_healthController, data, this);
            }

            _abilityHolderOne = new AbilityHolder(data.AbilityOne, this);
            _abilityHolderTwo = new AbilityHolder(data.AbilityTwo, this);
            _abilityHolderThree = new AbilityHolder(data.AbilityThree, this);

            _playerController = GetComponent<PlayerController>();
            _agent = GetComponent<NavMeshAgent>();
            _stats = new Stats(data);
            _healthController = new HealthController(_stats);

            _agent.speed = data.BaseMovementSpeed;
            
            healthBar.maxValue = _stats.MaxHealth;
            healthBar.value = _stats.MaxHealth;

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

        public PlayerData GetData() => data;

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void UpgradeDefense(float value)
        {
            _stats.BaseDefense += value;
        }

        public Stats GetStats() => _stats;

        private void Movement(Vector3 destination)
        {
            _destination = destination;
            _agent.isStopped = false;
            _agent.SetDestination(_destination);
        }

        public void DoDamage(float damage)
        {
            RPC_TakeDamage(damage);
        }
        [Rpc(RpcSources.All,RpcTargets.All)]
        private void RPC_TakeDamage(float damage)
        {
            _healthController.TakeDamage(damage);
            healthBar.value = _healthController.GetCurrentHealth();
        }

        private void OnActiveOneAbilityHandler()
        {
            _agent.isStopped = true;
            ActiveAbilityOne?.Invoke(data.AbilityOne.CooldownTime);
            _abilityHolderOne.Activate();
        }
        private void OnActiveTwoAbilityHandler()
        {
            _agent.isStopped = true;
            ActiveAbilityTwo?.Invoke(data.AbilityTwo.CooldownTime);
            _abilityHolderTwo.Activate();
        }
        private void OnActiveThreeAbilityHandler()
        {
            _agent.isStopped = true;
            ActiveAbilityThree?.Invoke(data.AbilityThree.CooldownTime);
            _abilityHolderThree.Activate();
        }

        public void SetStopped(bool value) => _agent.isStopped = value;

        private void SubscribePlayerController()
        {
            _playerController.OnActiveOne += OnActiveOneAbilityHandler;
            _playerController.OnActiveTwo += OnActiveTwoAbilityHandler;
            _playerController.OnActiveThree += OnActiveThreeAbilityHandler;
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
        public event Action<float> ActiveAbilityOne;
        public event Action<float> ActiveAbilityThree;
        public event Action<float> ActiveAbilityTwo;
    }
}
