using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.InputSystem.Users;
using System;
using UnityEngine.InputSystem.LowLevel;
using System.Linq;

namespace lota.gameplay.player
{
    [RequireComponent(typeof(EntityController))]
    public class PlayerController : MonoBehaviour
    {
        enum UserInputType
        {
            Keyboard,
            Gamepad
        }

        [Recursive]
        public PlayerSettings settings;
        [Header("Input")]
        public InputActionReference move;
        public InputActionReference jump;
        public InputActionReference primaryInteract;
        public InputActionReference secondaryInteract;
        public InputActionReference crouch;
        public InputActionReference sprint;


        private EntityController entityController;
        private float movementModifier = 1.0f;
        private Vector3 spawnPosition;
        private Camera mainCamera;
        private CinemachineBrain mainCameraBrain;
        private InputDevice inputDevice;



        void Awake()
        {
            mainCamera = Camera.main;

            InitializeEntityController();
            SetupCinemachineBrain();

            InputSystem.onEvent += OnInputSystemEvent;
            //DEBUG
            spawnPosition = transform.position;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


        }
        void OnInputSystemEvent(InputEventPtr eventPtr, InputDevice device)
        {
            if (inputDevice == device)
            {
                return;
            }

            var eventType = eventPtr.type;
            if (eventType == StateEvent.Type)
            {
                if (!eventPtr.EnumerateChangedControls(device, 0.0001f).Any())
                {
                    return;
                }
            }

            inputDevice = device;
        }

        private void SetupCinemachineBrain()
        {
            mainCameraBrain = mainCamera.GetComponent<CinemachineBrain>();
            mainCameraBrain.WorldUpOverride = transform;
        }

        private void InitializeEntityController()
        {
            entityController = GetComponent<EntityController>();
            entityController.GravitySettings = new EntityController.GravityData
            {
                gravity = settings.gravity,
                fallModifier = settings.fallMultiplier,
                lowJumpModifier = settings.lowFallMultiplier
            };
            transform.up = -settings.gravityDirection;
            entityController.Acceleration = settings.acceleration;
            entityController.Decceleration = settings.decceleration;
        }

        private void SetActionStates(bool newState)
        {
            if (newState)
            {
                move.action.Enable();
                jump.action.Enable();
                primaryInteract.action.Enable();
                secondaryInteract.action.Enable();
                crouch.action.Enable();
                sprint.action.Enable();
                return;
            }
            move.action.Disable();
            jump.action.Disable();
            primaryInteract.action.Disable();
            secondaryInteract.action.Disable();
            crouch.action.Disable();
            sprint.action.Disable();
        }

        private void OnSprint(InputAction.CallbackContext context)
        {

        }

        private void Update()
        {
            TryJumpingEntity();
            MoveEntity();

            //DEBUG
            if (Keyboard.current.escapeKey.isPressed)
            {
                Cursor.visible = !Cursor.visible;
            }
        }

        private void TryJumpingEntity()
        {
            if (jump.action.ReadValue<float>() > 0)
            {
                entityController.Jump(settings.jumpHeight);
            }
        }

        private void MoveEntity()
        {
            movementModifier = sprint.action.ReadValue<float>() > 0 ? settings.sprintModifier : 1.0f;

            var input = move.action.ReadValue<Vector2>() * (inputDevice is Gamepad ? 10.0f : 1.0f);

            entityController.RootMove(input, movementModifier);

            var lookDirection = mainCamera.transform.forward;
            lookDirection.y = 0.0f;
            entityController.RotateTowards(lookDirection, settings.rotationSpeed);


        }

        private void OnEnable()
        {
            SetActionStates(true);
        }

        private void OnDisable()
        {
            SetActionStates(false);
        }


        private void OnDrawGizmos()
        {
            if (!settings)
            {
                return;
            }
            settings.RenderGizmos(new Vector3(transform.position.x, spawnPosition.y, transform.position.z), transform.up.normalized);
        }
    }
}

