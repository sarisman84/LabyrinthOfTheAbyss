using System;
using UnityEngine;

namespace lota.gameplay
{
    public partial class PlayerController
	{
        [Serializable]
        public class FallingState : PlayerState
        {
            public FallingState(PlayerController player) : base(player)
            {
            }

            public override void OnEnterState()
            {
                //Debug.Log("Falling");
            }

            public override bool CanEnterState => player.body.linearVelocity.y < 0.0f && !player.IsGrounded;
        }

    }





}
