using lota.generated.input;
using lota.systemic;
using Spyro;
using Unity.VisualScripting;
using UnityEngine;

namespace lota.gameplay.interactions
{
	public class ExampleInteractable : Interactable
	{
		public string interactMessage;

		public Bounds detectionBounds;

		public override string UIDisplayMessage => $"Press <input:{InputService.IDToString(InputActionID.Player_Interact)}> to interact {gameObject.name}!";

		protected override DetectionBase Detection => new BoundsDetector(detectionBounds.center, detectionBounds.size);

		public override void OnInteractCommit()
		{
			Debug.Log(interactMessage);
		}


		// private void Awake()
		// {
		// 	ServiceLocator<InteractableManager>.Service.RegisterInteractableAs(this, new SphereDetector(detectionRadius));
		// }

		// void OnDestroy()
		// {
		// 	ServiceLocator<InteractableManager>.Service.DisposeInteractable(this);
		// }
		// public GameObject GetOwner()
		// {
		// 	return gameObject;
		// }

		// public void OnInteractCommit()
		// {
		// 	Debug.Log(interactMessage);
		// }

		// public void OnInteractHoverEnter()
		// {
		// 	//Debug.Log(interactHoverEnterMessage);
		// }

		// public void OnInteractHoverExit()
		// {
		// 	//Debug.Log(interactHoverExitMessage);
		// }

		// private void OnDrawGizmos()
		// {
		// 	ServiceLocator<InteractableManager>.Service.DrawInteractionHitbox(this, new SphereDetector(detectionRadius));
		// }
	}
}
