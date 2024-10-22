using System;
using UnityEditor;
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
        private Vector3 verticalRelativeVelocity;
        private Vector3 horizontalRelativeVelocity;
        private bool currentFrameJumpFlag;


        void Awake()
        {
            controller = GetComponent<CharacterController>();
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
            horizontalRelativeVelocity = Vector3.Lerp(horizontalRelativeVelocity, Vector3.zero, Decceleration * Time.deltaTime);
            if (direction.magnitude > 0)
            {
                var localVelocity = direction * speed;
                horizontalRelativeVelocity = Vector3.Lerp(horizontalRelativeVelocity, localVelocity, Acceleration * Time.deltaTime);
            }
            controller.Move(horizontalRelativeVelocity * Time.deltaTime);
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
    }
}

