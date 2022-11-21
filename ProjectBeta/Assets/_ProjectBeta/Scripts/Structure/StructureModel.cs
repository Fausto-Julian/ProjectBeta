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

        private float _currentHealth;

        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        public void DoDamage(float damage)
        {
            
        }

        [PunRPC]
        private void TakeDamage(float damage)
        {
            
        }
        
    }
}