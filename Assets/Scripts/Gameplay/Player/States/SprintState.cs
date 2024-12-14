using System;
using lota.generated.input;
using lota.utility;
using UnityEngine;

namespace lota.gameplay
{
    public partial class PlayerController
    {
        [Serializable]

        public class SprintState : PlayerState
        {
            public SprintState(PlayerController player) : base(player)
            {
            }
            public override bool CanEnterState => player.IsGrounded;
            public override void OnEnterState()
            {
                //Debug.Log("Sprinting");
            }
            public override void OnFixedUpdate()
            {
                Vector3 newLinearVelocity = PlayerController.LocalizedInputToCameraLook(player, InputService.GetActionAxis(InputActionID.Player_Move).ToVector3XZ()) * player.sprintSpeed;
                Vector3 targetLinearVelocity = new Vector3(newLinearVelocity.x, player.body.linearVelocity.y, newLinearVelocity.z);
                Vector3 oldLinearVelicity = player.body.linearVelocity;
                player.body.linearVelocity = Vector3.Lerp(oldLinearVelicity, targetLinearVelocity, player.accelerationSpeed);
            }
        }
    }



}
