using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _ProjectBeta.Scripts
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : NetworkBehaviour, IPlayerController, INetworkRunnerCallbacks
    {
        private PlayerModel _model;
        //private InputActionAsset _inputAsset;
        //private InputActionMap _playerControls;

        private InputAction _ability1;
        private InputAction _ability2;
        private InputAction _ability3;
        private InputAction _leftClick;
        private InputAction _rightClick;
        
        public event Action OnActiveOne;
        public event Action OnActiveTwo;
        public event Action OnActiveThree;
        public event Action<Vector2> onRightClick;
        public event Action onLeftClick;

        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                Runner.AddCallbacks(this);
            }
            _model = GetComponent<PlayerModel>();
            
            //Todo: Networking Get PlayerInput and destroy other.

            PlayerInputGetActions();

            // _inputAsset = GetComponent<PlayerInput>().actions;
            // _playerControls = _inputAsset.FindActionMap("PlayerControls");
        }

        private void PlayerInputGetActions()
        {
            var playerInput = GetComponent<PlayerInput>();
            if (!Object.HasInputAuthority)
            {
                Destroy(playerInput);
                return;
            }
                
            var input = playerInput.actions;

            if (input != null)
            {
                _ability1 = input["Ability1"];
                _ability2 = input["Ability2"];
                _ability3 = input["Ability3"];

                _leftClick = input["LeftClick"];
                _rightClick = input["RightClick"];
            }

            OnPlayerControllersSubscribe();
        }

        private void OnEnable()
        {
            

        }

        private void OnDisable()
        {
            OnPlayerControllersUnsubscribe();
        }
        
        private void OnPlayerControllersSubscribe()
        {
            _ability1.performed += Ability1Input;
            _ability2.performed += Ability2Input;
            _ability3.performed += Ability3Input;

            _leftClick.performed += LeftClickInput;
            _rightClick.performed += RightClickInput;

            
            /*
            _playerControls.FindAction("Ability1").performed += Ability1Input;
            _playerControls.FindAction("Ability2").performed += Ability2Input;
            _playerControls.FindAction("Ability3").performed += Ability3Input;
            _playerControls.FindAction("Ability4").performed += Ability4Input;
            _playerControls.FindAction("LeftClick").performed += LeftClickInput;
            _playerControls.FindAction("RightClick").performed += RightClickInput;
            _playerControls.Enable();*/
        }
        
        private void OnPlayerControllersUnsubscribe()
        {
            _ability1.performed -= Ability1Input;
            _ability2.performed -= Ability2Input;
            _ability3.performed -= Ability3Input;

            _leftClick.performed -= LeftClickInput;
            _rightClick.performed -= RightClickInput;
            /*
            _playerControls.FindAction("Ability1").performed -= Ability1Input;
            _playerControls.FindAction("Ability2").performed -= Ability2Input;
            _playerControls.FindAction("Ability3").performed -= Ability3Input;
            _playerControls.FindAction("Ability4").performed -= Ability4Input;
            _playerControls.FindAction("LeftClick").performed -= LeftClickInput;
            _playerControls.FindAction("RightClick").performed -= RightClickInput;
            _playerControls.Disable();*/
        }

        private void LeftClickInput(InputAction.CallbackContext obj)
        {
            onLeftClick?.Invoke();
        }

        private void RightClickInput(InputAction.CallbackContext obj)
        {
            Debug.Log("click derecho");
            onRightClick?.Invoke(Mouse.current.position.ReadValue());
        }

        private void Ability1Input(InputAction.CallbackContext obj)
        {
            Debug.Log("Q");
            OnActiveOne?.Invoke();
        }
        private void Ability2Input(InputAction.CallbackContext obj)
        {
            OnActiveTwo?.Invoke();
        }
        private void Ability3Input(InputAction.CallbackContext obj)
        {
            OnActiveThree?.Invoke();
        }
        
        //Todo: Implement Input Networking | Testear si funciona sin esto.
        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
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
