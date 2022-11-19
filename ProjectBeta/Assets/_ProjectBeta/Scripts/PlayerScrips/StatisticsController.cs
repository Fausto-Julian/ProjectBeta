using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

namespace _ProjectBeta.Scripts.PlayerScrips
{
    public class StatisticsController : MonoBehaviourPun
    {
        [SerializeField] private float timeLifeLastHit;
        [SerializeField] private float timeLifeAssistance;
        
        private Player _owner;
        private Player _lastHit;
        private List<Player> _assistanceList;

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
            _owner = model.photonView.Owner;
            
            model.OnTakeDamageStatics += ModelOnOnTakeDamageStatics;
            model.OnDieStatics += ModelOnOnDieStatics;
        }

        private void ModelOnOnDieStatics()
        {
            for (var i = 0; i < _assistanceList.Count; i++)
            {
                photonView.RPC(nameof(RPC_SendAssistance), _assistanceList[i]);
            }
            
            photonView.RPC(nameof(RPC_SendDeath), _owner);
            
            if (_lastHit != default)
                photonView.RPC(nameof(RPC_SendKill), _lastHit);
        }

        private void ModelOnOnTakeDamageStatics(Player model)
        {
            if (!photonView.IsMine)
                return;
            
            if (Equals(model, _owner))
                return;

            if (_lastHit != default)
                StartCoroutine(AssistanceCoroutine(_lastHit));

            StartCoroutine(LastHitCoroutine(model));
        }

        private IEnumerator AssistanceCoroutine(Player model)
        {
            if (_assistanceList.Contains(model))
                yield break;
            
            _assistanceList.Add(model);

            yield return _waitTimeAssistance;

            if (_assistanceList.Contains(model))
                _assistanceList.Remove(model);
        }
        
        private IEnumerator LastHitCoroutine(Player model)
        {
            _lastHit = model;

            yield return _waitTimeLastHit;

            _lastHit = default;
            _assistanceList.Clear();
        }

        private void RPC_SendKill()
        {
            _killCount++;
            OnKillChange?.Invoke(_killCount);
        }

        private void RPC_SendAssistance()
        {
            _assistanceCount++;
            OnAssistanceChange?.Invoke(_assistanceCount);
        }

        private void RPC_SendDeath()
        {
            _deathCount++;
            OnDeathChange?.Invoke(_deathCount);
        }
    }
}