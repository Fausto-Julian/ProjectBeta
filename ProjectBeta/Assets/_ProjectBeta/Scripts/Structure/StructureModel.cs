using System;
using _ProjectBeta.Scripts.Classes;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace _ProjectBeta.Scripts.Structure
{
    public class StructureModel : MonoBehaviourPun
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float defense;
        [SerializeField] private bool isAcceptProjectileDamage;
        [SerializeField] private FloatingText floatingTextPrefab;

        private float _currentHealth;
        public Action OnDestroyStructure;

        public bool GetIsAcceptProjectileDamage() => isAcceptProjectileDamage;

        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        public void DoDamage(float damage, Player doesToDamage)
        {
            photonView.RPC(nameof(RPC_TakeDamage), RpcTarget.All, damage, doesToDamage);
        }

        [PunRPC]
        private void RPC_TakeDamage(float damage, Player doesToDamage)
        {
            _currentHealth -= damage / (1 + defense / 100);
            photonView.RPC(nameof(RPC_CreateFloatingInt), doesToDamage, (int)damage);

            if (!(_currentHealth <= 0) || !photonView.IsMine) 
                return;
            
            photonView.RPC(nameof(RPC_DestroyStructure), RpcTarget.All);
        }

        private void RPC_DestroyStructure()
        {
            OnDestroyStructure?.Invoke();
            Destroy(gameObject);
        }

        [PunRPC]
        private void RPC_CreateFloatingInt(int textInt)
        {
            var position = transform.position;
            
            var floatingText = Instantiate(floatingTextPrefab, position, Quaternion.identity);
            floatingText.InstanciateInt(position, textInt, Color.red);
        }

    }
}