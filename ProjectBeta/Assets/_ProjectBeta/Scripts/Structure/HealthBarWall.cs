using Photon.Pun;
using UnityEngine;

namespace _ProjectBeta.Scripts.Structure
{
    public class HealthBarWall : MonoBehaviourPun
    {
        [SerializeField] private GameObject healthBar;

        private void Awake()
        {
     
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
            healthBar.transform.localScale = new Vector3(0, currentHealth / maxHealth);
            var posY = (currentHealth / maxHealth) - 1;

            healthBar.transform.position = new Vector3(0, posY);
        }
    }
}