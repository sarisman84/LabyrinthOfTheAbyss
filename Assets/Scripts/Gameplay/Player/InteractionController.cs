using System;
using System.Collections.Generic;
using lota.generated.input;
using lota.systemic;
using Spyro;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace lota.gameplay.player
{
	public class InteractionController : MonoBehaviour
	{
		public InputActionReference interactAction;
		private bool isFocused = false;
		private Interactable focusedInteractable;
		private HUD playerHud;

		private InputService inputService;

		void Awake()
		{
			playerHud = MonoLocator<HUD>.Service;
			inputService = ServiceLocator<InputService>.Service;
		}
		public void Defocus()
		{
			playerHud?.Hide("h_interact");
		}

		public void FocusOn(Interactable interactable)
		{
			playerHud?.Display("h_interact", (element) => { OnDisplayEnabled(element, interactable); });
			focusedInteractable = interactable;
		}

		private void OnDisplayEnabled(VisualElement element, Interactable interactable)
		{
			Label label = element as Label;
			label.text = interactable.InteractPromptText;
		}

		private void Update()
		{
			if (!isFocused)
				return;

			if (inputService.IsActionPressed(InputActionID.Player_Interact))
			{
				focusedInteractable.InvokeInteractEvent(this);
			}
		}


	}
}