  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{
	private void Start()
	{
		Managers.UI.ShowSceneUI<UI_Login>();
	}

	public override void Clear()
	{
		
	}
}
