using System;
using System.Collections;
using System.Collections.Generic;
using _ProjectBeta.Scripts.Manager;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.Assertions;

namespace _ProjectBeta.Scripts.PlayerScrips
{
    public class StatisticsController : MonoBehaviourPun
    {
        [SerializeField] private float timeLifeLastHit;
        [SerializeField] private float timeLifeAssistance;

        private StatisticsController _lastHit;
        private readonly List<StatisticsController> _assistanceList = new List<StatisticsController>();

        private WaitForSeconds _waitTimeLastHit;
        private WaitForSeconds _waitTimeAssistance;

        private int _killCount;
        private int _assistanceCount;
        private int _deathCount;

        public event Action<int> OnKillChange; 
        public event Action<int> OnAssistanceChange; 
        public event Action<int> OnDeathChange; 

        private void Awake()
        {
            _waitTimeLastHit = new WaitForSeconds(timeLifeLastHit);
            _waitTimeAssistance = new WaitForSeconds(timeLifeAssistance);
        }

        public void Initialize(PlayerModel model)
        {
            model.OnTakeDamageStatics += ModelOnOnTakeDamageStatics;
            model.OnDieStatics += OnDieStatics;
        }

        private void OnDieStatics()
        {
            if (!photonView.IsMine)
                return;
            
            for (var i = 0; i < _assistanceList.Count; i++)
            {
                _assistanceList[i].SendAssistance();
            }

            SendDeath();

            if (_lastHit == default) 
                return;
            
            _lastHit.SendKill();
        }

        private void ModelOnOnTakeDamageStatics(Player playerId)
        {
            if (!photonView.IsMine)
                return;
            
            if (Equals(playerId, photonView.Owner))
                return;
            
            if (!GameManager.Instance.TryGetPlayer(playerId, out var model))
                return;

            var playerStatics = model.GetStatisticsController();
            Assert.IsNotNull(playerStatics);
            
            if (_lastHit != default)
                if (_lastHit != playerStatics)
                    StartCoroutine(AssistanceCoroutine(_lastHit));

            StartCoroutine(LastHitCoroutine(playerStatics));
        }

        private IEnumerator AssistanceCoroutine(StatisticsController model)
        {
            if (_assistanceList.Contains(model))
                yield break;
            
            _assistanceList.Add(model);

            yield return _waitTimeAssistance;

            if (_assistanceList.Contains(model))
                _assistanceList.Remove(model);
        }
        
        private IEnumerator LastHitCoroutine(StatisticsController player)
        {
            _lastHit = player;

            yield return _waitTimeLastHit;

            _lastHit = default;
            _assistanceList.Clear();
        }

        public void SendKill()
        {
            photonView.RPC(nameof(RPC_SendKill), RpcTarget.All);
        }
        
        public void SendAssistance()
        {
            photonView.RPC(nameof(RPC_SendAssistance), RpcTarget.All);
        }
        
        private void SendDeath()
        {
            photonView.RPC(nameof(RPC_SendDeath), RpcTarget.All);
        }
        
        [PunRPC]
        private void RPC_SendKill()
        {
            _killCount++;
            OnKillChange?.Invoke(_killCount);
        }
        
        [PunRPC]
        private void RPC_SendAssistance()
        {
            _assistanceCount++;
            OnAssistanceChange?.Invoke(_assistanceCount);
        }
        
        [PunRPC]
        private void RPC_SendDeath()
        {
            _deathCount++;
            OnDeathChange?.Invoke(_deathCount);
        }
    }
}