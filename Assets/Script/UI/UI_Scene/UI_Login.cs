using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Login : UI_Scene
{
	TMP_InputField input;
	InputField tc;
	public TMP_InputField user;
	public static string _inputUser;
	public string inputRc;
	public static string inputTc;

	public enum LoginText
	{
		Title,
		RoomCode,
		TeamCode,
	}

	public enum LoginButtons
	{
		Login,
	}

	public enum InputFields
	{
		UserName,
		RoomCode,
		TeamNumber,
	}

	public override void Init()
	{
		Bind<Button>(typeof(LoginButtons));
		Bind<InputField>(typeof(InputFields));
		GameObject go = GetButton((int)LoginButtons.Login).gameObject;
		GetButton((int)LoginButtons.Login).gameObject.BindEvent(LoginClick);

		user.ActivateInputField();
		Managers.Sound.Play("Matching", Define.Sound.Bgm, 1, .05f);
	}

	public override void UpdateInit()
	{
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			LoginClick(null);
		}
	}

	public void LoginClick(PointerEventData data)
	{
		if (string.IsNullOrWhiteSpace(user.text)) 
		{
			Managers.Sound.Play("UI_ButtonFail", Define.Sound.Effect, 1, .5f);
			return;
		}

		Managers.Sound.Play($"UI_ButtonBeep/UI_ButtonBeep_{Random.Range(1, 6)}", Define.Sound.Effect, 1, .5f);

		_inputUser = user.text;
		PhotonNetwork.LocalPlayer.NickName = user.text;
		Managers.game.nickname = user.text;
		Debug.Log(user.text);

		//InitialRoom();
		SceneManager.LoadScene("Lobby");
	}
}
