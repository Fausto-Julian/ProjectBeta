using System;
using _ProjectBeta.Scripts.Extension;
using _ProjectBeta.Scripts.PlayerScrips.Interface;
using _ProjectBeta.Scripts.Projectiles;
using _ProjectBeta.Scripts.Structure;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _ProjectBeta.Scripts.PlayerScrips
{
    public class PlayerController : MonoBehaviourPun, IPlayerController
    {
        [SerializeField] private BasicProjectile projectilePrefab;
        [SerializeField] private float cooldownBasic;
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

        private float _currentCooldownBasic;

        private void Awake()
        {
            if(photonView.IsMine)
            {
                PlayerInputGetActions();
            }
        }

        private void PlayerInputGetActions()
        {
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
            
            if (Vector3.Distance(hit.point, transform.position) < _model.GetData().DistanceToBasicAttack && _currentCooldownBasic <= Time.time)
            {
                var layer = _model.GetProjectileLayerMask();
                
                if (hit.collider.TryGetComponent(out PlayerModel model))
                {
                    if (model != _model)
                    {
                        var projectile = PhotonNetworkExtension.Instantiate(projectilePrefab, transform.position, Quaternion.identity, layer);
                        projectile.Initialize(_model.GetStats().damage, model.transform);
                        _currentCooldownBasic = Time.time + cooldownBasic;
                        return;
                    }
                }

                if (hit.collider.TryGetComponent(out StructureModel structureModel))
                {
                    var projectile = PhotonNetworkExtension.Instantiate(projectilePrefab, transform.position, Quaternion.identity, layer);
                    projectile.Initialize(_model.GetStats().damage, structureModel.transform);
                    _currentCooldownBasic = Time.time + cooldownBasic;
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
