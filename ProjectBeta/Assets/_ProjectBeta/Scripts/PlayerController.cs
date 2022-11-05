using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _ProjectBeta.Scripts
{
    public class PlayerController : NetworkBehaviour, IPlayerController, INetworkRunnerCallbacks
    {
        private PlayerModel _model;

        private InputAction _abilityInputAction1;
        private InputAction _abilityInputAction2;
        private InputAction _abilityInputAction3;
        private InputAction _leftClickInputAction;
        private InputAction _rightClickInputAction;
        private InputAction _spaceInputAction;
        
        public event Action OnActiveOne;
        public event Action OnActiveTwo;
        public event Action OnActiveThree;
        public event Action<Vector2> OnRightClick;
        public event Action OnLeftClick;
        public event Action OnSpace;

        private NetworkInputData _networkInputData;
        
        public Vector2 rightClickCommand;
        public bool rightClickActiveCommand;
        public bool activeOneCommand;
        public bool activeTwoCommand;
        public bool activeThreeCommand;
        public bool leftClickCommand;
        public bool spaceCommand;

        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                Runner.AddCallbacks(this);
            }

            PlayerInputGetActions();

            // _inputAsset = GetComponent<PlayerInput>().actions;
            // _playerControls = _inputAsset.FindActionMap("PlayerControls");
        }

        private void PlayerInputGetActions()
        {
            var playerModels = FindObjectsOfType<PlayerModel>();
            PlayerInput playerInput = null;
            foreach (var player in playerModels)
            {
                if (!player.TryGetComponent(out PlayerInput input)) 
                    continue;
                
                if (player.Object.HasInputAuthority)
                {
                    _model = player;
                    playerInput = input;
                    continue;
                }
                
                Destroy(input);
            }

            if (playerInput == null)
            {
                Debug.LogError("Null player Input");
                return;
            }
            
            if (!Object.HasInputAuthority)
                return;
            
            var inputActions = playerInput.actions;

            if (inputActions != null)
            {
                _abilityInputAction1 = inputActions["Ability1"];
                _abilityInputAction2 = inputActions["Ability2"];
                _abilityInputAction3 = inputActions["Ability3"];
                _spaceInputAction = inputActions["CameraLock"];

                _leftClickInputAction = inputActions["LeftClick"];
                _rightClickInputAction = inputActions["RightClick"];
            }

            OnPlayerControllersSubscribe();
        }

        private void OnPlayerControllersSubscribe()
        {
            _abilityInputAction1.performed += AbilityInputAction1Input;
            _abilityInputAction2.performed += AbilityInputAction2Input;
            _abilityInputAction3.performed += AbilityInputAction3Input;

            _leftClickInputAction.performed += LeftClickInputActionInput;
            _rightClickInputAction.performed += RightClickInputActionInput;
            
            _spaceInputAction.performed += SpaceInputAction;
        }

        

        private void OnPlayerControllersUnsubscribe()
        {
            _abilityInputAction1.performed -= AbilityInputAction1Input;
            _abilityInputAction2.performed -= AbilityInputAction2Input;
            _abilityInputAction3.performed -= AbilityInputAction3Input;

            _leftClickInputAction.performed -= LeftClickInputActionInput;
            _rightClickInputAction.performed -= RightClickInputActionInput;
        }
        
        private void SpaceInputAction(InputAction.CallbackContext context)
        {
            spaceCommand = context.ReadValue<float>() > 0.5f;
        }

        private void LeftClickInputActionInput(InputAction.CallbackContext context)
        {
            leftClickCommand = context.ReadValue<float>() > 0.5f;
        }

        private void RightClickInputActionInput(InputAction.CallbackContext context)
        {
            rightClickActiveCommand = context.ReadValue<float>() > 0.5f;
            rightClickCommand = Mouse.current.position.ReadValue();
        }

        private void AbilityInputAction1Input(InputAction.CallbackContext context)
        {
            activeOneCommand = context.ReadValue<float>() > 0.5f;
        }
        private void AbilityInputAction2Input(InputAction.CallbackContext context)
        {
            activeTwoCommand = context.ReadValue<float>() > 0.5f;
        }
        private void AbilityInputAction3Input(InputAction.CallbackContext context)
        {
            activeThreeCommand = context.ReadValue<float>() > 0.5f;
        }
        
        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            _networkInputData.OnActiveOne = activeOneCommand;
            _networkInputData.OnActiveTwo = activeTwoCommand;
            _networkInputData.OnActiveThree = activeThreeCommand;
            _networkInputData.OnRightClick = rightClickCommand;
            _networkInputData.OnLeftClick = leftClickCommand;
            _networkInputData.OnRightClickActive = rightClickActiveCommand;
            _networkInputData.OnSpace = spaceCommand;

            input.Set(_networkInputData);

            activeOneCommand = false;
            activeTwoCommand = false;
            activeThreeCommand = false;
            leftClickCommand = false;
            rightClickActiveCommand = false;
            spaceCommand = false;
        }

        public override void FixedUpdateNetwork()
        {
            if (!GetInput(out NetworkInputData input)) 
                return;

            if (input.OnRightClickActive)
                OnRightClick?.Invoke(input.OnRightClick);
            
            if (input.OnActiveOne)
                OnActiveOne?.Invoke();

            if (input.OnActiveTwo)
                OnActiveTwo?.Invoke();
            
            if (input.OnActiveThree)
                OnActiveTwo?.Invoke();
            
            if (input.OnLeftClick)
                OnActiveTwo?.Invoke();
            
            if (input.OnSpace)
                OnSpace?.Invoke();
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
