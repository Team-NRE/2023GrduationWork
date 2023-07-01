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
	InputField input;
	InputField tc;
	public InputField user;
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
		GameObject go = GetButton((int)LoginButtons.Login).gameObject;
		GetButton((int)LoginButtons.Login).gameObject.BindEvent(LoginClick);
	} 

	public void LoginClick(PointerEventData data)
	{
		_inputUser = user.text;
		PhotonNetwork.NickName = user.text;
		Debug.Log(user.text);
		InitialRoom();
	}

	public void InitialRoom(string name = "default")
	{
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 10;
		roomOptions.IsVisible = true;
		roomOptions.IsOpen = true;
		//PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
		if (PhotonNetwork.JoinOrCreateRoom(name, roomOptions, TypedLobby.Default) == true)
			SceneManager.LoadScene("View Test Scene");
		Debug.Log($"your room code is {name}");
	}
}
