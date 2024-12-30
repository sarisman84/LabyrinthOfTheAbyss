using System;
using lota.generated.input;
using UnityEngine;

namespace lota.gameplay
{
    public partial class PlayerController
	{
        [Serializable]
        public class IdleState : PlayerState
        {
            public IdleState(PlayerController player) : base(player)
            {
            }

            public override void OnEnterState()
            {
                //Debug.Log("Idling");
            }
        }

    }
}
