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
            StringBuilder keybindEnum = new StringBuilder();
            StringBuilder mapEnum = new StringBuilder();
			keybindEnum.Append("public enum InputActionID {");
			mapEnum.Append("public enum InputActionMapID {");
			for (int i = 0; i < KeybindAsset.actionMaps.Count; i++)
			{
                InputActionMap actionMap = KeybindAsset.actionMaps[i];
				mapEnum.Append($"{actionMap.name}{(i == KeybindAsset.actionMaps.Count - 1 ? "" : ",")}");
				for (int j = 0; j < actionMap.actions.Count; j++)
				{
                    InputAction action = actionMap.actions[j];
					keybindEnum.Append($"{actionMap.name}_{action.name}{(j == actionMap.actions.Count - 1 ? "" : ",")}");
				}

			}
			keybindEnum.Append("}");
			mapEnum.Append("}");

            StringBuilder fileContent = new StringBuilder();
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
            int count = 0;

			for (int i = 0; i < KeybindAsset.actionMaps.Count; i++)
			{
                InputActionMap actionMap = KeybindAsset.actionMaps[i];
				mapDatabase.Add(i, actionMap);
				foreach (InputAction action in actionMap.actions)
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
            int key = (int)actionID;
            bool result = keybindDatabase[key].ReadValue<float>() > 0 && keybindDatabase[key].triggered;

			return result;
		}

		public bool IsActionHeld(InputActionID actionID)
		{
            int key = (int)actionID;
            bool result = keybindDatabase[key].ReadValue<float>() > 0;

			return result;
		}

		public Vector2 GetActionAxis(InputActionID actionID)
		{
            InputAction action = keybindDatabase[(int)actionID];
			return action.ReadValue<Vector2>();
		}

		public void SwitchControlActionMap(InputActionMapID mapID)
		{
            int oldMap = currentMap;
			currentMap = (int)mapID;

			mapDatabase[oldMap].Disable();
			mapDatabase[currentMap].Enable();

		}

		internal bool IsAxisActionPressed(InputActionID player_Move)
		{
			throw new NotImplementedException();
		}
	}
}