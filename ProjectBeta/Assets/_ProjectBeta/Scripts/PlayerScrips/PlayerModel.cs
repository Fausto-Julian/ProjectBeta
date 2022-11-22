using System;
using System.Collections;
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

#if UNITY_EDITOR
using UnityEngine.InputSystem;
#endif

namespace _ProjectBeta.Scripts.PlayerScrips
{
    public class PlayerModel : MonoBehaviourPun, IDamageable, IPlayerUIInvoker
    {
        [SerializeField] private PlayerData data;

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
        private KillStreakSystem _killStreakSystem;
        private UpgradeController _upgradeController;

        public static event Action<PlayerModel> OnDiePlayer;

        public event Action<Player> OnTakeDamageStatics; 
        public event Action<Player, float> OnTakeDamageUI; 
        public event Action OnDieStatics;

        private static int _playerOneLayerMask;
        private static int _playerTwoLayerMask;
        private static int _playerColOneLayerMask;
        private static int _playerColTwoLayerMask;

        private int _currentPlayerLayerMask;
        private int _currentEnemyLayerMask;
        private int _currentProjectileLayerMask;

        private LayerMask _basicLayerMask;

        private Vector3 _respawn;

        public LayerMask GetBasicLayerMask() => _basicLayerMask;
        public int GetPlayerLayerMask() => _currentPlayerLayerMask;
        public int GetEnemyLayerMask() => _currentEnemyLayerMask;
        public int GetProjectileLayerMask() => _currentProjectileLayerMask;
        public StatisticsController GetStatisticsController() => _statisticsController;
        public KillStreakSystem GetKillStreakSystem() => _killStreakSystem;
        public HealthController GetHealthController() => _healthController;
        public UpgradeController GetUpgradeController() => _upgradeController;

        public Vector3 GetRespawnPosition() => _respawn;

        public void Awake()
        {
            GameManager.Instance.AddPlayer(this);
            
            _abilityHolderOne = new AbilityHolder(data.AbilityOne, this);
            _abilityHolderTwo = new AbilityHolder(data.AbilityTwo, this);
            _abilityHolderThree = new AbilityHolder(data.AbilityThree, this);

            _agent = GetComponent<NavMeshAgent>();
            _stats = new Stats(data);
            _healthController = new HealthController(_stats);

            _upgradeController = GetComponent<UpgradeController>();
            Assert.IsNotNull(_upgradeController);
            _upgradeController.Initialize(_stats);
            
            _statisticsController = GetComponent<StatisticsController>();
            _statisticsController.Initialize(this);

            _agent.speed = _stats.movementSpeed;

            _healthController.OnDie += HealthControllerOnOnDie;

            if (!photonView.IsMine) 
                return;

            _killStreakSystem = GetComponent<KillStreakSystem>();
            Assert.IsNotNull(_killStreakSystem);
            _killStreakSystem.Initialize(this);
                
            _playerController = GetComponent<PlayerController>();
                
            var playerHeadUI = GetComponent<PlayerHeadUI>();
            Assert.IsNotNull(playerHeadUI);
            playerHeadUI.Initialize(this);

            _playerOneLayerMask = LayerMask.NameToLayer("Players One");
            _playerTwoLayerMask = LayerMask.NameToLayer("Players Two");
            _playerColOneLayerMask = LayerMask.NameToLayer("PlayersCol One");
            _playerColTwoLayerMask = LayerMask.NameToLayer("PlayersCol Two");

            if (photonView.Owner.CustomProperties.TryGetValue(GameSettings.IsTeamOneId, out var value))
            {
                var isTeamOne = (bool)value;

                var playerLayer = isTeamOne ? _playerOneLayerMask : _playerTwoLayerMask;
                var enemyLayer = isTeamOne ? _playerTwoLayerMask : _playerOneLayerMask;
                var projectileLayer = isTeamOne ? _playerColOneLayerMask : _playerColTwoLayerMask;
                    
                _basicLayerMask = isTeamOne ? data.ClickRightLayerMaskTeamOne : data.ClickRightLayerMaskTeamTwo;

                photonView.RPC(nameof(SetLayers), RpcTarget.All, playerLayer, enemyLayer, projectileLayer);
            }
                
            SubscribePlayerController();
        }

        public void SetRespawnPosition(Vector3 position)
        {
            photonView.RPC(nameof(RPC_SetRespawnPosition), RpcTarget.All, position);
        }

        [PunRPC]
        private void RPC_SetRespawnPosition(Vector3 position)
        {
            _respawn = position;
        }

        public void ClampCurrentHealth()
        {
            photonView.RPC(nameof(RPC_ClampCurrentHealth), RpcTarget.All);
        }
        
        [PunRPC]
        public void RPC_ClampCurrentHealth()
        {
            _healthController.ClampCurrentHealth();
        }

        public void UpgradeSpeed(float value)
        {
            _upgradeController.UpgradeSpeed(value);
            _agent.speed = _stats.movementSpeed;
        }

        [PunRPC]
        private void SetLayers(int playerLayerMask, int enemyLayer, int projectileLayerMask)
        {
            _currentPlayerLayerMask = playerLayerMask;
            _currentEnemyLayerMask = enemyLayer;
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
            if (Vector3.Distance(_destination, transform.position) < 0.1f)
                _agent.isStopped = true;

            _abilityHolderOne.Update();
            _abilityHolderTwo.Update();
            _abilityHolderThree.Update();
            
            GetHeal(data.HealthForSecond * Time.deltaTime);
        }

        public PlayerData GetData() => data;

        public void GetHeal(float heal)
        {
            photonView.RPC(nameof(RPC_GetHeal), RpcTarget.All, heal);
        }
        
        [PunRPC]
        private void RPC_GetHeal(float heal)
        {
            _healthController.Heal(heal);
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

        public void Impulse(Vector3 value)
        {
            Debug.Log("nono");
            photonView.RPC(nameof(RPC_Impulse), photonView.Owner, value);
        }

        [PunRPC]
        private void RPC_Impulse(Vector3 value)
        {
            StartCoroutine(MovementImpulseTest(value));
            Debug.Log("Hola");
        }

        private IEnumerator MovementImpulseTest(Vector3 value)
        {
            var distance = Vector3.Distance(transform.position, value);
            while (distance > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, value, 50f);
                distance = Vector3.Distance(transform.position, value);
                yield return null;
            }
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

        public void SetStopped(bool value) => _agent.isStopped = value;

        private void SubscribePlayerController()
        {
            _playerController.OnActiveOne += OnActiveOneAbilityHandler;
            _playerController.OnActiveTwo += OnActiveTwoAbilityHandler;
            _playerController.OnActiveThree += OnActiveThreeAbilityHandler;
            _playerController.OnRightClick += Movement;
        }

#if UNITY_EDITOR
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
