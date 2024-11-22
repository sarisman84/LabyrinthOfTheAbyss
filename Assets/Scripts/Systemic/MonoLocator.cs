using System;
using UnityEngine;

namespace lota
{
	public class MonoLocator<T> where T : MonoBehaviour
	{
		public static T Service { get; } = FetchService();

		private static T FetchService()
		{
			if (Service)
				return Service;

			return GameObject.FindFirstObjectByType<T>();
		}
	}
}