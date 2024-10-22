using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Spyro.Editor;
using Unity.Cinemachine;

namespace lota.gameplay.player
{
    [RequireComponent(typeof(EntityController))]
    public class PlayerInput : MonoBehaviour
    {
        [Recursive]
        public PlayerSettings settings;
        public InputActionAsset input;

        private InputAction
            moveAction,
            jumpAction,
            primaryInteractAction,
            secondaryInteractAction,
            crouchAction,
            sprintAction;

        private EntityController entityController;
        private float movementModifier = 1.0f;
        private Vector3 spawnPosition;
        private Camera mainCamera;
        private CinemachineBrain mainCameraBrain;

        void Awake()
        {
            mainCamera = Camera.main;

            InitializeEntityController();
            SetupCinemachineBrain();
            FetchActions();
            BindActions();

            //DEBUG
            spawnPosition = transform.position;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void SetupCinemachineBrain()
        {
            mainCameraBrain = mainCamera.GetComponent<CinemachineBrain>();
            mainCameraBrain.WorldUpOverride = transform;
        }

        private void InitializeEntityController()
        {
            entityController = GetComponent<EntityController>();
            entityController.Gravity = settings.gravity;
            transform.up = -settings.gravityDirection;
        }

        private void FetchActions()
        {
            moveAction = input.FindAction("Move", true);
            jumpAction = input.FindAction("Jump", true);
            primaryInteractAction = input.FindAction("Primary_Attack", true);
            secondaryInteractAction = input.FindAction("Secondary_Attack", true);
            crouchAction = input.FindAction("Crouch", true);
            sprintAction = input.FindAction("Sprint", true);
        }

        private void BindActions()
        {
            jumpAction.performed += OnJump;
            sprintAction.performed += OnSprint;
        }
        private void OnJump(InputAction.CallbackContext context)
        {
            entityController.Jump(settings.jumpHeight);
        }

        private void OnSprint(InputAction.CallbackContext context)
        {
            movementModifier = context.ReadValue<float>() > 0 ? settings.sprintModifier : 1.0f;
        }

        private void Update()
        {
            MoveEntity();

            //DEBUG
            if (Keyboard.current.escapeKey.isPressed)
            {
                Cursor.visible = !Cursor.visible;
            }
        }

        private void MoveEntity()
        {
            var input = moveAction.ReadValue<Vector2>();
            var direction = mainCamera.transform.right * input.x + mainCamera.transform.forward * input.y;
            direction.y = 0;
            entityController.Move(direction, settings.movementSpeed * movementModifier);
        }

        private void OnEnable()
        {
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
        }


        private void OnDrawGizmos()
        {
            settings.RenderGizmos(new Vector3(transform.position.x, spawnPosition.y, transform.position.z));
        }
    }
}

