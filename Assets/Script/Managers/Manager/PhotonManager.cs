using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Define;

public class PhotonManager : MonoBehaviourPunCallbacks
{
	void Start()
	{
		ConnectToServer();
		DontDestroyOnLoad(this.gameObject);
	}

	public void ConnectToServer()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
		PhotonNetwork.ConnectUsingSettings();
		Debug.Log("Trying to connect to the server...");
	}
	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to the server!");
		//base.OnConnectedToMaster();
		PhotonNetwork.JoinLobby();
	}

	public override void OnJoinedLobby()
	{
		Debug.Log("On Joined Lobby");
		//base.OnJoinedLobby();
	}
	
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Debug.Log("A new player has entered the room!");
		base.OnPlayerEnteredRoom(newPlayer);

		Debug.Log(newPlayer);
		Debug.Log(newPlayer.CustomProperties);

		Debug.Log(newPlayer.NickName);

		Debug.Log("인게임 접속 완료");

		// 다른 클라이언트로 보낼 값들 처리
		if (PhotonNetwork.IsMasterClient)
		{
			
		}
	}
}
