using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using lota.utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace lota.gameplay.interactions
{

	public partial class InteractableManager
	{
		private struct InteractableInstance
		{
			public Interactable interactable;
			public bool exitHoverOnInteractCommitFlag;
		}
		private Dictionary<int, Interactable> registeredInteractables;
		public InteractionController InteractionController { get; set; }
		private int lastHoveredInteractable;
		private IInteractionEventProcessor eventHandlerChain;
		public InteractableManager()
		{
			registeredInteractables = new Dictionary<int, Interactable>();

			eventHandlerChain = new InputInteractInteractionProcesss();
			eventHandlerChain.SetNext(new HoverEnterInteractionProcessor()).SetNext(new HoverExitInteractionProcessor());

			SceneManager.sceneLoaded += FetchAllInteractables;
		}

		~InteractableManager()
		{
			SceneManager.sceneLoaded -= FetchAllInteractables;
		}


		public void FetchAllInteractables(Scene scene, LoadSceneMode mode)
		{
			registeredInteractables.Clear();

			scene.ForEachObjectOfType<Interactable>((i) => { registeredInteractables.Add(i.gameObject.GetInstanceID(), i); });
		}

		// public void RegisterInteractableAs(IInteractable interactable, DetectionBase detectionDesc, bool exitHoverOnInteractCommit = true)
		// {
		// 	Trace.Assert(interactable != null, "interactable was null");

		// 	if (registeredInteractables.ContainsKey(interactable.GetOwner().GetInstanceID()))
		// 	{
		// 		return;
		// 	}

		// 	registeredInteractables.Add(
		// 		interactable.GetOwner().GetInstanceID(),
		// 		new()
		// 		{
		// 			interactableDetectionCondition = detectionDesc,
		// 			interactable = interactable,
		// 			exitHoverOnInteractCommitFlag = exitHoverOnInteractCommit
		// 		});

		// }

		public void RegisterInteractionController(InteractionController controller)
		{
			InteractionController = controller;
		}

		// public void DisposeInteractable(IInteractable interactable)
		// {
		// 	int key = interactable.GetOwner().GetInstanceID();

		// 	if (registeredInteractables.ContainsKey(key))
		// 		registeredInteractables.Remove(key);
		// }


		public void Update()
		{
			foreach (var (id, _) in registeredInteractables)
			{
				eventHandlerChain.Process(this, registeredInteractables, id);
			}
		}

		// public void DrawInteractionHitbox(IInteractable interactable, DetectionBase detectionDesc)
		// {
		// 	Trace.Assert(interactable != null, "interactable was null");
		// 	int key = interactable.GetOwner().GetInstanceID();
		// 	if (!registeredInteractables.ContainsKey(interactable.GetOwner().GetInstanceID()))
		// 	{
		// 		RegisterInteractableAs(interactable, detectionDesc);
		// 	}
		// 	var entry = registeredInteractables[key];
		// 	entry.UpdateDetectionCondition(detectionDesc);
		// 	detectionDesc.DrawGizmo(interactable, lastHoveredInteractable);
		// }
	}
}
