using lota.systemic.ui;
using Spyro;
using UnityEngine;
using UnityEngine.UIElements;

namespace lota.gameplay
{
	[RequireComponent(typeof(UIDocument))]
	public class DocumentController : MonoBehaviour
	{
		private UIDocument document;

		private void Awake()
		{
			document = GetComponent<UIDocument>();
			ServiceLocator<UIService>.Service.RegisterUIDocument(document);
		}
	}
}
