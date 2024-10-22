using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

namespace lota.gameplay.player
{
    [RequireComponent(typeof(EntityController))]
    public class PlayerInput : MonoBehaviour
    {
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

        void Awake()
        {
            mainCamera = Camera.main;

            InitializeEntityController();
            SetupCinemachineBrain();
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

        private void BindActions()
        {
            sprint.action.performed += OnSprint;
        }

        private void OnSprint(InputAction.CallbackContext context)
        {
            movementModifier = context.ReadValue<float>() > 0 ? settings.sprintModifier : 1.0f;
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
            var input = move.action.ReadValue<Vector2>();
            var direction = mainCamera.transform.right * input.x + mainCamera.transform.forward * input.y;
            direction.y = 0;
            entityController.Move(direction, settings.movementSpeed * movementModifier);
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
            settings.RenderGizmos(new Vector3(transform.position.x, spawnPosition.y, transform.position.z));
        }
    }
}

