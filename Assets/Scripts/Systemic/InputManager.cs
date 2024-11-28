using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;
using System.Linq;
using System;
using System.Text;
using System.IO;
using lota.generated.input;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace lota.systemic
{
	public class InputService
	{
		private const string GeneratedContentPath = "Assets/Generated";
		private const string KeybindEnumPath = "/InputKeybindEnum.cs";
		private static InputActionAsset KeybindAsset { get; } = Resources.Load<InputActionAsset>("Controls");
		private Dictionary<int, InputAction> keybindDatabase;
		private Dictionary<int, InputActionMap> mapDatabase;
		private int currentMap = 0;

#if UNITY_EDITOR
		[MenuItem("Tools/LOTA/Update Keybind Enums")]
		private static void GenerateKeybindEnums()
		{
			var keybindEnum = new StringBuilder();
			var mapEnum = new StringBuilder();
			keybindEnum.Append("public enum InputActionID {");
			mapEnum.Append("public enum InputActionMapID {");
			for (int i = 0; i < KeybindAsset.actionMaps.Count; i++)
			{
				var actionMap = KeybindAsset.actionMaps[i];
				mapEnum.Append($"{actionMap.name}{(i == KeybindAsset.actionMaps.Count - 1 ? "" : ",")}");
				for (int j = 0; j < actionMap.actions.Count; j++)
				{
					var action = actionMap.actions[j];
					keybindEnum.Append($"{actionMap.name}_{action.name}{(j == actionMap.actions.Count - 1 ? "" : ",")}");
				}

			}
			keybindEnum.Append("}");
			mapEnum.Append("}");

			var fileContent = new StringBuilder();
			fileContent.AppendLine("namespace lota.generated.input\n{");
			fileContent.AppendLine(mapEnum.ToString());
			fileContent.AppendLine(keybindEnum.ToString());
			fileContent.AppendLine("}");

			if (!Directory.Exists(GeneratedContentPath))
			{
				Directory.CreateDirectory(GeneratedContentPath);
			}

			File.WriteAllText(GeneratedContentPath + KeybindEnumPath, fileContent.ToString());
			AssetDatabase.Refresh();
			Debug.Log("[InputService]: Keybind Enums updated!");
		}
#endif
		public InputService()
		{
			mapDatabase = new Dictionary<int, InputActionMap>();
			keybindDatabase = new Dictionary<int, InputAction>();
			var count = 0;

			for (int i = 0; i < KeybindAsset.actionMaps.Count; i++)
			{
				var actionMap = KeybindAsset.actionMaps[i];
				mapDatabase.Add(i, actionMap);
				foreach (var action in actionMap.actions)
				{
					keybindDatabase.Add(count++, action);
				}
			}
		}

		public InputAction GetKeybind(InputActionID actionID)
		{
			return keybindDatabase[(int)actionID];
		}


		public void RebindKeyOfAction(InputActionID actionID, Action<InputActionRebindingExtensions.RebindingOperation> onCompleteEvent)
		{
			throw new NotImplementedException();
		}

		public bool IsActionPressed(InputActionID actionID)
		{
			var key = (int)actionID;
			var result = keybindDatabase[key].ReadValue<float>() > 0;

			if (result)
			{
				Debug.Log(keybindDatabase[key].name);
			}

			return result;
		}

		public Vector2 GetActionAxis(InputActionID actionID)
		{
			var action = keybindDatabase[(int)actionID];
			return action.ReadValue<Vector2>();
		}

		public void SwitchControlActionMap(InputActionMapID mapID)
		{
			var oldMap = currentMap;
			currentMap = (int)mapID;

			mapDatabase[oldMap].Disable();
			mapDatabase[currentMap].Enable();

		}
	}
}