using Spyro;
using UnityEngine;

namespace lota.gameplay.interactions
{
	public class ExampleInteractable : MonoBehaviour, IInteractable
	{
		public string interactMessage, interactHoverEnterMessage, interactHoverExitMessage;

		public float detectionRadius;


		private void Awake()
		{
			ServiceLocator<InteractableManager>.Service.RegisterInteractableAs(this, new SphereDetector(detectionRadius));
		}

		void OnDestroy()
		{
			ServiceLocator<InteractableManager>.Service.DisposeInteractable(this);
		}
		public GameObject GetOwner()
		{
			return gameObject;
		}

		public void OnInteractCommit()
		{
			Debug.Log(interactMessage);
		}

		public void OnInteractHoverEnter()
		{
			Debug.Log(interactHoverEnterMessage);
		}

		public void OnInteractHoverExit()
		{
			Debug.Log(interactHoverExitMessage);
		}

		private void OnDrawGizmos()
		{
			ServiceLocator<InteractableManager>.Service.DrawInteractionHitbox(this, new SphereDetector(detectionRadius));
		}
	}
}
