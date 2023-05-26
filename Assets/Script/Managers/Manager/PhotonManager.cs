using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
	public TMP_InputField roomCodeIF;
	void Start()
	{
		ConnectToServer();
		DontDestroyOnLoad(this.gameObject);
	}

	void ConnectToServer()
	{
		PhotonNetwork.ConnectUsingSettings();
		Debug.Log("Trying to connect to the server...");
	}
	/*
	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to the server!");
		base.OnConnectedToMaster();
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 10;
		roomOptions.IsVisible = true;
		roomOptions.IsOpen = true;
		//PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
		PhotonNetwork.JoinOrCreateRoom(roomCodeIF.text, roomOptions, TypedLobby.Default);
		Debug.Log(roomCodeIF.text);
	}
	*/
	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to the server!");
		base.OnConnectedToMaster();
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Joined a room!");
		base.OnJoinedRoom();
		SceneManager.LoadScene("Content");
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Debug.Log("A new player has entered the room!");
		base.OnPlayerEnteredRoom(newPlayer);
	}

	public void InitRoom()
	{
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 10;
		roomOptions.IsVisible = true;
		roomOptions.IsOpen = true;
		//PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
		PhotonNetwork.JoinOrCreateRoom(roomCodeIF.text, roomOptions, TypedLobby.Default);
		Debug.Log($"your room code is {roomCodeIF.text}");
	}
}
