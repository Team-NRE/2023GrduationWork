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
		Managers.UI.ShowSceneUI<UI_Mana>();
		Managers.UI.ShowSceneUI<UI_CardPanel>();
		Managers.UI.ShowSceneUI<UI_Popup>();
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
		PhotonNetwork.Instantiate("Police", new Vector3(-56, 0, 0), Quaternion.identity);
	}
}
