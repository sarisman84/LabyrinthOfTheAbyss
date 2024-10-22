using System;
using UnityEditor;
using UnityEngine;

namespace lota.gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class EntityController : MonoBehaviour
    {
        public float Gravity { private get; set; }
        CharacterController controller;
        Vector3 verticalRelativeVelocity;
        Vector3 horizontalRelativeVelocity;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
        }
        public void Jump(float jumpHeight)
        {
            var force = CalculateForce(jumpHeight, Gravity);
            verticalRelativeVelocity = transform.up.normalized * force;
        }

        private float CalculateForce(float jumpHeight, float gravity)
        {
            return Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);
        }

        public void Move(Vector3 direction, float speed)
        {
            horizontalRelativeVelocity = Vector3.Lerp(horizontalRelativeVelocity, Vector3.zero, 0.95f * Time.deltaTime);
            if (direction.magnitude > 0)
            {
                var localVelocity = direction * speed;
                horizontalRelativeVelocity = Vector3.Lerp(horizontalRelativeVelocity, localVelocity, 0.95f * Time.deltaTime);
            }
            controller.Move(horizontalRelativeVelocity * Time.deltaTime);
        }


        private void Update()
        {
            verticalRelativeVelocity -= transform.up.normalized * Gravity * Time.deltaTime;
            controller.Move(verticalRelativeVelocity * Time.deltaTime);
        }

    }
}

