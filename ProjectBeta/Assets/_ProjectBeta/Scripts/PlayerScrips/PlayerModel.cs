using System;
using _ProjectBeta.Scripts.Classes;
using _ProjectBeta.Scripts.Manager;
using _ProjectBeta.Scripts.PlayerScrips.Interface;
using _ProjectBeta.Scripts.ScriptableObjects.Abilities;
using _ProjectBeta.Scripts.ScriptableObjects.Player;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AI;

namespace _ProjectBeta.Scripts.PlayerScrips
{
    public class PlayerModel : MonoBehaviourPun, IDamageable, IPlayerUIInvoker
    {
        public static PlayerModel Local;

        [SerializeField] private PlayerData data;
        //[SerializeField] private Image healthBar;

        private AbilityHolder _abilityHolderOne;
        private AbilityHolder _abilityHolderTwo;
        private AbilityHolder _abilityHolderThree;

        private Stats _stats;
        private HealthController _healthController;
        private IPlayerController _playerController;
        private NavMeshAgent _agent;
        private float _rotateVelocity;

        private Vector3 _destination;

        private StatisticsController _statisticsController;

        public static event Action<PlayerModel> OnDiePlayer;

        public event Action<Player> OnTakeDamageStatics; 
        public event Action OnDieStatics;

        private static LayerMask PlayerOneLayerMask;
        private static LayerMask PlayerTwoLayerMask;
        private static LayerMask PlayerColOneLayerMask;
        private static LayerMask PlayerColTwoLayerMask;

        private LayerMask _currentPlayerLayerMask;
        private LayerMask _currentProjectileLayerMask;

        public LayerMask GetPlayerLayerMask() => _currentPlayerLayerMask;
        public LayerMask GetProjectileLayerMask() => _currentProjectileLayerMask;
        public StatisticsController GetStatisticsController() => _statisticsController;
        public HealthController GetHealthController() => _healthController;




        public void Awake()
        {
            GameManager.Instance.AddPlayer(this);
            
            _abilityHolderOne = new AbilityHolder(data.AbilityOne, this);
            _abilityHolderTwo = new AbilityHolder(data.AbilityTwo, this);
            _abilityHolderThree = new AbilityHolder(data.AbilityThree, this);

            
            _agent = GetComponent<NavMeshAgent>();
            _stats = new Stats(data);
            _healthController = new HealthController(_stats);
            
            _statisticsController = GetComponent<StatisticsController>();
            _statisticsController.Initialize(this);

            _agent.speed = data.BaseMovementSpeed;

            PlayerOneLayerMask = LayerMask.GetMask("Players One");
            PlayerTwoLayerMask = LayerMask.GetMask("Players Two");
            PlayerColOneLayerMask = LayerMask.GetMask("PlayersCol One");
            PlayerColTwoLayerMask = LayerMask.GetMask("PlayersCol Two");

            //healthBar.fillAmount = 1;

            if (photonView.IsMine)
            {
                _playerController = GetComponent<PlayerController>();
                Local = this;

                if(photonView.Owner.CustomProperties.TryGetValue(GameSettings.IsTeamOneId, out var value))
                {
                    var isTeamOne = (bool)value;

                    int playerLayer = isTeamOne ? PlayerOneLayerMask : PlayerTwoLayerMask;
                    int projectileLayer = isTeamOne ? PlayerColTwoLayerMask : PlayerColOneLayerMask;

                    photonView.RPC(nameof(SetLayers), RpcTarget.All, playerLayer, projectileLayer);
                }
            }

            _healthController.OnDie += HealthControllerOnOnDie;

            SubscribePlayerController();
        }

        [PunRPC]
        private void SetLayers(int playerLayerMask, int projectileLayerMask)
        {
            _currentPlayerLayerMask = playerLayerMask;
            _currentProjectileLayerMask = projectileLayerMask;
        }

        private void HealthControllerOnOnDie()
        {
            OnDieStatics?.Invoke();
            OnDiePlayer?.Invoke(this);
        }

        public void RestoreMaxHealth()
        {
            photonView.RPC(nameof(RPC_RestoreMaxHealth), RpcTarget.All);
        }

        [PunRPC]
        private void RPC_RestoreMaxHealth()
        {
            _healthController.RestoreMaxHealth();
            //healthBar.fillAmount = _healthController.GetCurrentHealth() / _healthController.GetMaxHealth();
        }

        public void Update()
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

        public void UpgradeDefense(float value)
        {
            //RPC_UpgradeDefense(value);
            photonView.RPC(nameof(RPC_UpgradeDefense), RpcTarget.All, value);
        }

        [PunRPC]
        private void RPC_UpgradeDefense(float value)
        {
            _stats.BaseDefense += value;
        }

        public Stats GetStats() => _stats;

        public AbilityHolder GetAbilityHolderOne => _abilityHolderOne;
        public AbilityHolder GetAbilityHolderTwo => _abilityHolderTwo;
        public AbilityHolder GetAbilityHolderThree => _abilityHolderThree;
        
        private void Movement(Vector3 destination)
        {
            _destination = destination;
            _agent.isStopped = false;
            _agent.SetDestination(_destination);
        }

        public void DoDamage(float damage, Player doesTheDamage)
        {
            //RPC_TakeDamage(damage);
            photonView.RPC(nameof(RPC_TakeDamage), RpcTarget.All, damage, doesTheDamage);
        }

        [PunRPC]
        private void RPC_TakeDamage(float damage, Player doesTheDamage)
        {
            if (doesTheDamage != null)
                OnTakeDamageStatics?.Invoke(doesTheDamage);
            _healthController.TakeDamage(damage);
            FloatingTextManager.Instance.CreateFloatingInt(this, (int)damage, Color.yellow);
            //healthBar.fillAmount = _healthController.GetCurrentHealth() / _healthController.GetMaxHealth();
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
            if (_playerController == default)
                return;
            
            _playerController.OnActiveOne += OnActiveOneAbilityHandler;
            _playerController.OnActiveTwo += OnActiveTwoAbilityHandler;
            _playerController.OnActiveThree += OnActiveThreeAbilityHandler;
            _playerController.OnRightClick += Movement;
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