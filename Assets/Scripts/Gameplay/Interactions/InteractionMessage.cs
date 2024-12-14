using System.Diagnostics;
using System.Reflection;

namespace lota.gameplay.interactions
{
	public partial class InteractableManager
	{
		private interface IInteractionEventProcessor
		{
			IInteractionEventProcessor SetNext(IInteractionEventProcessor processor);
			void Process(InteractableManager manager, ref Interactable entry);
		}

		private abstract class InteractionEventProcessorBase : IInteractionEventProcessor
		{
			IInteractionEventProcessor next;
			public virtual void Process(InteractableManager manager, ref Interactable entry) => next?.Process(manager, ref entry);
			public IInteractionEventProcessor SetNext(IInteractionEventProcessor processor) => next = processor;
		}


		private class HoverEnterInteractionProcessor : InteractionEventProcessorBase
		{
			public override void Process(InteractableManager manager, ref Interactable entry)
			{
				int id = entry.interactable.GetOwner().GetInstanceID();
				if (entry.hoverStateFlag != IInteractable.HOVERSTATE_ENTERED
				&& entry.interactableDetectionCondition.TestDetection(entry.interactable, manager.interactionController)
				&& manager.lastHoveredInteractable != id)
				{
					UnityEngine.Debug.Log("Entered!");
					var keyReg = manager.sparseIndexRegistry;
					if (keyReg.ContainsKey(manager.lastHoveredInteractable))
					{
						var reg = manager.registeredInteractables;
						var lastEntry = reg[keyReg[manager.lastHoveredInteractable]];
						reg[keyReg[manager.lastHoveredInteractable]].interactable.OnInteractHoverExit();
						lastEntry.hoverStateFlag = IInteractable.HOVERSTATE_EXIT;
						reg[keyReg[manager.lastHoveredInteractable]] = lastEntry;
					}

					entry.hoverStateFlag = IInteractable.HOVERSTATE_ENTERED;
					entry.interactable.OnInteractHoverEnter();
					manager.lastHoveredInteractable = id;
					return;
				}

				base.Process(manager, ref entry);
			}
		}

		private class HoverExitInteractionProcessor : InteractionEventProcessorBase
		{
			public override void Process(InteractableManager manager, ref Interactable entry)
			{

				if (entry.hoverStateFlag != IInteractable.HOVERSTATE_EXIT)
				{
					UnityEngine.Debug.Log("Exited!");
					entry.hoverStateFlag = IInteractable.HOVERSTATE_EXIT;
					entry.interactable.OnInteractHoverExit();
					return;
				}

				base.Process(manager, ref entry);
			}
		}

	}


}