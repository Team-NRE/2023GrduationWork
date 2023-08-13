using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Lobby : UI_Scene
{
	private string roomCode;
	public TMP_InputField roomCodeIF;

	public enum Buttons
	{
		EnterSingle,
		EnterMulti,
	}

	public override void Init()
	{
		// ��� UI ��ü ���ε�
		Bind<Button>(typeof(Buttons));

		// ĳ���� ��ư Ŭ���� ���� ���� ǥ��
		GetButton((int)Buttons.EnterSingle).gameObject.BindEvent(EnterSingle);
		GetButton((int)Buttons.EnterMulti).gameObject.BindEvent(EnterMulti);
	}

	// Select Button Ŭ���� �߻��� �̺�Ʈ
	public void EnterSingle(PointerEventData data)
	{
		Debug.Log("EnterSingle");
		// 1. ������ ĳ���͸� ���� ��(GameScene)���� �ѱ��.
		SceneManager.LoadScene("Select");
	}

	// Select Button Ŭ���� �߻��� �̺�Ʈ
	public void EnterMulti(PointerEventData data)
	{
		Debug.Log("EnterMulti");
		// 1. ������ ĳ���͸� ���� ��(GameScene)���� �ѱ��.
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
			SceneManager.LoadScene("Select");
		Debug.Log($"your room code is {name}");
	}
}
