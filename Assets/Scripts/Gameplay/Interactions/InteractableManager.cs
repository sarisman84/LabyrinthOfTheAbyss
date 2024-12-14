using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace lota.gameplay.interactions
{

	public partial class InteractableManager
	{
		private struct Interactable
		{
			public DetectionBase interactableDetectionCondition;
			public IInteractable interactable;
			public int hoverStateFlag;
			public bool exitHoverOnInteractCommitFlag;


			public void UpdateDetectionCondition(DetectionBase newDetectionCondition)
			{
				interactableDetectionCondition = newDetectionCondition;
			}
		}
		private List<Interactable> registeredInteractables;
		private Dictionary<int, int> sparseIndexRegistry;
		private InteractionController interactionController;
		private int lastHoveredInteractable;
		private IInteractionEventProcessor eventHandlerChain;

		private HoverEnterInteractionProcessor enterEvent;
		private HoverExitInteractionProcessor exitEvent;

		public InteractableManager()
		{
			registeredInteractables = new List<Interactable>();
			sparseIndexRegistry = new Dictionary<int, int>();

			eventHandlerChain = new HoverEnterInteractionProcessor();
			eventHandlerChain.SetNext(new HoverExitInteractionProcessor());

			enterEvent = new();
			exitEvent = new();
		}

		public void RegisterInteractableAs(IInteractable interactable, DetectionBase detectionDesc, bool exitHoverOnInteractCommit = true)
		{
			Trace.Assert(interactable != null, "interactable was null");

			if (sparseIndexRegistry.ContainsKey(interactable.GetOwner().GetInstanceID()))
			{
				return;
			}


			sparseIndexRegistry.Add(interactable.GetOwner().GetInstanceID(), registeredInteractables.Count);
			registeredInteractables.Add(
				new()
				{
					interactableDetectionCondition = detectionDesc,
					interactable = interactable,
					exitHoverOnInteractCommitFlag = exitHoverOnInteractCommit
				});

		}

		public void RegisterInteractionController(InteractionController controller)
		{
			interactionController = controller;
		}

		public void DisposeInteractable(IInteractable interactable)
		{
			int key = interactable.GetOwner().GetInstanceID();
			int index = sparseIndexRegistry[key];

			registeredInteractables.RemoveAt(index);
			sparseIndexRegistry.Remove(key);
		}


		public void Update()
		{
			for (int i = 0; i < registeredInteractables.Count; i++)
			{
				Interactable entry = registeredInteractables[i];
				IInteractable interactable = entry.interactable;

				eventHandlerChain.Process(this, ref entry);

				if (entry.hoverStateFlag != IInteractable.HOVERSTATE_ENTERED)
				{
					continue;
				}



				if (interactionController.HasInteracted())
				{
					if (entry.exitHoverOnInteractCommitFlag)
					{
						eventHandlerChain.Process(this, ref entry);
					}

					interactable.OnInteractCommit();
				}

				return;

				// InteractionDetectionEvent detectionCondition = registeredInteractables[id].detectionConditionEvent;


				// if (!detectionCondition.Invoke(interactionController, registeredInteractables[id].interactable))
				// {
				// 	if (registeredInteractables[id].hoverStateFlag == Interactable.HOVERSTATE_ENTERED)
				// 	{
				// 		TriggerInteractionEvent(ref registeredInteractables, id, Interactable.HOVERSTATE_EXIT);

				// 	}
				// 	continue;
				// }

				// if (registeredInteractables[id].hoverStateFlag != Interactable.HOVERSTATE_ENTERED)
				// {
				// 	if (registeredInteractables.ContainsKey(lastHoveredInteractable))
				// 	{
				// 		TriggerInteractionEvent(ref registeredInteractables, lastHoveredInteractable, Interactable.HOVERSTATE_EXIT);
				// 	}

				// 	TriggerInteractionEvent(ref registeredInteractables, id, Interactable.HOVERSTATE_ENTERED);
				// 	lastHoveredInteractable = id;
				// }

				// if (interactionController.HasInteracted())
				// {
				// 	registeredInteractables[id].interactable.OnInteractCommit();
				// 	if (registeredInteractables[id].exitHoverOnInteractCommitFlag)
				// 	{
				// 		TriggerInteractionEvent(ref registeredInteractables, id, Interactable.HOVERSTATE_EXIT);
				// 	}
				// }

				// return;

			}
		}


		public void DrawInteractionHitbox(IInteractable interactable, DetectionBase detectionDesc)
		{
			Trace.Assert(interactable != null, "interactable was null");
			int key = interactable.GetOwner().GetInstanceID();
			if (!sparseIndexRegistry.ContainsKey(interactable.GetOwner().GetInstanceID()))
			{
				RegisterInteractableAs(interactable, detectionDesc);
			}
			var entry = registeredInteractables[sparseIndexRegistry[key]];
			entry.UpdateDetectionCondition(detectionDesc);
			detectionDesc.DrawGizmo(interactable, entry.hoverStateFlag);
		}
	}
}
