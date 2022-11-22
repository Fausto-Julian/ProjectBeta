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
                var index = (int)obj;
                var playersData = GameManager.Instance.GetPlayerSpawnData(index);

                var isTeamOne = (bool)teamBlue;

                var position = isTeamOne ? respawnOne.position : respawnTwo.position;

                var layer = isTeamOne ? LayerMask.NameToLayer("Players One") : LayerMask.NameToLayer("Players Two");
                
                var player = PhotonNetworkExtension.Instantiate(playersData.Prefab, position, Quaternion.identity, layer);
                Assert.IsNotNull(player);

                var controller = player.GetComponent<PlayerController>();
                
                Assert.IsNotNull(controller);
                cameraController.SetTarget(controller);

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