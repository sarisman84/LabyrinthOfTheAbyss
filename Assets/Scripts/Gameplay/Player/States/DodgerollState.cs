using System;
using UnityEngine;

namespace lota.gameplay
{
    public partial class PlayerController
    {
        [Serializable]
        public class DodgerollState : PlayerState
        {
            public DodgerollState(PlayerController player) : base(player)
            {
            }

            public override void OnEnterState()
            {
                Debug.Log("Dodgerolled!");
            }
        }

    }





}
