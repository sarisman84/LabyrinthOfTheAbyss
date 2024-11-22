using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.InputSystem.Users;
using System;
using UnityEngine.InputSystem.LowLevel;
using System.Linq;
using Spyro;
using lota.systemic;
using lota.utility;
using lota.generated.input;

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
        public InputActionReference primaryItemUse;
        public InputActionReference secondaryItemUse;
        public InputActionReference crouch;
        public InputActionReference sprint;


        private EntityController entityController;
        private float movementModifier = 1.0f;
        private Vector3 spawnPosition;
        private Camera mainCamera;
        private CinemachineBrain mainCameraBrain;
        private InputService inputService;


        public Camera MainCamera => mainCamera;

        void Awake()
        {
            inputService = ServiceLocator<InputService>.Service;
            mainCamera = Camera.main;

            InitializeEntityController();
            SetupCinemachineBrain();


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
            if (inputService.IsActionPressed(InputActionID.Player_Jump))
            {
                entityController.Jump(settings.jumpHeight);
            }
        }

        private void MoveEntity()
        {
            movementModifier = inputService.IsActionPressed(InputActionID.Player_Sprint) ? settings.sprintModifier : 1.0f;

            var input = inputService.GetActionAxis(InputActionID.Player_Move).ToVector3XZ();

            entityController.RootMove(input, movementModifier);

            var lookDirection = mainCamera.transform.forward;
            lookDirection.y = 0.0f;
            entityController.RotateTowards(lookDirection, settings.rotationSpeed);


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

