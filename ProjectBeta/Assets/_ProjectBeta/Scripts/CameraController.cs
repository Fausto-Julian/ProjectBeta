using _ProjectBeta.Scripts.PlayerScrips;
using _ProjectBeta.Scripts.PlayerScrips.Interface;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _ProjectBeta.Scripts
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float camSpeed;
        [SerializeField] private float borderThickness;
        [SerializeField] private float smoothness;

        [SerializeField] private float offsetZ;
        [SerializeField] private float offsetY;
        
        private Vector3 _offset;
        private Vector3 _pos;

        private bool _isCameraLocked;

        private IPlayerController _playerController;
        private Transform _target;

        public void SetTarget(PlayerController playerController, bool isTeamOne)
        {
            _playerController = playerController;
            _target = playerController.transform;

            var targetPosition = _target.position;
            var position = new Vector3(targetPosition.x, offsetY, targetPosition.z - offsetZ);
            if (!isTeamOne)
            {
                var euler = transform.rotation.eulerAngles;
                euler.y = 180;
                transform.Rotate(euler);

                position.z = targetPosition.z + offsetZ;
            }
            
            transform.position = position;
            _offset = position - targetPosition;

            _playerController.OnSpace += CameraLock;
            _isCameraLocked = true;
        }

        private void LateUpdate()
        {
            if (_target == default)
                return;
            
            if (!_isCameraLocked)
            {
                var mousePos = Mouse.current.position.ReadValue();
                CameraFreeMovement(mousePos);
                return;
            }

            CameraLockedMovement();
        }

        private void OnDisable()
        {
            if (_playerController == null)
                return;
            
            _playerController.OnSpace -= CameraLock;
        }

        private void CameraLockedMovement()
        {
            _pos = _target.position + _offset;
            transform.position = Vector3.Slerp(transform.position, _pos, smoothness);
        }

        private void CameraFreeMovement(Vector2 mousePos)
        {
            _pos = transform.position;
            //up
            if (mousePos.y >= Screen.height - borderThickness)
                _pos.x -= camSpeed * Time.deltaTime;
            //down
            if (mousePos.y <= borderThickness)
                _pos.x += camSpeed * Time.deltaTime;
            //left
            if (mousePos.x <= borderThickness)
                _pos.z -= camSpeed * Time.deltaTime;
            //right
            if (mousePos.x >= Screen.height - borderThickness)
                _pos.z += camSpeed * Time.deltaTime;

            transform.position = _pos;
        }

        private void CameraLock()
        {
            _isCameraLocked = !_isCameraLocked;
        }
    }
}