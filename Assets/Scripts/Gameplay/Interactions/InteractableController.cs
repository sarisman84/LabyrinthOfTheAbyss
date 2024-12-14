
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

		public Bounds CollisionBounds { get; private set; }
		public Camera MainCamera => mainCamera;

		private void Awake()
		{
			mainCamera = Camera.main;
			CollisionBounds = GetComponent<Collider>().bounds;
			ServiceLocator<InteractableManager>.Service.RegisterInteractionController(this);
		}



		public bool HasInteracted() => ServiceLocator<InputService>.Service.IsActionPressed(generated.input.InputActionID.Player_Interact);
	}
}