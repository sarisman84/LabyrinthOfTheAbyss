using System;
using UnityEngine;

namespace lota
{
	public class MonoLocator<T> where T : MonoBehaviour
	{
		public static T Service { get; } = GameObject.FindFirstObjectByType<T>();
	}
}