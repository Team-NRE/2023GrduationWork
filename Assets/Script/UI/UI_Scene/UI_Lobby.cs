using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using AllIn1VfxToolkit.Demo.Scripts;

public class UI_Lobby : UI_Scene
{
	private string roomCode;
	public TMP_InputField roomCodeIF;
	public UI_Select characterSelectUI;
	public AllIn1CanvasFader characterSelectUIFader;

	public enum Buttons
	{
		EnterSingle,
		EnterMulti,
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
		GetButton((int)Buttons.EnterMulti).gameObject.BindEvent(EnterMulti);
	}

	// Select Button 클릭시 발생할 이벤트
	public void EnterSingle(PointerEventData data)
	{
		Debug.Log("EnterSingle");
		characterSelectUI.gameMode = Define.GameMode.Single;
		characterSelectUIFader.HideUiButtonPressed();
	}

	// Select Button 클릭시 발생할 이벤트
	public void EnterMulti(PointerEventData data)
	{
		Debug.Log("EnterMulti");
		characterSelectUI.gameMode = Define.GameMode.Multi;
		characterSelectUIFader.HideUiButtonPressed();
	}
}
