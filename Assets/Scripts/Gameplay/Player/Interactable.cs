using System;
using Animancer;
using Spyro;
using UnityEngine;

namespace lota.gameplay.interactions
{
	public class Interactable : MonoBehaviour
	{
		public UnityEvent onInteractEvent;
		public float detectionRadius = 1.0f;

		private void Awake()
		{
			ServiceLocator<InteractableManager>.Service.RegisterInteractable(this);
		}

		void OnDestroy()
		{
			ServiceLocator<InteractableManager>.Service.DisposeInteractable(this);
		}
	}
}
