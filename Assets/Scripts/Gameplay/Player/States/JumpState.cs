using System;
using Animancer.FSM;
using UnityEngine;

namespace lota.gameplay
{
    public partial class PlayerController
    {
        [Serializable]
        public class JumpState : PlayerState
        {
            public JumpState(PlayerController player) : base(player)
            {
            }

            public override bool CanEnterState => player.IsGrounded && StateMachine.CurrentState.GetType() != typeof(CrouchState);

            public override void OnEnterState()
            {
                player.lastKnownYPositionBeforeJump = player.bodyCollider.bounds.center.y;
                Vector3 targetLinearVelocity = -Physics.gravity.normalized * player.JumpVelocity;
                player.body.linearVelocity += targetLinearVelocity;
                player.IsGrounded = false;

                //Debug.Log("Jumped!");
            }
        }
    }



}
