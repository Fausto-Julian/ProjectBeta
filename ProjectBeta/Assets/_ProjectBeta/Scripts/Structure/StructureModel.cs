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
        [SerializeField] private bool isWall;
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
            photonView.RPC(nameof(RPC_DoDamage), photonView.Owner, damage);
            photonView.RPC(nameof(RPC_CreateFloatingInt), doesToDamage, (int)damage);
        }

        [PunRPC]
        private void RPC_DoDamage(float damage)
        {
            _currentHealth -= damage / (1 + defense / 100);

            CheckDestroy();
        }

        private void CheckDestroy()
        {
            if (_currentHealth > 0) 
                return;
            
            OnDestroyStructure?.Invoke();
            photonView.RPC(nameof(RPC_DestroyStructure), RpcTarget.Others);
            PhotonNetwork.Destroy(gameObject);
        }

        [PunRPC]
        private void RPC_DestroyStructure()
        {
            OnDestroyStructure?.Invoke();
        }

        [PunRPC]
        private void RPC_CreateFloatingInt(int textInt)
        {
            var position = isWall ? transform.position + Vector3.down * 3 : transform.position;
            
            var floatingText = Instantiate(floatingTextPrefab, position, Quaternion.identity);
            floatingText.InstanciateInt(position, textInt, Color.red);
        }

    }
}