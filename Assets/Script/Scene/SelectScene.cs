using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectScene : BaseScene
{
	private void Start()
	{
		Managers.UI.ShowSceneUI<UI_Select>();
	}

	public override void Clear()
	{
		
	}
}
