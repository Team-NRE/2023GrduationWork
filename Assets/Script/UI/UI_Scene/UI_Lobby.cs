using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Lobby : UI_Scene
{
	public enum Buttons
	{
		EnterSingle,
		EnterMulti,
	}


	public override void Init()
	{
		// 모든 UI 객체 바인딩
		Bind<Button>(typeof(Buttons));

		// 캐릭터 버튼 클릭에 따른 스팟 표시
		GetButton((int)Buttons.EnterSingle).gameObject.BindEvent(EnterSingle);
		GetButton((int)Buttons.EnterMulti).gameObject.BindEvent(EnterMulti);
	}

	// Select Button 클릭시 발생할 이벤트
	public void EnterSingle(PointerEventData data)
	{
		Debug.Log("EnterSingle");
		// 1. 선택한 캐릭터를 다음 씬(GameScene)으로 넘긴다.
		SceneManager.LoadScene("Select");
	}

	// Select Button 클릭시 발생할 이벤트
	public void EnterMulti(PointerEventData data)
	{
		Debug.Log("EnterMulti");
		// 1. 선택한 캐릭터를 다음 씬(GameScene)으로 넘긴다.
		SceneManager.LoadScene("Select");
	}
}
