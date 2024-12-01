using System.Collections.Generic;
using UnityEngine;

namespace lota.gameplay.interactions
{
	public class InteractableManager
	{
		private List<Interactable> registeredInteractables;
		private InteractionController interactionController;

		public InteractableManager()
		{
			registeredInteractables = new List<Interactable>();
		}

		public void RegisterInteractable(Interactable interactable)
		{
			//TODO: Make this register unique interactables.
			registeredInteractables.Add(interactable);
		}

		public void RegisterInteractionController(InteractionController controller)
		{
			interactionController = controller;
		}
		
		public void DisposeInteractable(Interactable interactable)
		{
			registeredInteractables.Remove(interactable);
		}


		public void Update()
		{
			for (int i = 0; i < registeredInteractables.Count; i++)
			{
				Interactable interactable = registeredInteractables[i];

				float dist = Vector3.Distance(interactable.transform.position, interactionController.transform.position);

				if (dist > interactable.detectionRadius)
				{
					continue;
				}

				if (!interactionController.IsInteractableInView(interactable))
				{
					continue;
				}
				interactionController.CanInteractEvent?.Invoke();
				if (!interactionController.HasInteracted())
				{
					continue;
				}
				interactable.onInteractEvent?.Invoke();
			}
		}

		public void OnDrawGizmos()
		{
			for (int i = 0; i < registeredInteractables.Count; i++)
			{
				Interactable interactable = registeredInteractables[i];

				float dist = Vector3.Distance(interactable.transform.position, interactionController.transform.position);

				Gizmos.color = dist > interactable.detectionRadius ? Color.red : Color.green;
				Gizmos.DrawLine(interactionController.transform.position, interactable.transform.position);

				Gizmos.color = Color.cyan - new Color(0, 0, 0, 0.5f);
				Gizmos.DrawSphere(interactable.transform.position, interactable.detectionRadius);
			}
		}
	}
}
