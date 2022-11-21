using System;
using _ProjectBeta.Scripts.Classes;
using Photon.Pun;
using UnityEngine;

namespace _ProjectBeta.Scripts.Structure
{
    public class StructureModel : MonoBehaviourPun
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float defense;
        [SerializeField] private bool isAcceptProjectileDamage;

        private float _currentHealth;
        public Action OnDestroyStructure;

        public bool GetIsAcceptProjectileDamage() => isAcceptProjectileDamage;

        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        public void DoDamage(float damage)
        {
            photonView.RPC(nameof(RPC_TakeDamage), RpcTarget.All, damage);
        }

        [PunRPC]
        private void RPC_TakeDamage(float damage)
        {
            _currentHealth -= damage / (1 + defense / 100);

            if (!(_currentHealth <= 0) || !photonView.IsMine) 
                return;
            
            photonView.RPC(nameof(RPC_DestroyStructure), RpcTarget.All);
        }

        private void RPC_DestroyStructure()
        {
            OnDestroyStructure?.Invoke();
            Destroy(gameObject);
        }

    }
}