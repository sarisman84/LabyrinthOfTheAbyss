using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace lota.systemic
{
	[CreateAssetMenu(menuName = "LabyrinthOfTheAbyss/Root/Data")]
	public class RootData : ScriptableObject
	{
		public List<SceneRef> scenesToLoad;
		public InputActionAsset globalInput;
	}







}
