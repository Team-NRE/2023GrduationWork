using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using AllIn1VfxToolkit.Demo.Scripts;

public class UI_Lobby : UI_Scene
{
	public UI_Select characterSelectUI;
	public AllIn1CanvasFader characterSelectUIFader;

	public enum Buttons
	{
		EnterSingle,
		EnterMulti_1vs1,
		EnterMulti_2vs2
	}

	public override void Init()
	{
		characterSelectUI = GameObject.FindObjectOfType<UI_Select>();
		characterSelectUIFader = characterSelectUI.GetComponent<AllIn1CanvasFader>();
		characterSelectUIFader.gameObject.SetActive(false);

		// 모든 UI 객체 바인딩
		Bind<Button>(typeof(Buttons));

		// 캐릭터 버튼 클릭에 따른 스팟 표시
		GetButton((int)Buttons.EnterSingle).gameObject.BindEvent(EnterSingle);
		GetButton((int)Buttons.EnterMulti_1vs1).gameObject.BindEvent(EnterMulti_1vs1);
		GetButton((int)Buttons.EnterMulti_2vs2).gameObject.BindEvent(EnterMulti_2vs2);
	}

	// Select Button 클릭시 발생할 이벤트
	public void EnterSingle(PointerEventData data)
	{
		Debug.Log("EnterSingle");
		Managers.game.gameMode = Define.GameMode.Single;
		characterSelectUIFader.HideUiButtonPressed();
	}

	// Select Button 클릭시 발생할 이벤트
	public void EnterMulti_1vs1(PointerEventData data)
	{
		Debug.Log("EnterMulti_1vs1");
		Managers.game.gameMode = Define.GameMode.Multi_1vs1;
		characterSelectUIFader.HideUiButtonPressed();
	}

	// Select Button 클릭시 발생할 이벤트
	public void EnterMulti_2vs2(PointerEventData data)
	{
		Debug.Log("EnterMulti_2vs2");
		Managers.game.gameMode = Define.GameMode.Multi_2vs2;
		characterSelectUIFader.HideUiButtonPressed();
	}
}
