using System.Collections.Generic;
using _ProjectBeta.Scripts.Manager;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _ProjectBeta.Scripts.Menu
{
    public class RoomsManager : MonoBehaviourPunCallbacks
    {
        [Header("MainMenu")]
        [SerializeField] private GameObject main;
        [SerializeField] private TMP_InputField roomName;
        [SerializeField] private Button joinButton;
        
        [Header("Room")]
        [SerializeField] private GameObject lobby;
        [SerializeField] private Transform playerListLeft;
        [SerializeField] private Transform playerListRight;
        [SerializeField] private Transform championsContent;
        [SerializeField] private Button leaveRoomButton;
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button readyButton;

        [Header("RoomSetting")]
        [SerializeField] private byte maxPlayers;
        [SerializeField] private PlayerListController playerListPrefab;
        [SerializeField] private ChampionsButtonController championsButtonPrefab;

        [Header("StartGameSetting")] 
        [SerializeField] private string sceneToLoad;

        private readonly Dictionary<int, PlayerListController> _playerList = new();
        private readonly List<ChampionsButtonController> _championsCache = new();

        private void Awake()
        {
            startGameButton.onClick.AddListener(OnStartGameButtonClicked);
            leaveRoomButton.onClick.AddListener(OnLeaveGameButtonClicked);
            joinButton.onClick.AddListener(OnJoinRoomButtonClicked);
            readyButton.onClick.AddListener(OnReadyButtonClicked);
        }

        #region Rooms

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            SetActivePanel(main.name);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            SetActivePanel(main.name);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            var roomName = "Room " + Random.Range(0, 10000);

            var props = new Hashtable();

            var options = new RoomOptions {MaxPlayers = maxPlayers, CustomRoomProperties = props};

            PhotonNetwork.CreateRoom(roomName, options);
        }

        public override void OnJoinedRoom()
        {
            SetActivePanel(lobby.name);

            var team = true;

            // Instance player in current room and set active if is player ready
            for (var i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                var player = PhotonNetwork.PlayerList[i];

                var props = player.CustomProperties;
                props[GameSettings.IsTeamOneId] = team;

                var entry = Instantiate(playerListPrefab, team ? playerListLeft : playerListRight, true);
                entry.transform.localScale = Vector3.one;
                
                entry.Initialize(player.NickName);

                if (props.TryGetValue(GameSettings.ReadyId, out var isPlayerReady))
                    entry.SetPlayerReady((bool)isPlayerReady);
                
                if (player.CustomProperties.TryGetValue(GameSettings.PlayerPrefabId, out var index))
                    entry.SetIcon((int)index);

                player.SetCustomProperties(props);
                _playerList.Add(player.ActorNumber, entry);
                team = !team;
            }

            for (var i = 0; i < GameManager.Instance.GetPlayersData().Count; i++)
            {
                var champion = Instantiate(championsButtonPrefab, championsContent);
                champion.Initialize(i);
                _championsCache.Add(champion);
            }

            startGameButton.interactable = CheckPlayersReady();
        }

        public override void OnLeftRoom()
        {
            SetActivePanel(main.name);

            foreach (var entry in _playerList.Values)
            {
                Destroy(entry.gameObject);
            }
            
            foreach (var entry in _championsCache)
            {
                Destroy(entry.gameObject);
            }
        
            _playerList.Clear();
            _championsCache.Clear();
        }

        #endregion

        #region Insider Room

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            var team = (bool)newPlayer.CustomProperties[GameSettings.IsTeamOneId];
            
            var entry = Instantiate(playerListPrefab,team ? playerListLeft : playerListRight, true);
            entry.transform.localScale = Vector3.one;
            
            entry.Initialize(newPlayer.NickName);
            
            if (newPlayer.CustomProperties.TryGetValue(GameSettings.PlayerPrefabId, out var index))
                entry.SetIcon((int)index);

            _playerList.Add(newPlayer.ActorNumber, entry);

            startGameButton.interactable = CheckPlayersReady();
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            Destroy(_playerList[otherPlayer.ActorNumber].gameObject);
        
            _playerList.Remove(otherPlayer.ActorNumber);

            startGameButton.interactable = CheckPlayersReady();
        }
    
        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            startGameButton.interactable = CheckPlayersReady();
        }

        public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
        {
            if (_playerList.TryGetValue(targetPlayer.ActorNumber, out var entry))
            {
                if (changedProps.TryGetValue(GameSettings.ReadyId, out var isPlayerReady))
                    entry.SetPlayerReady((bool) isPlayerReady);
                
                if (changedProps.TryGetValue(GameSettings.IsTeamOneId, out var team))
                    entry.transform.SetParent(((bool)team) ? playerListLeft : playerListRight, false);
                
                if (changedProps.TryGetValue(GameSettings.PlayerPrefabId, out var index))
                    entry.SetIcon((int)index);
            }

            startGameButton.interactable = CheckPlayersReady();
        }

        #endregion

        #region Buttons

        private void OnStartGameButtonClicked()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            PhotonNetwork.LoadLevel(sceneToLoad);
            GameManager.Instance.StartGame();
        }
    
        private static void OnLeaveGameButtonClicked()
        {
            PhotonNetwork.LeaveRoom();
        }

        private void OnJoinRoomButtonClicked()
        {
            var props = new Hashtable()
            {
                { GameSettings.ReadyId, false },
                { GameSettings.IsTeamOneId, true },
                { GameSettings.PlayerPrefabId, 0 }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            var options = new RoomOptions()
            {
                MaxPlayers = maxPlayers
            };

            PhotonNetwork.JoinOrCreateRoom(roomName.text, options, TypedLobby.Default);
        }

        private static void OnReadyButtonClicked()
        {
            var props = PhotonNetwork.LocalPlayer.CustomProperties;
            props[GameSettings.ReadyId] = !(bool)props[GameSettings.ReadyId];
            
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        #endregion

        private void SetActivePanel(string activePanel)
        {
            main.SetActive(activePanel.Equals(main.name));
            lobby.SetActive(activePanel.Equals(lobby.name));
        }
    
        private static bool CheckPlayersReady()
        {
            if (!PhotonNetwork.IsMasterClient)
                return false;

            for (var i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                var player = PhotonNetwork.PlayerList[i];
                if (player.CustomProperties.TryGetValue(GameSettings.ReadyId, out var isPlayerReady))
                {
                    if (!(bool)isPlayerReady)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}