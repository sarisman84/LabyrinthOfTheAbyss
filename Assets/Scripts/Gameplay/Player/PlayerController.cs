using lota.systemic;
using lota.utility;
using Spyro;
using UnityEngine;
using lota.generated.input;
using System;

namespace lota.gameplay
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerController : MonoBehaviour
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
		private Vector3 GroundDetectorPosition => (collider.bounds.center) - Vector3.up * (collider.bounds.extents.y + (groundDetectionWidth / 2.0f)) + groundDetectionPositionOffset;
		private Vector3 GroundDetectorSize => new Vector3(collider.bounds.size.x, groundDetectionWidth, collider.bounds.size.z) + groundDetectionSizeOffset;


		private bool jumpInput;
		private Vector3 directionalInput;


		private float lastKnownYPositionBeforeJump;
		private bool isGrounded;
		private Collider[] groundCheckAlloc;

		private Rigidbody body;
		private InputService inputService;
		private Collider collider;
		private Camera mainCamera;
		private void Awake()
		{
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
			directionalInput = LocalizedInputToCameraLook(inputService.GetActionAxis(InputActionID.Player_Move).ToVector3XZ());
			jumpInput = inputService.IsActionHeld(InputActionID.Player_Jump);
		}
		private void FixedUpdate()
		{
			HandleMovement();
			HandleJump();
			HandleGroundCheck();
		}

		private void HandleMovement()
		{
			var newLinearVelocity = directionalInput * movementSpeed;
			var targetLinearVelocity = new Vector3(newLinearVelocity.x, body.linearVelocity.y, newLinearVelocity.z);
			var oldLinearVelicity = body.linearVelocity;
			body.linearVelocity = Vector3.Lerp(oldLinearVelicity, targetLinearVelocity, accelerationSpeed);
		}

		private void HandleJump()
		{
			if (isGrounded && jumpInput && body.linearVelocity.y < 0.5f)
			{
				lastKnownYPositionBeforeJump = collider.bounds.center.y;
				var targetLinearVelocity = -Physics.gravity.normalized * JumpVelocity;
				body.linearVelocity += targetLinearVelocity;
				isGrounded = false;
			}
		}

		private Vector3 LocalizedInputToCameraLook(Vector3 rawInput)
		{
			var result = mainCamera.transform.right * rawInput.x + mainCamera.transform.forward * rawInput.z;
			result.y = 0;
			return result.normalized;
		}

		private void HandleGroundCheck()
		{
			var result = Physics.OverlapBoxNonAlloc(GroundDetectorPosition, GroundDetectorSize, groundCheckAlloc, transform.rotation, groundDetectionMask);
			isGrounded = result > 0;

			for (int i = 0; i < result; ++i)
			{
				Debug.Log(groundCheckAlloc[i].name);
			}
		}

		private void OnDrawGizmos()
		{

			collider ??= GetComponent<Collider>();



			Gizmos.color = isGrounded ? Color.green : Color.red;
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
