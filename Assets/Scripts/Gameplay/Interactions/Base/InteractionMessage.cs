using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using lota.systemic.ui;
using Spyro;

namespace lota.gameplay.interactions
{
	public partial class InteractableManager
	{
		private interface IInteractionEventProcessor
		{
			IInteractionEventProcessor SetNext(IInteractionEventProcessor processor);
			void Process(InteractableManager manager, Dictionary<int, Interactable> reg, int key);
		}

		private abstract class InteractionEventProcessorBase : IInteractionEventProcessor
		{
			IInteractionEventProcessor next;
			public virtual void Process(InteractableManager manager, Dictionary<int, Interactable> reg, int key) => next?.Process(manager, reg, key);
			public IInteractionEventProcessor SetNext(IInteractionEventProcessor processor) => next = processor;
		}


		private class HoverEnterInteractionProcessor : InteractionEventProcessorBase
		{
			public override void Process(InteractableManager manager, Dictionary<int, Interactable> reg, int key)
			{
				if (reg[key].Detector.TestDetection(reg[key], manager.InteractionController) && reg[key].IsActive)
				{
					var knownInteractable = manager.lastHoveredInteractable;
					if (knownInteractable != key)
					{
						if (reg.ContainsKey(knownInteractable))
						{
							//UnityEngine.Debug.Log($"Exited! {reg[knownInteractable].gameObject.name}");
							reg[knownInteractable].OnInteractHoverExit();
							reg[knownInteractable].IsDetected = false;
						}

						//UnityEngine.Debug.Log($"Entered! {reg[key].gameObject.name}");
						reg[key].OnInteractHoverEnter();
						reg[key].IsDetected = true;
						ServiceLocator<UIService>.Service.ShowInteractionMessage(reg[key].UIDisplayMessage);
						manager.lastHoveredInteractable = key;
					}
					return;
				}
				base.Process(manager, reg, key);
			}
		}

		private class HoverExitInteractionProcessor : InteractionEventProcessorBase
		{
			public override void Process(InteractableManager manager, Dictionary<int, Interactable> reg, int key)
			{
				// UnityEngine.Debug.Log(entry.hoverStateFlag);
				var knownInteractable = manager.lastHoveredInteractable;
				if (knownInteractable == key)
				{
					//UnityEngine.Debug.Log($"Exited! {reg[key].gameObject.name}");
					reg[key].OnInteractHoverExit();
					reg[key].IsDetected = false;

					ServiceLocator<UIService>.Service.HideInteractionMessage();
					manager.lastHoveredInteractable = 0;
					return;
				}

				base.Process(manager, reg, key);
			}
		}


		private class InputInteractInteractionProcesss : InteractionEventProcessorBase
		{
			public override void Process(InteractableManager manager, Dictionary<int, Interactable> reg, int key)
			{
				// UnityEngine.Debug.Log(entry.hoverStateFlag);
				var knownInteractable = manager.lastHoveredInteractable;
				if (knownInteractable == key && manager.InteractionController.HasInteracted() && reg[key].IsActive)
				{
					//UnityEngine.Debug.Log($"Interacted! {reg[key].interactable.GetOwner().name}");
					reg[key].OnInteractCommit();

					ServiceLocator<UIService>.Service.HideInteractionMessage();
					if (reg[key].triggerOnce)
					{
						manager.lastHoveredInteractable = 0;
						reg[key].Disable();
						reg[key].IsDetected = false;
					}
					return;
				}
				//UnityEngine.Debug.Log($"To next process! {reg[key].interactable.GetOwner().name}");
				base.Process(manager, reg, key);
			}
		}


	}


}