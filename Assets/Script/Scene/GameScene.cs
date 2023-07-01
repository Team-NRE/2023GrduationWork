using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameScene : BaseScene
{
	protected override void Init()
	{
		SceneType = Define.Scene.Game;
		Managers.UI.ShowSceneUI<UI_CardPanel>();
		StartCoroutine("ForStupidPhoton");
		SetRoomInfo();
	}
	
	void LoadObjects()
	{

	}


	public override void Clear()
	{
		
	}

	IEnumerator ForStupidPhoton()
	{
		yield return new WaitForSeconds(2.0f);
		Debug.Log("Instantiate Player");
		PhotonNetwork.Instantiate("PoliceHu", new Vector3(-56, 0, 0), Quaternion.identity);
		PhotonNetwork.Instantiate($"Prefabs/Reference/AI/NeutralMob/NeutralMob", new Vector3(-1.6f, 4.4f, -0.5f), Quaternion.identity);
	}

	void SetRoomInfo()
	{
		Room room = PhotonNetwork.CurrentRoom;
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Joined a room!");
		base.OnJoinedRoom();
		//Debug.Log(PhotonNetwork.InRoom);
		//Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
		foreach(var player in PhotonNetwork.CurrentRoom.Players)
		{
			Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}");
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		SetRoomInfo();
		Debug.Log(newPlayer.NickName + "has joined");
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		SetRoomInfo();
		Debug.Log(otherPlayer.NickName + "has left");
	}
}
