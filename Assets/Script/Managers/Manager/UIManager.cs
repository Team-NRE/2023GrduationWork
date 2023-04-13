using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine;

//Canvas의 sortorder관리가 주 목적
public class UIManager
{
	int _order = 10;    //sorting하지 않는 UI와 겹치는 것을 방지, 제일 뒤로 보내버리기

	Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
	UI_Scene _sceneUI = null;

	public GameObject Root
	{
		get
		{
			GameObject root = GameObject.Find("@UI_Root");
			if (root == null)
				root = new GameObject { name = "@UI_Root" };
			return root;
		}
	}

	public void SetCanvas(GameObject go, bool sort = true)
	{
		Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.overrideSorting = true;

		if (sort)   //sorting을 요청한경우
		{
			canvas.sortingOrder = _order;
			_order++;
		}
		else
		{
			canvas.sortingOrder = 0;
		}
	}

	public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");

		if (parent != null)
			go.transform.SetParent(parent);

		Canvas canvas = go.GetOrAddComponent<Canvas>();
		canvas.renderMode = RenderMode.WorldSpace;
		canvas.worldCamera = Camera.main;

		return Util.GetOrAddComponent<T>(go);
	}

	public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

		if (parent != null)
			go.transform.SetParent(parent);

		return Util.GetOrAddComponent<T>(go);
	}


	public T ShowSceneUI<T>(string name = null) where T : UI_Scene
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
		T sceneUI = Util.GetOrAddComponent<T>(go);    //실수로 스크립트 추가 안해도 넣어드립니다
		_sceneUI = sceneUI;
		//_popupStack.Push(popup);

		go.transform.SetParent(Root.transform);

		return sceneUI;
	}

	//T타입을 사용하고 이름은 옵션으로만 넣어준다.
	public T ShowPopupUI<T>(string name = null) where T : UI_Popup
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
		T popup = Util.GetOrAddComponent<T>(go);    //실수로 스크립트 추가 안해도 넣어드립니다
		_popupStack.Push(popup);

		go.transform.SetParent(Root.transform);

		return popup;
	}

	public void ClosePopupUI(UI_Popup popup)
	{
		if (_popupStack.Count == 0)
			return;

		if (_popupStack.Peek() != popup)
		{
			Debug.Log("Close Popup Failed");
			return;
		}
		ClosePopupUI();
	}

	public void ClosePopupUI()
	{
		if (_popupStack.Count == 0) //스택은 카운트 우선 항상 체크해야한다.
			return;

		UI_Popup popup = _popupStack.Pop();
		Managers.Resource.Destroy(popup.gameObject);
		popup = null;
		_order--;
	}

	public void CloseAllPopupUI()
	{
		while (_popupStack.Count > 0)
			ClosePopupUI();
	}

	public void Clear()
	{
		CloseAllPopupUI();
		_sceneUI = null;
	}
}
