using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.TextCore.Text;
using System;
using Unity.VisualScripting;
using UnityEngine.TextCore;


public class SpriteAssetUpdater : ScriptableWizard
{
	public Vector2 glyphRect;
	public Vector2 glyphBearings;
	public float glyphAdvance;

	[MenuItem("Tools/LOTA/Sprite Asset/Update Sprite Assets")]
	public static void UpdateSpriteAssets()
	{

		var ins = DisplayWizard<SpriteAssetUpdater>("Sprite Asset Updater", "Apply Changes", "Cancel");
		// var objs = Selection.objects;

		// foreach(var selectedObject in objs)
		// {
		// 	if(selectedObject.GetType() != typeof(SpriteAsset))
		// 	{
		// 		continue;
		// 	}

		// 	var spriteAsset = selectedObject as SpriteAsset;

		// 	spriteAsset.();
		// }
	}




	private void OnWizardCreate()
	{
		var selection = Selection.objects;
		foreach (var obj in selection)
		{
			if (obj is not SpriteAsset)
			{
				continue;
			}

			SpriteAsset asset = obj as SpriteAsset;

			for (int i = 0; i < asset.spriteGlyphTable.Count; i++)
			{
				SpriteGlyph glyph = asset.spriteGlyphTable[i];
				glyph.metrics = new GlyphMetrics(glyphRect.x, glyphRect.y, glyphBearings.x, glyphBearings.y, glyphAdvance);
				asset.spriteGlyphTable[i] = glyph;

				Debug.Log("updated glyph");
			}
			asset.UpdateLookupTables();
		}
	}

}


