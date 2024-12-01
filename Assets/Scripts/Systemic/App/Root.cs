using System;
using System.Collections.Generic;
using lota.gameplay.interactions;
using Spyro;
using UnityEngine;

namespace lota.systemic
{
	public class Root : MonoBehaviour
	{
		[SerializeField] private List<SceneRef> scenesToLoad;

		private InteractableManager interactableManager;

		void Awake()
		{
			interactableManager = ServiceLocator<InteractableManager>.Service;
			foreach (SceneRef scene in scenesToLoad)
			{
				scene.Load(true);
			}
		}

		void Update()
		{
			interactableManager.Update();
		}


		void OnDrawGizmos()
		{
			interactableManager ??= ServiceLocator<InteractableManager>.Service;
			interactableManager.OnDrawGizmos();
		}
	}
}

