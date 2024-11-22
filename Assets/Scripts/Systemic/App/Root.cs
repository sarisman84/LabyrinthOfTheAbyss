using System;
using Spyro;
using UnityEngine;

namespace lota.systemic
{
	public class Root
	{
		private static Root _appRoot;
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void OnAppInit()
		{
			_appRoot = new Root();
		}

		Root()
		{
			const string rootDataPath = "App/RootData";
			RootData data = Resources.Load<RootData>(rootDataPath);
			if (!data)
			{
				throw new NullReferenceException($"Could not find RootData at path: {rootDataPath}");
			}
			foreach (var scene in data.scenesToLoad)
			{
				scene.Load(true);
			}

			//ServiceLocator<InputService>.Service.Init(data.globalInput);
			//ServiceLocator<InputService>.Service.EnableInputMap("Player");

		}
	}
}

