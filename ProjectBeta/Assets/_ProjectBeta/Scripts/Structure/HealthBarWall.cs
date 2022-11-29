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
            Debug.Log(currentHealth / maxHealth);
            healthBar.transform.localScale = new Vector3(1, currentHealth / maxHealth,1);
            /*
            var posY = currentHealth / maxHealth;

            var healthBarPos = healthBar.transform.position;
            var newPos = new Vector3(healthBarPos.x, posY,  healthBarPos.z);
            healthBar.transform.position = newPos;
            */
        }
    }
}