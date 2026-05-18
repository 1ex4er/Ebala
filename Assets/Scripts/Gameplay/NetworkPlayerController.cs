using Unity.Netcode;
using UnityEngine;

namespace Ebala.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class NetworkPlayerController : NetworkBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float gravity = -20f;

        private CharacterController _controller;
        private float _velocityY;
        private Camera _localCamera;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        public override void OnNetworkSpawn()
        {
            enabled = IsOwner;

            if (IsOwner)
            {
                SetupCamera();
            }
        }

        private void SetupCamera()
        {
            var cameraObject = new GameObject("LocalPlayerCamera");
            _localCamera = cameraObject.AddComponent<Camera>();
            _localCamera.transform.SetParent(transform);
            _localCamera.transform.localPosition = new Vector3(0f, 1.6f, -3f);
            _localCamera.transform.localRotation = Quaternion.Euler(12f, 0f, 0f);
        }

        private void Update()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            var moveDirection = (transform.right * horizontal + transform.forward * vertical).normalized;
            var horizontalVelocity = moveDirection * moveSpeed;

            if (_controller.isGrounded && _velocityY < 0f)
            {
                _velocityY = -2f;
            }

            _velocityY += gravity * Time.deltaTime;
            horizontalVelocity.y = _velocityY;

            _controller.Move(horizontalVelocity * Time.deltaTime);

            var mouseX = Input.GetAxis("Mouse X") * 100f * Time.deltaTime;
            transform.Rotate(Vector3.up * mouseX);
        }
    }
}
