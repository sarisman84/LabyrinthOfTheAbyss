using System;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace lota.gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class EntityController : MonoBehaviour
    {
        public struct GravityData
        {
            public float gravity;
            public float fallModifier;
            public float lowJumpModifier;
        }

        public GravityData GravitySettings { private get; set; }
        public float Acceleration { private get; set; }
        public float Decceleration { private get; set; }


        private CharacterController controller;
        private Animator animator;
        private Vector3 verticalRelativeVelocity;
        private Vector3 horizontalRelativeVelocity;
        private bool currentFrameJumpFlag;
        private float movementMultiplier;


        void Awake()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
        }
        public void Jump(float jumpHeight)
        {
            currentFrameJumpFlag = true;
            if (!controller.isGrounded)
            {
                return;
            }
            verticalRelativeVelocity = transform.up.normalized * CalculateForce(jumpHeight, GravitySettings.gravity);
        }

        private float CalculateForce(float jumpHeight, float gravity)
        {
            return Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);
        }

        public void Move(Vector3 direction, float speed)
        {
            controller.Move(UpdateVelocity(ref horizontalRelativeVelocity, direction, speed) * Time.deltaTime);
        }

        public void RootMove(Vector3 input, float speedRatio = 1.0f)
        {
            const float DampTime = 0.05f;
            animator.SetFloat("xInput", input.x, DampTime, Time.deltaTime);
            animator.SetFloat("yInput", input.y, DampTime, Time.deltaTime);

            movementMultiplier = speedRatio;
        }

        public void RotateTowards(Vector3 lookDirection, float rotationSpeed = 100.0f)
        {
            if (lookDirection.magnitude > 0.0f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection, transform.up.normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        private Vector3 UpdateVelocity(ref Vector3 currentVelocity, Vector3 direction, float speed = 1.0f)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, Decceleration * Time.deltaTime);
            if (direction.magnitude > 0)
            {
                var localVelocity = direction * speed;
                currentVelocity = Vector3.Lerp(currentVelocity, localVelocity, Acceleration * Time.deltaTime);
            }
            return currentVelocity;
        }

        private void Update()
        {
            HandleGravity();
        }

        private void HandleGravity()
        {
            if (controller.isGrounded)
            {
                verticalRelativeVelocity = Vector3.zero;
                return;
            }

            const float ApproxLimit = 0.9f;
            verticalRelativeVelocity -= transform.up.normalized * GravitySettings.gravity * Time.deltaTime;

            bool isFalling = Vector3.Dot(verticalRelativeVelocity, -transform.up.normalized) >= ApproxLimit;
            bool isRising = Vector3.Dot(verticalRelativeVelocity, transform.up.normalized) >= ApproxLimit;

            if (isFalling)
            {
                verticalRelativeVelocity -= transform.up.normalized * GravitySettings.gravity * (GravitySettings.fallModifier - 1) * Time.deltaTime;
            }
            else if (isRising && !currentFrameJumpFlag)
            {
                verticalRelativeVelocity -= transform.up.normalized * GravitySettings.gravity * (GravitySettings.lowJumpModifier - 1) * Time.deltaTime;
            }
            controller.Move(verticalRelativeVelocity * Time.deltaTime);
            currentFrameJumpFlag = false;
        }


        private void OnDrawGizmos()
        {
            if (controller)
            {
                Gizmos.color = controller.isGrounded ? Color.red : Color.cyan;
                Gizmos.DrawSphere(transform.position, 0.25f);
            }


            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, verticalRelativeVelocity);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, horizontalRelativeVelocity);
        }


        private void OnAnimatorMove()
        {
            if (!animator)
            {
                return;
            }

            controller.Move(animator.deltaPosition * movementMultiplier);
        }
    }
}

