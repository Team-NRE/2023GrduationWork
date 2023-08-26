using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;
using Stat;
using Photon.Pun;

public class GameScene : BaseScene
{
	public int test_attackerID;
	public int test_deadUserID;
	public bool test_isKillEvent = false;

	private void Update()
	{
		if (test_isKillEvent)
		{
			Managers.game.killEvent(test_attackerID, test_deadUserID);
			test_isKillEvent = false;
		}	
	}


	protected override void Init()
	{
		Debug.Log("Inst");
		SceneType = Define.Scene.Game;
		Managers.UI.ShowSceneUI<UI_Mana>();
		Managers.UI.ShowSceneUI<UI_CardPanel>();
		Managers.UI.ShowSceneUI<UI_Popup>();

		StartCoroutine("ForStupidPhoton");
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

		/// 시작 시간 초기화 : Master Client만
		if (PhotonNetwork.IsMasterClient) 
		{
			Managers.game.startTime = PhotonNetwork.Time;

			GetComponent<PhotonView>().RPC(
				"SyncPlayTime",
				RpcTarget.Others,
				Managers.game.startTime
			);
		}

		Debug.Log("Instantiate Player");

		// player summon
		Vector3 spawnPos = GameObject.Find("CyborgRespawn").transform.position;

		Managers.game.myCharacter = PhotonNetwork.Instantiate(
			$"Prefabs/InGame/Player/{UI_Select._name}", 
			spawnPos, 
			Quaternion.identity
		);

		GetComponent<PhotonView>().RPC(
			"SetTeamCharacter",
			RpcTarget.Others,
			Managers.game.myCharacter.GetComponent<PhotonView>().ViewID
		);
	}

	[PunRPC]
	public void SyncPlayTime(double time)
	{
		Managers.game.startTime = time;
	}

	[PunRPC]
	public void SyncPlayerCharacter((int, int) humanPlayerViewID, (int, int) cyborgPlayerViewID)
	{
		Managers.game.humanTeamCharacter = (
			PhotonView.Find(humanPlayerViewID.Item1), 
			PhotonView.Find(humanPlayerViewID.Item2)
		);

		Managers.game.cyborgTeamCharacter = (
			PhotonView.Find(humanPlayerViewID.Item1), 
			PhotonView.Find(humanPlayerViewID.Item2)
		);
	}

	[PunRPC]
	public void KillEvent(int attackerID, int deadUserID)
	{
		GameObject attacker = PhotonView.Find(attackerID)?.gameObject;
		GameObject deadUser = PhotonView.Find(deadUserID)?.gameObject;

		// 예외 처리
		if (attacker == null) return;
		if (deadUser == null) return;

		// 가해자 처리
		if (attacker.tag == "PLAYER")
		{
			if (attacker.layer == LayerMask.NameToLayer("Human")) Managers.game.humanTeamKill++;
			if (attacker.layer == LayerMask.NameToLayer("Cyborg")) Managers.game.cyborgTeamKill++;

			attacker.GetComponent<PlayerStats>().kill++;
		}

		// 피해자 처리
		deadUser.GetComponent<PlayerStats>().death++;

		/// UI Event
		UI_KillLog killLog = Managers.UI.MakeSubItem<UI_KillLog>
			(Managers.UI.Root.transform.Find("UI_Popup/UI_KillLog"), "KillLog");
		
		killLog.Init(
			attacker,
			deadUser
		);
	}
}
