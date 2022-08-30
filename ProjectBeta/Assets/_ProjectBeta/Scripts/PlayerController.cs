using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerModel model;
    private InputActionAsset inputAsset;
    private InputActionMap playerControls;

    private void Awake()
    {
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




}
