using System;
using Animancer.FSM;
using lota.systemic;
using Spyro;

namespace lota.gameplay
{
	public partial class PlayerController
	{
		[Serializable]
		public abstract class PlayerState : IState
		{
			protected PlayerController player;
			protected InputService InputService => ServiceLocator<InputService>.Service;
			protected Animancer.FSM.StateMachine<PlayerState> StateMachine => player.stateMachine;
			public virtual bool CanEnterState => true;
			public virtual bool CanExitState => true;
			public virtual void OnEnterState() { }
			public virtual void OnExitState() { }
			public virtual void OnFixedUpdate() { }


			public PlayerState(PlayerController player)
			{
				this.player = player;
			}
		}

	}





}
