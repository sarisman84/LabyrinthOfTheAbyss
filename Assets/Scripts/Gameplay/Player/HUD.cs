using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace lota.gameplay.player
{

	[RequireComponent(typeof(UIDocument))]
	public class HUD : MonoBehaviour
	{
		private UIDocument document;
		private VisualElement root;

		private void Awake()
		{
			document = GetComponent<UIDocument>();
			root = document.rootVisualElement;
		}
		public void RegisterCallback<TEventType>(string idName, EventCallback<TEventType> evt) where TEventType : EventBase<TEventType>, new()
		{
			root.Q(idName).RegisterCallback(evt);
		}
		public void Display(string idName, Action<VisualElement> evt = null)
		{
			var element = root.Q(idName);
			element.visible = true;
			evt?.Invoke(element);
		}


		public void Hide(string idName)
		{
			root.Q(idName).visible = false;
		}
	}
}
