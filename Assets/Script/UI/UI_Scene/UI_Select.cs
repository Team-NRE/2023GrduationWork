using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_Select : UI_Scene
{
	public static string _name;

	public enum Selectors
	{
		Police,
		FireFighter,
		LightSabre,
		Monk,
	}

	public enum Buttons
	{
		Select,
	}

	void Start()
    {
        
    }

	public override void Init()
	{
		Bind<GameObject>(typeof(Selectors));

		Get<GameObject>((int)Selectors.Police).gameObject.BindEvent(SpotOnPolice);
		Get<GameObject>((int)Selectors.FireFighter).gameObject.BindEvent(SpotOnFireFighter);
		Get<GameObject>((int)Selectors.LightSabre).gameObject.BindEvent(SpotOnFireLightSabre);
		Get<GameObject>((int)Selectors.Monk).gameObject.BindEvent(SpotOnFireMonk);
	}

	// 캐릭터 선택시 스팟이 켜지는 부분
	public void SpotOnPolice(PointerEventData data)
	{
		string name = Get<GameObject>((int)Selectors.Police).gameObject.name;
		// 1. 클릭할 때 마다 스팟을 띄운다
		Debug.Log(name);
		// 2. 클릭 할 때 마다 string 객체에 버튼 이름을 저장한다.
		_name = name;
		Debug.Log($"MemberName : {_name}");
	}

	public void SpotOnFireFighter(PointerEventData data)
	{
		string name = Get<GameObject>((int)Selectors.FireFighter).gameObject.name;
		Debug.Log(name);
		_name = name;
		Debug.Log($"MemberName : {_name}");

	}

	public void SpotOnFireLightSabre(PointerEventData data)
	{
		string name = Get<GameObject>((int)Selectors.LightSabre).gameObject.name;
		Debug.Log(name);
		_name = name;
		Debug.Log($"MemberName : {_name}");
	}

	public void SpotOnFireMonk(PointerEventData data)
	{
		string name = Get<GameObject>((int)Selectors.Monk).gameObject.name;
		Debug.Log(name);
		_name = name;
		Debug.Log($"MemberName : {_name}");
	}

	public void SelectButton(PointerEventData data)
	{
		Debug.Log("Start Game");
		// 1. 선택한 캐릭터를 다음 씬(GameScene)으로 넘긴다.
		SceneManager.LoadScene("View Test Scene");
	}
}
