using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine.InputSystem;

namespace _ProjectBeta.Scripts
{
    public class PlayerController : NetworkBehaviour, IPlayerController, INetworkRunnerCallbacks
    {
        private PlayerModel _model;
        private InputActionAsset _inputAsset;
        private InputActionMap _playerControls;
        
        public event Action OnActiveQ;
        public event Action OnActiveW;
        public event Action OnActiveE;
        public event Action OnActiveR;

        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                Runner.AddCallbacks(this);
            }
            _model = GetComponent<PlayerModel>();
            _inputAsset = this.GetComponent<PlayerInput>().actions;
            _playerControls = _inputAsset.FindActionMap("PlayerControls");
        }

        private void OnEnable()
        {
            OnPlayerControllersSubscribe();
        }

        private void OnDisable()
        {
            OnPlayerControllersUnsubscribe();
        }

        private void OnPlayerControllersSubscribe()
        {
            _playerControls.FindAction("Ability1").performed += Ability1Input;
            _playerControls.FindAction("Ability2").performed += Ability2Input;
            _playerControls.FindAction("Ability3").performed += Ability3Input;
            _playerControls.FindAction("Ability4").performed += Ability4Input;
            _playerControls.FindAction("LeftClick").performed += LeftClickInput;
            _playerControls.FindAction("RightClick").performed += RightClickInput;
            _playerControls.Enable();
        }
        private void OnPlayerControllersUnsubscribe()
        {
            _playerControls.FindAction("Ability1").performed -= Ability1Input;
            _playerControls.FindAction("Ability2").performed -= Ability2Input;
            _playerControls.FindAction("Ability3").performed -= Ability3Input;
            _playerControls.FindAction("Ability4").performed -= Ability4Input;
            _playerControls.FindAction("LeftClick").performed -= LeftClickInput;
            _playerControls.FindAction("RightClick").performed -= RightClickInput;
            _playerControls.Disable();
        }

        private void LeftClickInput(InputAction.CallbackContext obj)
        {
            //PlayerModel LeftClick function 
        }

        private void RightClickInput(InputAction.CallbackContext obj)
        {
            //PlayerModel RightClick function
        }

        private void Ability1Input(InputAction.CallbackContext obj)
        {
            OnActiveQ?.Invoke();
        }
        private void Ability2Input(InputAction.CallbackContext obj)
        {
            OnActiveW?.Invoke();
        }
        private void Ability3Input(InputAction.CallbackContext obj)
        {
            OnActiveE?.Invoke();
        }

        private void Ability4Input(InputAction.CallbackContext obj)
        {
            OnActiveR?.Invoke();
        }
    
        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            throw new NotImplementedException();
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player){ }
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player){ }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){ }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason){ }
        public void OnConnectedToServer(NetworkRunner runner){ }
        public void OnDisconnectedFromServer(NetworkRunner runner){ }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token){ }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){ }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){ }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){ }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){ }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){ }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data){ }
        public void OnSceneLoadDone(NetworkRunner runner){ }
        public void OnSceneLoadStart(NetworkRunner runner) { }
    }
}
