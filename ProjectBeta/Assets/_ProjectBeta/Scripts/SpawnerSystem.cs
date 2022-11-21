using System.Collections;
using _ProjectBeta.Scripts.Extension;
using _ProjectBeta.Scripts.Manager;
using _ProjectBeta.Scripts.PlayerScrips;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Assertions;

namespace _ProjectBeta.Scripts
{
    public class SpawnerSystem : MonoBehaviour
    {
        [SerializeField] private Transform respawnOne;
        [SerializeField] private Transform respawnTwo;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerUI playerUI;

        private void Awake()
        {
            var props = PhotonNetwork.LocalPlayer.CustomProperties;

            if (props.TryGetValue(GameSettings.PlayerPrefabId, out var obj) && 
                props.TryGetValue(GameSettings.IsTeamOneId, out var teamBlue))
            {
                var playersData = GameManager.Instance.GetPlayersData();
                var index = (int)obj;
                
                if (index >= playersData.Count)
                    index = playersData.Count - 1;

                var position = (bool)teamBlue ? respawnOne.position : respawnTwo.position;

                var player = PhotonNetworkExtension.Instantiate<PlayerModel>(playersData[index].Prefab, position, Quaternion.identity);

                var controller = player.GetComponent<PlayerController>();
                
                Assert.IsNotNull(player);
                cameraController.SetTarget(controller);

                Assert.IsNotNull(player);
                playerUI.Initialized(player);
            }

            PlayerModel.OnDiePlayer += ActiveRespawn;
        }

        private void OnDisable()
        {
            PlayerModel.OnDiePlayer -= ActiveRespawn;
        }

        private void ActiveRespawn(PlayerModel model)
        {
            StartCoroutine(RespawnCoroutine(model));
        }
        
        private static IEnumerator RespawnCoroutine(PlayerModel model)
        {
            model.gameObject.SetActive(false);
            yield return new WaitForSeconds(model.GetData().RespawnCooldown);
            model.RestoreMaxHealth();
            model.gameObject.SetActive(true);
        }
    }
}