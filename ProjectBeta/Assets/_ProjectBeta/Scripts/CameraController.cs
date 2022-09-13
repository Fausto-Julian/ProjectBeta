using UnityEngine;
using UnityEngine.InputSystem;

namespace _ProjectBeta.Scripts
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float camSpeed;
        [SerializeField] private float borderThickness;
        [SerializeField] private float smoothness;

        private Vector3 _offset;
        private Vector3 _pos;

        private bool _isCameraLocked;

        private IPlayerController _playerController;
        private Transform _target;

        private void Start()
        {
            var playerController = GetComponentInParent<PlayerController>();

            if (playerController == null)
                return;

            if (!playerController.Object.HasInputAuthority)
            {
                Destroy(gameObject);
                return;
            }

            _playerController = playerController;
            _target = playerController.transform;

            _offset = transform.position - _target.position;

            _playerController.OnSpace += CameraLock;

            _isCameraLocked = true;

            transform.SetParent(null);
        }

        private void LateUpdate()
        {
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