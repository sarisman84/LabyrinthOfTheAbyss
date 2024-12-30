using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;

public class SpriteAtlasImporter : ScriptableWizard
{
	public class Frame
	{
		[JsonProperty("filename")]
		public string Filename { get; set; }

		[JsonProperty("frame")]
		public Rectangle FrameData { get; set; }

		[JsonProperty("rotated")]
		public bool Rotated { get; set; }

		[JsonProperty("trimmed")]
		public bool Trimmed { get; set; }

		[JsonProperty("spriteSourceSize")]
		public Rectangle SpriteSourceSize { get; set; }

		[JsonProperty("sourceSize")]
		public Size SourceSize { get; set; }
	}

	public class Rectangle
	{
		[JsonProperty("x")]
		public int X { get; set; }

		[JsonProperty("y")]
		public int Y { get; set; }

		[JsonProperty("w")]
		public int Width { get; set; }

		[JsonProperty("h")]
		public int Height { get; set; }
	}

	public class Size
	{
		[JsonProperty("w")]
		public int Width { get; set; }

		[JsonProperty("h")]
		public int Height { get; set; }
	}

	public class Meta
	{
		[JsonProperty("app")]
		public string App { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; }

		[JsonProperty("image")]
		public string Image { get; set; }

		[JsonProperty("format")]
		public string Format { get; set; }

		[JsonProperty("size")]
		public Size Size { get; set; }

		[JsonProperty("scale")]
		public string Scale { get; set; }
	}

	public class Root
	{
		[JsonProperty("frames")]
		public List<Frame> Frames { get; set; }

		[JsonProperty("meta")]
		public Meta Meta { get; set; }
	}

	private Button browseButton;
	private TextField filePathField;
	private Label resultLabel;

	public SpriteAsset assetToUpdate;

	[MenuItem("Tools/LOTA/Sprite Asset/Atlas Importer")]
	public static void ShowEditorWindow()
	{
		DisplayWizard<SpriteAtlasImporter>("Atlas Importer, Import Json Array");
	}

	void OnWizardUpdate()
    {
        helpString = assetToUpdate == null ? "Please select a sprite asset to update!" : assetToUpdate.name;
    }

	private void OnWizardCreate()
	{
		// Open File Picker
		string path = EditorUtility.OpenFilePanel("Select JSON File", Application.dataPath, "json");
		if (string.IsNullOrEmpty(path))
		{
			return;
		}
		var result = ParseJsonFile(path);

		if (result == null)
		{
			return;
		}

		for (int i = 0; i < assetToUpdate.spriteCharacterTable.Count; ++i)
		{
			var character = assetToUpdate.spriteCharacterTable[i];
			character.name = result.Frames[i].Filename.Replace(".png", "");
			assetToUpdate.spriteCharacterTable[i] = character;
		}

		assetToUpdate.UpdateLookupTables();
		Debug.Log($"Updated Sprite Asset {assetToUpdate.name}!");
	}

	private Root ParseJsonFile(string path)
	{
		try
		{
			string jsonContent = File.ReadAllText(path);
			return JsonConvert.DeserializeObject<Root>(jsonContent);
		}
		catch (System.Exception e)
		{
			Debug.LogError($"Failed to parse JSON: {e.Message}");
			resultLabel.text = "Error: Unable to parse the JSON file.";
		}

		return null;
	}
}