using System;
using Animancer;
using Spyro;
using UnityEngine;

namespace lota.gameplay.interactions
{
	public interface IInteractable
	{
		public const int HOVERSTATE_ENTERED = 1, HOVERSTATE_EXIT = 0;
		GameObject GetOwner();
		void OnInteractCommit();
		void OnInteractHoverEnter();
		void OnInteractHoverExit();
	}



}
