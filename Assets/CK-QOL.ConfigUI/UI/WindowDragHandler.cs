using UnityEngine;
using UnityEngine.EventSystems;

namespace CK_QOL.ConfigUI.UI
{
	public class WindowDragHandler : MonoBehaviour, IDragHandler
	{
		[SerializeField] private RectTransform rectTransform;

		private Canvas _canvas;

		public void OnDrag(PointerEventData eventData)
		{
			rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
		}

		public void Initialize(Canvas canvas)
		{
			_canvas = canvas;
		}
	}
}