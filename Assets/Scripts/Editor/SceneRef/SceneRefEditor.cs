using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using lota.systemic;
using System.Linq;
using System;
using TPUModelerEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;

namespace lota.editor
{
	[CustomPropertyDrawer(typeof(SceneRef))]
	public class SceneRefEditor : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = new VisualElement();
			root.style.flexDirection = FlexDirection.Row;
			root.Add(ScenePickerField(property));
			if (IsFieldPopulated(property))
				root.Add(ProjectBuildSceneToggle(property));
			return root;
		}

		private bool IsFieldPopulated(SerializedProperty property)
		{
			var path = property.FindPropertyRelative("scenePath");
			return !string.IsNullOrEmpty(path.stringValue);
		}

		private VisualElement ProjectBuildSceneToggle(SerializedProperty property)
		{
			var button = new Toggle("Built");
			var builtScenes = EditorBuildSettings.scenes.ToList();
			var scenePathProperty = property.FindPropertyRelative("scenePath");
			var scenePath = scenePathProperty.stringValue;

			button.value = builtScenes.FindIndex(s => s.path.ToLower() == scenePath.ToLower()) != -1;
			button.RegisterCallback<ChangeEvent<bool>>((evt) => { ToggleBuildSceneState(evt, scenePathProperty, builtScenes); });
			return button;
		}

		private void ToggleBuildSceneState(ChangeEvent<bool> evt, SerializedProperty property, List<EditorBuildSettingsScene> builtScenes)
		{
			var scenePath = property.stringValue;
			if (!evt.newValue)
			{
				builtScenes.RemoveAt(builtScenes.FindIndex(s => s.path.ToLower() == scenePath.ToLower()));
			}
			else
			{
				builtScenes.Add(new EditorBuildSettingsScene(scenePath, true));
			}

			EditorBuildSettings.scenes = builtScenes.ToArray();

		}

		private VisualElement ScenePickerField(SerializedProperty property)
		{
			var scenePath = property.FindPropertyRelative("scenePath");
			var field = new ObjectField("Scene");
			field.objectType = typeof(SceneAsset);
			field.Bind(property.serializedObject);
			field.style.minWidth = new Length(95.0f, LengthUnit.Percent);
			if (!string.IsNullOrEmpty(scenePath.stringValue))
				field.value = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath.stringValue);
			field.RegisterValueChangedCallback((evt) => { UpdateData(evt, scenePath); });
			return field;

		}

		private void UpdateData(ChangeEvent<UnityEngine.Object> evt, SerializedProperty property)
		{
			if (!evt.newValue)
			{
				property.stringValue = "";
				property.serializedObject.ApplyModifiedProperties();
				return;
			}


			SceneAsset asset = evt.newValue as SceneAsset;
			property.stringValue = AssetDatabase.GetAssetPath(asset);
			property.serializedObject.ApplyModifiedProperties();
		}
	}
}

