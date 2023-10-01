using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{


	// Start is called before the first frame update
	void Start()
    {
		Managers.UI.ShowSceneUI<UI_Select>();
        Managers.UI.ShowSceneUI<UI_Lobby>();
		Managers.UI.ShowSceneUI<UI_Setting>();
    }
	public override void Clear()
	{
		throw new System.NotImplementedException();
	}
}
