using System;
using System.Collections;
using lota.gameplay.player;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace lota.gameplay
{
	public class Interactable : MonoBehaviour
	{
		[SerializeField] public string InteractPromptText { get; private set; }

		public event Action<InteractionController> onInteract;
		public void InvokeInteractEvent(InteractionController interactionController)
		{
			onInteract?.Invoke(interactionController);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent<InteractionController>(out var interactionController))
				return;
			interactionController.FocusOn(this);
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.TryGetComponent<InteractionController>(out var interactionController))
				return;
			interactionController.Defocus();
		}
	}
}
