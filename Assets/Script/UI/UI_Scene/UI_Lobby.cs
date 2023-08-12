using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Lobby : UI_Scene
{
	public static string _name;

	public enum Pages
	{
		ModePage,
		SelectPage,
	}

	public enum Selectors
	{
		Police,
		FireFighter,
		LightSabre,
		Monk,
	}

	public enum Buttons
	{
		EnterSingle,
		EnterMulti,
	}


	public override void Init()
	{
		// 모든 UI 객체 바인딩
		Bind<GameObject>(typeof(Selectors));
		Bind<Button>(typeof(Buttons));

		// 캐릭터 버튼 클릭에 따른 스팟 표시
		GetButton((int)Buttons.EnterSingle).gameObject.BindEvent(EnterSingle);
		GetButton((int)Buttons.EnterMulti).gameObject.BindEvent(EnterMulti);

		Get<GameObject>((int)Selectors.Police).gameObject.BindEvent(SpotOnPolice);
		Get<GameObject>((int)Selectors.FireFighter).gameObject.BindEvent(SpotOnFireFighter);
		Get<GameObject>((int)Selectors.LightSabre).gameObject.BindEvent(SpotOnFireLightSabre);
		Get<GameObject>((int)Selectors.Monk).gameObject.BindEvent(SpotOnFireMonk);
	}

	// Select Button 클릭시 발생할 이벤트
	public void EnterSingle(PointerEventData data)
	{
		Debug.Log("EnterSingle");
		// 1. 선택한 캐릭터를 다음 씬(GameScene)으로 넘긴다.
		//SceneManager.LoadScene("View Test Scene");

		GameObject page = Get<GameObject>((int)Pages.SelectPage);
		page.SetActive(true);
	}

	// Select Button 클릭시 발생할 이벤트
	public void EnterMulti(PointerEventData data)
	{
		Debug.Log("EnterMulti");
		// 1. 선택한 캐릭터를 다음 씬(GameScene)으로 넘긴다.
		SceneManager.LoadScene("View Test Scene");
	}

	public void GetSpot()
	{

	}

	public void SetName()
	{

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
}
