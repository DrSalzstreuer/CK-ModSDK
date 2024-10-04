using UnityEngine;
using UnityEngine.EventSystems;

namespace CK_QOL.UI
{
	public class WindowDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler
	{
		private Vector2 _pointerOffset;
		private RectTransform _canvasRectTransform;
		private RectTransform _windowRectTransform;

		private void Awake()
		{
			Initialize();
		}

		public void Initialize()
		{
			var canvas = GetComponentInParent<Canvas>();
			if (canvas != null)
			{
				_canvasRectTransform = canvas.GetComponent<RectTransform>();
			}

			_windowRectTransform = transform.parent.GetComponent<RectTransform>();
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(_windowRectTransform, eventData.position, eventData.pressEventCamera, out _pointerOffset);
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (_windowRectTransform == null)
			{
				return;
			}

			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, eventData.position, eventData.pressEventCamera, out var localPointerPosition))
			{
				return;
			}

			var offsetPosition = localPointerPosition - _pointerOffset;
			_windowRectTransform.localPosition = offsetPosition;
		}
	}
}