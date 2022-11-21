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
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace _ProjectBeta.Scripts.PlayerScrips
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerModel : MonoBehaviourPun, IDamageable, IPlayerUIInvoker
    {
        public static PlayerModel Local;

        public PlayerView PlayerView { get; private set; }

        [SerializeField] private PlayerData data;
        
        private AbilityHolder _abilityHolderOne;
        private AbilityHolder _abilityHolderTwo;
        private AbilityHolder _abilityHolderThree;

        private Stats _stats;
        private HealthController _healthController;
        private Rigidbody _rb;
        private IPlayerController _playerController;
        private NavMeshAgent _agent;
        private float _rotateVelocity;

        private Vector3 _destination;

        private StatisticsController _statisticsController;

        public static event Action<PlayerModel> OnDiePlayer;

        public event Action<Player> OnTakeDamageStatics; 
        public event Action<Player, float> OnTakeDamageUI; 
        public event Action OnDieStatics;

        private float _debugCurrentHealth;
        private float _currentTimeRegen;
        private float _cooldownRegen = 1;

        private static int PlayerOneLayerMask;
        private static int PlayerTwoLayerMask;
        private static int PlayerColOneLayerMask;
        private static int PlayerColTwoLayerMask;  

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
            PlayerView = GetComponent<PlayerView>();
            _rb = GetComponent<Rigidbody>();
            _currentTimeRegen = _cooldownRegen;

            _statisticsController = GetComponent<StatisticsController>();
            _statisticsController.Initialize(this);

            _agent.speed = data.BaseMovementSpeed;

            
            

            if (photonView.IsMine)
            {
                _playerController = GetComponent<PlayerController>();
                var playerHeadUI = GetComponent<PlayerHeadUI>();
                Assert.IsNotNull(playerHeadUI);
                playerHeadUI.Initialize(this);
                
                
                Local = this;
                PlayerOneLayerMask = LayerMask.NameToLayer("Players One");
                PlayerTwoLayerMask = LayerMask.NameToLayer("Players Two");
                PlayerColOneLayerMask = LayerMask.NameToLayer("PlayersCol One");
                PlayerColTwoLayerMask = LayerMask.NameToLayer("PlayersCol Two");

                if (photonView.Owner.CustomProperties.TryGetValue(GameSettings.IsTeamOneId, out var value))
                {
                    var isTeamOne = (bool)value;
                    
                    
                    int playerLayer = isTeamOne ? PlayerOneLayerMask : PlayerTwoLayerMask;
                    int projectileLayer = isTeamOne ? PlayerColOneLayerMask : PlayerColTwoLayerMask;

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
        public LayerMask GetLayers()
        {
           return gameObject.layer;
        }

        [PunRPC]
        private void RPC_RestoreMaxHealth()
        {
            _healthController.RestoreMaxHealth();
        }

        public void Update()
        {
            int layer = GetPlayerLayerMask();
            print(layer);

            _debugCurrentHealth = GetHealthController().GetCurrentHealth();
            _currentTimeRegen -= Time.deltaTime;

            if (_currentTimeRegen <= 0)
            {
                _currentTimeRegen = _cooldownRegen;
                _healthController.HealthRegen();
            }

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
        public void ApplyRegeneration(float value)
        {
            photonView.RPC(nameof(RPC_ApplyRegeneration), RpcTarget.All, value);
        }
        [PunRPC]
        private void RPC_ApplyRegeneration(float value)
        {     
            _healthController.ModifyRegen(value);
        }

        public void Impulse(Vector3 _transform,float value)
        {
            photonView.RPC(nameof(RPC_Impulse), RpcTarget.All,_transform,value);
        }

        [PunRPC]
        private void RPC_Impulse(Vector3 _transform , float value)
        {
            _rb.isKinematic = false;
            _rb.AddForce(_transform * value, ForceMode.Impulse);
          
        }

        public Stats GetStats() => _stats;

        public AbilityHolder GetAbilityHolderOne() => _abilityHolderOne;
        public AbilityHolder GetAbilityHolderTwo() => _abilityHolderTwo;
        public AbilityHolder GetAbilityHolderThree() => _abilityHolderThree;
        
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
            if (doesTheDamage != default)
            {
                OnTakeDamageStatics?.Invoke(doesTheDamage);
                OnTakeDamageUI?.Invoke(doesTheDamage, damage);
            }
            
            _healthController.TakeDamage(damage);
            
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
        private void OnDrawGizmos()
        {
            var mousePos = (Vector3)Mouse.current.position.ReadValue();
            Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out var hit, Mathf.Infinity);

            Gizmos.DrawWireSphere(hit.point, 5f);
            //var mousePos = (Vector3)Mouse.current.position.ReadValue();
            //Ray ray = Camera.current.ScreenPointToRay(mousePos);
            //RaycastHit hit;
            //Physics.Raycast(ray, out hit, Mathf.Infinity);
            //Gizmos.DrawWireSphere(hit.point, 5);
            //Gizmos.color = Color.red;
        }

        public void SetStopped(bool value) => _agent.isStopped = value;
        public void ZeroMovement() => _agent.speed= 0f;
        public void RecoveryMovement() => _agent.speed = data.BaseMovementSpeed;
        public void RbZero() => _rb.isKinematic=true;

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
