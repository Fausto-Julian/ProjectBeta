using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;
using Fusion.Sockets;

public class PlayerController : NetworkBehaviour, INetworkRunnerCallbacks
{
    private PlayerModel model;
    private InputActionAsset inputAsset;
    private InputActionMap playerControls;
    

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Runner.AddCallbacks(this);
        }
        model = GetComponent<PlayerModel>();
        inputAsset = this.GetComponent<PlayerInput>().actions;
        playerControls = inputAsset.FindActionMap("PlayerControls");
    }

    void Update()
    {

    }

    private void OnEnable()
    {
        onPlayerControllersSubscribe();
    }

    private void OnDisable()
    {
        onPlayerControllersUnsubscribe();
    }

    private void onPlayerControllersSubscribe()
    {
        playerControls.FindAction("Ability1").performed += Ability1Input;
        playerControls.FindAction("Ability2").performed += Ability2Input;
        playerControls.FindAction("Ability3").performed += Ability3Input;
        playerControls.FindAction("Ability4").performed += Ability4Input;
        playerControls.FindAction("LeftClick").performed += LeftClickInput;
        playerControls.FindAction("RightClick").performed += RightClickInput;
        playerControls.Enable();
    }
    private void onPlayerControllersUnsubscribe()
    {
        playerControls.FindAction("Ability1").performed -= Ability1Input;
        playerControls.FindAction("Ability2").performed -= Ability2Input;
        playerControls.FindAction("Ability3").performed -= Ability3Input;
        playerControls.FindAction("Ability4").performed -= Ability4Input;
        playerControls.FindAction("LeftClick").performed -= LeftClickInput;
        playerControls.FindAction("RightClick").performed -= RightClickInput;
        playerControls.Disable();
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
        //PlayerMoodel ability1
    }
    private void Ability2Input(InputAction.CallbackContext obj)
    {
        //PlayerMoodel ability2
    }
    private void Ability3Input(InputAction.CallbackContext obj)
    {
        //PlayerMoodel ability3
    }

    private void Ability4Input(InputAction.CallbackContext obj)
    {
        //PlayerModel ability4
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }
}
