using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;

public abstract class UI_Base : MonoBehaviourPunCallbacks
{
	Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>(); //타입별로 따로 저장되는 딕셔너리 선언

	public abstract void Init();
	public abstract void UpdateInit();

	private void Start()
	{
		Init();
	}

	private void Update()
	{
		UpdateInit();	
	}


	//Reflection을 이용한다. 하위 컴포넌트들을 모두 찾아주는 단계
	protected void Bind<T>(Type type) where T : UnityEngine.Object    //하위 함수에서와 조건을 맞춰준다.
	{
		string[] names = Enum.GetNames(type);

		UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
		_objects.Add(typeof(T), objects);
		for (int i = 0; i < names.Length; i++)
		{
			if (typeof(T) == typeof(GameObject))
				objects[i] = Util.FindChild(gameObject, names[i], true);
			else
				objects[i] = Util.FindChild<T>(gameObject, names[i], true);

			if (objects[i] == null)
				Debug.Log($"Failed to bind({names[i]})");
		}
	}


	protected T Get<T>(int idx) where T : UnityEngine.Object
	{
		UnityEngine.Object[] objects = null;
		if (_objects.TryGetValue(typeof(T), out objects) == false)
			return null;

		return objects[idx] as T;
	}


	//편의를 위해서 입력을 줄이기 위한 함수들
	protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
	protected Text GetText(int idx) { return Get<Text>(idx); }
	protected Button GetButton(int idx) { return Get<Button>(idx); }
	protected Image GetImage(int idx) { return Get<Image>(idx); }


	public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
	{
		UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

		switch (type)
		{
			case Define.UIEvent.Click:
				evt.OnClickHandler -= action;
				evt.OnClickHandler += action;
				break;
			case Define.UIEvent.Drag:
				evt.OnDragHandler -= action;
				evt.OnDragHandler += action;
				break;
		}
	}
}
