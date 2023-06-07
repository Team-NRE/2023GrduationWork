using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameScene : BaseScene
{
	protected override void Init()
	{
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
}
