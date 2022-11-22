using System;
using _ProjectBeta.Scripts.Manager;
using _ProjectBeta.Scripts.ScriptableObjects;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectBeta.Scripts.Menu
{
    public class ResultManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private Transform playerResultOne;
        [SerializeField] private Transform playerResultTwo;
        [SerializeField] private PlayerResultController playerResultControllerPrefab;
        [SerializeField] private Button backToMainMenuButton;
        [SerializeField] private Button exitButton;

        private void Awake()
        {
            backToMainMenuButton.onClick.AddListener(OnBackToMainMenuButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);
            
            var players = PhotonNetwork.PlayerList;

            for (var i = 0; i < players.Length; i++)
            {
                var player = players[i];

                var props = player.CustomProperties;

                if (!props.TryGetValue(GameSettings.IsTeamOneId, out var isTeamOne)) 
                    continue;
                
                if (!props.TryGetValue(GameSettings.PlayerPrefabId, out var championSpawn)) 
                    continue;
                
                var spawn = GameManager.Instance.GetPlayerSpawnData((int)championSpawn);
                var content = (bool)isTeamOne ? playerResultOne : playerResultTwo;
                var result = GameManager.Instance.GetPlayerResult(player);

                var playerResultController = Instantiate(playerResultControllerPrefab, content);
                playerResultController.Initialize(spawn.Icon, player.NickName, result);
            }

            var localProps = PhotonNetwork.LocalPlayer.CustomProperties;

            if (localProps.TryGetValue(GameSettings.TeamWonId, out var value))
            {
                if (localProps.TryGetValue(GameSettings.IsTeamOneId, out var isTeamOne))
                {
                    var teamWon = (string)value;
                    var teamName = (bool)isTeamOne ? GameSettings.TeamOneName : GameSettings.TeamTwoName;

                    if (teamWon == teamName)
                    {
                        resultText.text = "Victory";
                        return;
                    }
                }
            }
            
            resultText.text = "Defeat";
        }

        private static void OnBackToMainMenuButtonClicked()
        {
            GameManager.Instance.BackToMainMenu();
        }
        
        private static void OnExitButtonClicked()
        {
            GameManager.Instance.ExitApplication();
        }
    }
}