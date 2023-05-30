using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class UI_Login : UI_Scene
{
	public enum Logins
	{
		Title,
		RoomCode,
		Login,
	}

	public override void Init()
	{
		Bind<Button>(typeof(Logins));
		GameObject go = GetButton((int)Logins.Login).gameObject;
		GetButton((int)Logins.Login).gameObject.BindEvent(LoginClick);
	} 

	public void LoginClick(PointerEventData data)
	{
		InitialRoom();
		//SceneManager.LoadScene("View Test Scene");
		//if (PhotonNetwork.InRoom)
			SceneManager.LoadScene("View Test Scene");
	}

	public void InitialRoom(string name = "default")
	{
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 10;
		roomOptions.IsVisible = true;
		roomOptions.IsOpen = true;
		//PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
		PhotonNetwork.JoinOrCreateRoom(name, roomOptions, TypedLobby.Default);
		Debug.Log($"your room code is {name}");
	}
}
