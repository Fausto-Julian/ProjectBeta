using System.Collections;
using _ProjectBeta.Scripts.Player;
using UnityEngine;

namespace _ProjectBeta.Scripts
{
    public class Test : MonoBehaviour
    {
        public static Test Instance;

        private void Awake()
        {
            if (Instance != default)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void ActiveRespawn(PlayerModel model)
        {
            StartCoroutine(RespawnCoroutine(model));
        }
        
        private static IEnumerator RespawnCoroutine(PlayerModel model)
        {
            model.gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);
            model.photonView.RPC("RPC_RestoreMaxHealth", Photon.Pun.RpcTarget.All);
            //model.RPC_RestoreMaxHealth();
            model.gameObject.SetActive(true);
        }
    }
}