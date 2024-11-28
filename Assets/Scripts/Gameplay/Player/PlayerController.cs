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
		public float jumpHeightInMeters;
		public float movementSpeed;
		public float sprintSpeed;

		public float JumpVelocity => Mathf.Sqrt(2 * Physics.gravity.magnitude * jumpHeightInMeters);

		private Vector3 velocity;
		private bool isGrounded;

		private Rigidbody body;
		private InputService inputService;
		private Camera mainCamera;
		private void Awake()
		{
			body = GetComponent<Rigidbody>();
			body.isKinematic = true;

			inputService = ServiceLocator<InputService>.Service;
			mainCamera = Camera.main;
		}

		private void Update()
		{
			HandleMovement();
			HandleJump();
			HandleGravity();


			body.MovePosition(body.position + velocity);
		}

		private void HandleMovement()
		{
			velocity = LocalizedInputToCameraLook(inputService.GetActionAxis(InputActionID.Player_Move).ToVector3XZ());
		}

		private void HandleJump()
		{
			if (isGrounded && inputService.IsActionPressed(InputActionID.Player_Jump))
			{
				velocity.y = JumpVelocity;
				isGrounded = false;
			}
		}

		private void HandleGravity()
		{
			if (!isGrounded)
			{
				// Apply gravity to vertical velocity
				velocity.y += Physics.gravity.y * Time.deltaTime;
			}

			// Prevent infinite downward velocity
			if (isGrounded && velocity.y < 0)
			{
				velocity.y = 0f;
			}
		}

		private Vector3 LocalizedInputToCameraLook(Vector3 rawInput)
		{
			var result = mainCamera.transform.right * rawInput.x + mainCamera.transform.forward * rawInput.z;
			result.y = 0;
			return result.normalized;
		}

		private void OnCollisionEnter(Collision collision)
		{
			// Basic grounded check
			if (collision.contacts[0].normal.y > 0.5f)
			{
				isGrounded = true;
			}
		}

		private void OnCollisionExit(Collision collision)
		{
			isGrounded = false;
		}
	}
}
