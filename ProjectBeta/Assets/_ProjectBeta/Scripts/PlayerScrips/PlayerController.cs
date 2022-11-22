using System;
using _ProjectBeta.Scripts.PlayerScrips.Interface;
using _ProjectBeta.Scripts.Structure;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _ProjectBeta.Scripts.PlayerScrips
{
    public class PlayerController : MonoBehaviourPun, IPlayerController
    {
        private InputAction _abilityInputAction1;
        private InputAction _abilityInputAction2;
        private InputAction _abilityInputAction3;
        private InputAction _leftClickInputAction;
        private InputAction _rightClickInputAction;
        private InputAction _spaceInputAction;
        
        public event Action OnActiveOne;
        public event Action OnActiveTwo;
        public event Action OnActiveThree;
        public event Action<Vector3> OnRightClick;
        public event Action OnLeftClick;
        public event Action OnSpace;

        private PlayerModel _model;

        private void Awake()
        {
            if(photonView.IsMine)
            {
                PlayerInputGetActions();
            }
        }

        //public void Spawned()
        //{
            //if (true)Object.HasInputAuthority)
            //{
                //Runner.AddCallbacks(this);
            //}

            //PlayerInputGetActions();

            // _inputAsset = GetComponent<PlayerInput>().actions;
            // _playerControls = _inputAsset.FindActionMap("PlayerControls");
        //}

        private void PlayerInputGetActions()
        {

            //var playerModels = FindObjectsOfType<PlayerModel>();
            //PlayerInput playerInput = null;
            //foreach (var player in playerModels)
            //{
            //    if (!player.TryGetComponent(out PlayerInput input))
            //        continue;

            //    if (player.Object.HasInputAuthority)
            //    {
            //        _model = player;
            //        playerInput = input;
            //        continue;
            //    }

            //    Destroy(input);
            //}

            //if (playerInput == null)
            //{
            //    Debug.LogError("Null player Input");
            //    return;
            //}

            //if (!Object.HasInputAuthority)
            //    return;

            //var inputActions = playerInput.actions;

            //if (inputActions != null)
            //{
            //    _abilityInputAction1 = inputActions["Ability1"];
            //    _abilityInputAction2 = inputActions["Ability2"];
            //    _abilityInputAction3 = inputActions["Ability3"];
            //    _spaceInputAction = inputActions["CameraLock"];

            //    _leftClickInputAction = inputActions["LeftClick"];
            //    _rightClickInputAction = inputActions["RightClick"];
            //}
            var input = GetComponent<PlayerInput>();
            _model = GetComponent<PlayerModel>();

            var inputActions = input.actions;

            _abilityInputAction1 = inputActions["Ability1"];
            _abilityInputAction2 = inputActions["Ability2"];
            _abilityInputAction3 = inputActions["Ability3"];
            _spaceInputAction = inputActions["CameraLock"];

            _leftClickInputAction = inputActions["LeftClick"];
            _rightClickInputAction = inputActions["RightClick"];

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


        private void SpaceInputAction(InputAction.CallbackContext context)
        {
            OnSpace?.Invoke();
        }

        private void LeftClickInputActionInput(InputAction.CallbackContext context)
        {
            //_leftClickCommand = context.ReadValue<float>() > 0.5f;
            OnLeftClick?.Invoke();
        }

        private void RightClickInputActionInput(InputAction.CallbackContext context)
        {
            var mouse = Mouse.current.position.ReadValue();
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(mouse), out var hit, Mathf.Infinity, _model.GetBasicLayerMask())) 
                return;
            
            if (Vector3.Distance(hit.point, transform.position) < _model.GetData().DistanceToBasicAttack)
            {
                if (hit.collider.TryGetComponent(out PlayerModel model))
                {
                    if (model != _model)
                    {
                        model.DoDamage(_model.GetStats().damage, _model.photonView.Owner);
                        return;
                    }
                }

                if (hit.collider.TryGetComponent(out StructureModel structureModel))
                {
                    structureModel.DoDamage(_model.GetStats().damage, _model.photonView.Owner);
                    return;
                }
            }

            OnRightClick?.Invoke(hit.point);
        }

        private void AbilityInputAction1Input(InputAction.CallbackContext context)
        {
            //_activeOneCommand = context.ReadValue<float>() > 0.5f;
            OnActiveOne?.Invoke();
        }
        private void AbilityInputAction2Input(InputAction.CallbackContext context)
        {
            //_activeTwoCommand = context.ReadValue<float>() > 0.5f;
            OnActiveTwo?.Invoke();
        }
        private void AbilityInputAction3Input(InputAction.CallbackContext context)
        {
            //_activeThreeCommand = context.ReadValue<float>() > 0.5f;
            OnActiveThree?.Invoke();
        }
    }
}
