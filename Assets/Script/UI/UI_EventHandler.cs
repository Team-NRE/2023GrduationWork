using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//두 인터페이스는 하위 항목에도 모두 적용된다.
public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
	public Action<PointerEventData> OnClickHandler = null;
	public Action<PointerEventData> OnDragHandler = null;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (OnClickHandler != null)
			OnClickHandler.Invoke(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (OnDragHandler != null)
			OnDragHandler.Invoke(eventData);
	}
}
