using System;
using Animancer.FSM;
using lota.generated.input;
using lota.utility;
using UnityEngine;

namespace lota.gameplay
{
	public partial class PlayerController
	{
		[Serializable]
		public class MoveState : PlayerState
		{
			public MoveState(PlayerController player) : base(player)
			{
			}

            public override bool CanEnterState => player.IsGrounded;
            public override void OnEnterState()
            {
                Debug.Log("Moving");
            }

            public override void OnFixedUpdate()
			{
				var newLinearVelocity = PlayerController.LocalizedInputToCameraLook(player, InputService.GetActionAxis(InputActionID.Player_Move).ToVector3XZ()) * player.movementSpeed;
				var targetLinearVelocity = new Vector3(newLinearVelocity.x, player.body.linearVelocity.y, newLinearVelocity.z);
				var oldLinearVelicity = player.body.linearVelocity;
				player.body.linearVelocity = Vector3.Lerp(oldLinearVelicity, targetLinearVelocity, player.accelerationSpeed);
			}
		}

	}
}