using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
	protected override void Init()
	{
		SceneType = Define.Scene.Game;
		Managers.Resource.Instantiate("UI_Card");
		Managers.UI.ShowSceneUI<UI_CardPanel>();
		//Managers.UI.ShowSceneUI<UI_NextCard>();
	}
	

	public override void Clear()
	{
		
	}
}
