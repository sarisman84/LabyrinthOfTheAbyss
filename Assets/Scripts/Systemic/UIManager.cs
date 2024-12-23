
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;
using MSDebug = System.Diagnostics.Debug;
using Debug = UnityEngine.Debug;
using System.Text.RegularExpressions;
using Spyro;
using lota.generated.input;

namespace lota.systemic.ui
{
	public delegate string MessageEvent(string argument);

	public class UIService
	{
		private VisualElement root;
		private Label hudInteractionMessage;
		private Dictionary<string, MessageEvent> keywordRegistry;


		private Label HUDInteractionMessage
		{
			get
			{
				if (hudInteractionMessage == null && root != null)
				{
					hudInteractionMessage = root.Q<Label>("hud_interact_desc");
				}

				return hudInteractionMessage;
			}
		}

		public UIService()
		{
			keywordRegistry = new Dictionary<string, MessageEvent>();
		}




		public void RegisterUIDocument(UIDocument document)
		{
			MSDebug.Assert(document != null, "Argument is null");
			root = document.rootVisualElement;
			HideInteractionMessage();
		}

		public void AddKeyword(string keyword, MessageEvent effect)
		{
			keywordRegistry.Add(keyword, effect);
		}

		public void ShowInteractionMessage(string newMessage)
		{
			HUDInteractionMessage.style.display = DisplayStyle.Flex;
			HUDInteractionMessage.text = ParseMessage(newMessage);

		}

		public void HideInteractionMessage()
		{
			HUDInteractionMessage.style.display = DisplayStyle.None;
		}

		private string ParseMessage(string newMessage)
		{
			string result = newMessage;
			const string spriteKeywordFilter = @"\<([a-zA-Z0-9_]+):([^\}]+)\>";
			Regex spriteKeyword = new Regex(spriteKeywordFilter);
			MatchCollection foundKeywords = spriteKeyword.Matches(newMessage);

			foreach (Match match in foundKeywords)
			{
				string keyword = match.Groups[1].Value;
				string argument = match.Groups[2].Value;

				result = $"{result.Replace(match.Value, keywordRegistry[keyword].Invoke(argument))}";
			}

			return result;
		}
	}
}