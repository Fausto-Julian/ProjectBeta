using System.Collections.Generic;
using _ProjectBeta.Scripts.Classes;
using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _ProjectBeta.Scripts.Manager
{
    [System.Serializable]
    public struct PlayerResult
    {
        public int kills;
        public int assistance;
        public int death;
    }
    
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private List<PlayerSpawnData> playersData;

        private readonly Dictionary<Player, PlayerModel> _playerList = new();
        private readonly Dictionary<Player, PlayerResult> _playersResult = new();

        private bool _gameEnd;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public List<PlayerSpawnData> GetPlayersData() => playersData;

        public PlayerSpawnData GetPlayerSpawnData(int index)
        {
            if (index >= playersData.Count)
            {
                Debug.LogError("Index out of range");
                index = playersData.Count - 1;
            }

            if (index < 0)
                index = 0;

            return playersData[index];
        }

        public void StartGame()
        {
            _gameEnd = false;
        }

        public bool GetGameFinish() => _gameEnd;

        public void GameEnd()
        {
            _gameEnd = true;

            foreach (var model in _playerList.Values)
            {
                _playersResult[model.photonView.Owner] = model.GetStatisticsController().GetResults;
            }
        }

        public PlayerResult GetPlayerResult(Player playerId)
        {
            return _playersResult.TryGetValue(playerId, out var playerResult) ? playerResult : default;
        }

        public void AddPlayer(PlayerModel model)
        {
            _playerList[model.photonView.Owner] = model;
        }

        public void RemovePlayer(PlayerModel model)
        {
            var player = model.photonView.Owner;
            if (_playerList.ContainsKey(player))
            {
                _playerList.Remove(player);
            }
        }

        public PlayerModel GetPlayer(Player playerId)
        {
            return _playerList.TryGetValue(playerId, out var model) ? model : default;
        }

        public bool TryGetPlayer(Player playerId, out PlayerModel model)
        {
            return _playerList.TryGetValue(playerId, out model);
        }

        public void BackToMainMenu()
        {
            PhotonNetwork.LeaveRoom();
            _playerList.Clear();
            _playersResult.Clear();
            SceneManager.LoadScene("MainMenuScene");
        }

        public void ExitApplication()
        {
            if (PhotonNetwork.IsConnected)
                PhotonNetwork.Disconnect();
            
            Application.Quit();
        }
    }
}