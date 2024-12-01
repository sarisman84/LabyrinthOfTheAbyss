
using System;
using lota.systemic;
using Spyro;
using UnityEngine;
using UnityEngine.Events;

namespace lota.gameplay.interactions
{
	public class InteractionController : MonoBehaviour
	{
		private Camera mainCamera;

		[SerializeField] private UnityEvent canInteractEvent;

		public UnityEvent CanInteractEvent => canInteractEvent;

		private void Awake()
		{
			mainCamera = Camera.main;
			ServiceLocator<InteractableManager>.Service.RegisterInteractionController(this);
		}

		public bool IsInteractableInView(Interactable interactable)
		{
			// Convert the object's world position to viewport coordinates
			Vector3 viewportPos = mainCamera.WorldToViewportPoint(interactable.transform.position);
			// Check if the object is within the viewport boundaries
			return viewportPos.x > 0
				&& viewportPos.x < 1
				&& viewportPos.y > 0
				&& viewportPos.y < 1
				&& viewportPos.z > 0; // Ensure the object is in front of the camera

		}

		public bool HasInteracted() => ServiceLocator<InputService>.Service.IsActionPressed(generated.input.InputActionID.Player_Interact);
	}
}