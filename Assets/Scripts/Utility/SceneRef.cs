using UnityEngine;
using UnityEngine.SceneManagement;

namespace lota
{
	[System.Serializable]
	public class SceneRef
	{
		[SerializeField] private string scenePath;

		public string ScenePath => scenePath;

		// You can add a property to fetch the scene's name directly
		public string SceneName => System.IO.Path.GetFileNameWithoutExtension(scenePath);

		public void Load(bool additive = false)
		{
			SceneManager.LoadScene(SceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
		}
	}
}