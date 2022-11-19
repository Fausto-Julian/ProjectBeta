using System.Collections.Generic;
using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.ScriptableObjects;
using Photon.Realtime;
using UnityEngine;

namespace _ProjectBeta.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private List<PlayerSpawnData> playersData;

        private Dictionary<Player, PlayerModel> _playerList = new Dictionary<Player, PlayerModel>();

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

    }
}