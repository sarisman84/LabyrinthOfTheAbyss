using System;
using System.Collections.Generic;
using Spyro;
using UnityEngine;

namespace lota.systemic
{
	public class Root : MonoBehaviour
	{
		public List<SceneRef> scenesToLoad;

		void Awake()
		{
			foreach(var scene in scenesToLoad)
			{
				scene.Load(true);
			}
		}
	}
}

