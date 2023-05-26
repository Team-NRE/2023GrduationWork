using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
	protected override void Init()
	{
		SceneType = Define.Scene.Game;
		Managers.UI.ShowSceneUI<UI_CardPanel>();
	}
	

	public override void Clear()
	{
		
	}
}
