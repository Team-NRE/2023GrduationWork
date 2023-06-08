using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameScene : BaseScene
{
	protected override void Init()
	{
		Debug.Log("Inst");
		SceneType = Define.Scene.Game;
		Managers.UI.ShowSceneUI<UI_CardPanel>();
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
		Debug.Log("Instantiate Player");
		PhotonNetwork.Instantiate("PoliceHu", new Vector3(-56, 0, 0), Quaternion.identity);
		PhotonNetwork.Instantiate($"Prefabs/Reference/AI/NeutralMob/NeutralMob", new Vector3(-1.6f, 4.4f, -0.5f), Quaternion.identity);
	}
}
