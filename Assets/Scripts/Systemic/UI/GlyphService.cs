

using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace lota.systemic.ui
{
	public class GlyphService
	{
		public const string InputAtlas = "inputatlas";
		public const string InputIconsPath = "Input Icons/";
		public const string RootPath = "Glyphs/";

		public List<SpriteCharacter> Characters => inputAtlas.spriteCharacterTable;
		private static SpriteAsset inputAtlas { get; } = Resources.Load<SpriteAsset>(RootPath + InputIconsPath + InputAtlas);
		private Dictionary<string, Dictionary<string, int>> glyphRegistry;


		public GlyphService()
		{
			glyphRegistry = new Dictionary<string, Dictionary<string, int>>();
			ParseGlyphsToRegistry();
		}

		private void ParseGlyphsToRegistry()
		{
			for (int i = 0; i < Characters.Count; i++)
			{
				SpriteCharacter spriteCharacter = Characters[i];
				var assetName = spriteCharacter.name;
				var data = assetName.Split('_');
				if (data.Length < 2)
				{
					continue;
				}
				var category = data[0].ToLower();
				var key = data[1].ToLower();

				AppendToRegistry(category, key, i);
				Debug.Log($"Registed character {key} in category {category}");
			}

			Debug.Log("Parsed chracters from input atlas");
		}

		private void AppendToRegistry(string category, string key, int glyphIndex)
		{
			if (!glyphRegistry.ContainsKey(category))
			{
				glyphRegistry.Add(category, new Dictionary<string, int>());
			}

			if (!glyphRegistry[category].ContainsKey(key))
			{
				glyphRegistry[category].Add(key, -1);
			}

			glyphRegistry[category][key] = glyphIndex;
		}

		public int GetGlyphIndexByKey(string inputKey)
		{
			var key = ParseInput(inputKey);
			foreach (var (_, keys) in glyphRegistry)
			{
				if (keys.ContainsKey(key))
					return keys[key];
			}
			Debug.LogError($"Could not find key: {key}");
			return -1;
		}

		public int[] GetGlyphIndexesByCategory(string inputCategory)
		{
			var category = inputCategory.ToLower();
			return glyphRegistry[category].Values.ToArray();
		}

		public string ToSpriteTag(int glyphId)
		{
			return $"<sprite=\"{InputIconsPath + InputAtlas}\" name=\"{Characters[glyphId].name}\">";
		}


		private string ParseInput(string inputKey)
		{
			var key = inputKey.ToLower();
			if (key.Contains("control"))
			{
				return "ctrl";
			}

			if (key.Contains("shift"))
			{
				return "shift";
			}

			if (key.Contains("button"))
			{
				return key.Replace("button", "");
			}

			return key;
		}
	}
}