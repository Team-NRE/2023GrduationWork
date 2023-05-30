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
	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to the server!");
		//base.OnConnectedToMaster();
	}

	public override void OnJoinedLobby()
	{
		Debug.Log("On Joined Lobby");
		//base.OnJoinedLobby();
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Joined a room!");
		//base.OnJoinedRoom();
		//SceneManager.LoadScene("View Test Scene");
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Debug.Log("A new player has entered the room!");
		base.OnPlayerEnteredRoom(newPlayer);
	}
}
