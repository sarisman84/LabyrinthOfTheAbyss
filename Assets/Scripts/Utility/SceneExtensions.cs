using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace lota.utility
{
	public static class SceneExtensions
	{

		public static void ForEachObjectOfType<T>(this Scene scene, Action<T> predicate) where T : MonoBehaviour
		{
			foreach (var obj in scene.GetRootGameObjects())
			{
				if (obj.TryGetComponent<T>(out var t))
					predicate?.Invoke(t);

				if (obj.transform.childCount > 0)
					IterateRecursivelyThroughHierachy(obj.transform, predicate);
			}

		}

		private static void IterateRecursivelyThroughHierachy<T>(Transform root, Action<T> predicate) where T : MonoBehaviour
		{
			for (var i = 0; i < root.transform.childCount; ++i)
			{
				var child = root.transform.GetChild(i);
				if (child.TryGetComponent<T>(out var t))
					predicate?.Invoke(t);

				if (child.childCount > 0)
					IterateRecursivelyThroughHierachy(child, predicate);

			}
		}
	}
}