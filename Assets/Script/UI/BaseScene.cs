using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
	public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;   //get은 아무나 해라, set은 자손클래스만

	void Awake()
	{
		Init();
	}

	protected virtual void Init()
	{
		Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
		if (obj == null)
			Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
	}

	public abstract void Clear();
}
