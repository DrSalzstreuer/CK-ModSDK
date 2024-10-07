using UnityEngine;
using UnityEngine.UI;

namespace CK_QOL.ConfigUI.UI
{
	public class CollapsiblePanel : MonoBehaviour
	{
		[Header("Collapsible Panel")]
		public Button headerButton;
		public Text buttonText;
		public RectTransform content;

		private ConfigUI _configUI;
		
		private const string ExpandedIcon = "▲";
		private const string CollapsedIcon = "▼";

		private bool _isExpanded;

		private void Awake()
		{
			_configUI = FindObjectOfType<ConfigUI>();
		}

		private void Start()
		{
			_isExpanded = false;
			headerButton.onClick.AddListener(ToggleContent);
			UpdateContentVisibility();
		}

		public void ToggleContent()
		{
			_isExpanded = !_isExpanded;
			UpdateContentVisibility();
		}

		private void UpdateContentVisibility()
		{
			content.gameObject.SetActive(_isExpanded);
			buttonText.text = _isExpanded 
				? ExpandedIcon 
				: CollapsedIcon;
			
			StartCoroutine(_configUI.RebuildLayout());
		}
	}
}