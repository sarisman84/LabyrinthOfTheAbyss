using System;
using lota.generated.input;
using lota.utility;
using UnityEngine;
using Animancer.FSM;

namespace lota.gameplay
{
    public partial class PlayerController
    {
        [Serializable]
        public class CrouchState : PlayerState
        {
            public CrouchState(PlayerController player) : base(player)
            {
            }

            public override void OnEnterState()
            {
                //Debug.Log("Crouching");
            }

            public override bool CanEnterState => player.IsGrounded;

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
