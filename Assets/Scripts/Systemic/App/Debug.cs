using UnityEngine;

namespace lota.systemic.debug
{
	public class GameDebugger : MonoBehaviour
	{
		public void PrintMessage(string message)
		{
			Debug.Log(message);
		}
	}
}
