using lota.systemic;
using lota.utility;
using Spyro;
using UnityEngine;
using lota.generated.input;
using System;

namespace lota.gameplay
{
	[RequireComponent(typeof(Rigidbody))]
	public partial class PlayerController : MonoBehaviour
	{

		[Header("Jump Settings")]
		public float jumpHeightInMeters;
		public float groundDetectionWidth = 0.1f;
		public Vector3 groundDetectionPositionOffset;
		public Vector3 groundDetectionSizeOffset;
		public LayerMask groundDetectionMask;

		[Header("Horizontal Movement Settings")]
		public float movementSpeed;
		public float sprintSpeed;
		[Range(0.0f, 1.0f)]
		public float accelerationSpeed = 0.8f;

		private float JumpVelocity => Mathf.Sqrt(2 * Physics.gravity.magnitude * jumpHeightInMeters);
		private Vector3 GroundDetectorPosition => collider.bounds.center - Vector3.up * (collider.bounds.extents.y + (groundDetectionWidth / 2.0f)) + groundDetectionPositionOffset;
		private Vector3 GroundDetectorSize => new Vector3(collider.bounds.size.x, groundDetectionWidth, collider.bounds.size.z) + groundDetectionSizeOffset;


		private bool jumpInput;
		private Vector3 directionalInput;
		private float finalSpeed;


		private float lastKnownYPositionBeforeJump;
		public bool IsGrounded { get; protected set; }
		private Collider[] groundCheckAlloc;

		private Rigidbody body;
		private InputService inputService;
		private Collider collider;
		private Camera mainCamera;
		[SerializeField] private Animancer.FSM.StateMachine<PlayerState> stateMachine;

		private PlayerState idle, move, crouch, jump, sprint, dodgeroll, fall;

		private void Awake()
		{
			idle = new IdleState(this);
			move = new MoveState(this);
			crouch = new CrouchState(this);
			jump = new JumpState(this);
			sprint = new SprintState(this);
			dodgeroll = new DodgerollState(this);
			fall = new FallingState(this);

			stateMachine = new Animancer.FSM.StateMachine<PlayerState>(idle);
			groundCheckAlloc = new Collider[10];

			body = GetComponent<Rigidbody>();
			collider = GetComponent<Collider>();

			body.freezeRotation = true;

			inputService = ServiceLocator<InputService>.Service;
			mainCamera = Camera.main;

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;



		}
		private void Update()
		{

			if (inputService.IsActionHeld(InputActionID.Player_Jump))
			{
				stateMachine.TrySetState(jump);
			}
			else if (inputService.GetActionAxis(InputActionID.Player_Move).magnitude > 0.0f)
			{
				var finalState = inputService.IsActionHeld(InputActionID.Player_Sprint) ?
				inputService.IsActionHeld(InputActionID.Player_Crouch) ? dodgeroll : sprint : inputService.IsActionHeld(InputActionID.Player_Crouch) ? crouch : move;
				stateMachine.TrySetState(finalState);
			}
			else if (body.linearVelocity.y < 0.0f && !IsGrounded)
			{
				stateMachine.TrySetState(fall);
			}
			else if (stateMachine.CurrentState.GetType() != typeof(IdleState) && IsGrounded)
			{
				stateMachine.TrySetState(idle);
			}


		}


		private void FixedUpdate()
		{
			stateMachine.CurrentState.OnFixedUpdate();
			HandleGroundCheck();
		}



		public static Vector3 LocalizedInputToCameraLook(PlayerController player, Vector3 rawInput)
		{
			var result = player.mainCamera.transform.right * rawInput.x + player.mainCamera.transform.forward * rawInput.z;
			result.y = 0;
			return result.normalized;
		}

		private void HandleGroundCheck()
		{
			var result = Physics.OverlapBoxNonAlloc(GroundDetectorPosition, GroundDetectorSize, groundCheckAlloc, transform.rotation, groundDetectionMask);
			IsGrounded = result > 0;
		}

		private void OnDrawGizmos()
		{

			collider ??= GetComponent<Collider>();



			Gizmos.color = IsGrounded ? Color.green : Color.red;
			Gizmos.DrawCube(GroundDetectorPosition, GroundDetectorSize);
			Gizmos.DrawSphere(GroundDetectorPosition, 0.15f);

			Gizmos.color = Color.magenta;
			Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);

			Gizmos.color = Color.cyan;
			var aPos = collider.bounds.center;
			var bPos = new Vector3(collider.bounds.center.x, lastKnownYPositionBeforeJump, collider.bounds.center.z) + Vector3.up * jumpHeightInMeters;
			Gizmos.DrawSphere(aPos, 0.05f);
			Gizmos.DrawSphere(bPos, 0.05f);
			Gizmos.DrawLine(aPos, bPos);
		}

	}
}
