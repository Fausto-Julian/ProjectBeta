using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectBeta.Scripts.Structure
{
    public class HealthBarStructure : MonoBehaviourPun
    {
        [SerializeField] private Image healthBar;

        private void Awake()
        {
            healthBar.fillAmount = 1;
        }

        public void Initialize(StructureModel model)
        {
            model.OnChangeHealth += OnChangeHealthHandler;
        }

        private void OnChangeHealthHandler(float maxHealth, float currentHealth)
        {
            if(!photonView.IsMine) return;

            photonView.RPC(nameof(RPC_UpdateUI), RpcTarget.All, maxHealth, currentHealth);
        }


        [PunRPC]
        private void RPC_UpdateUI(float maxHealth, float currentHealth)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }
}