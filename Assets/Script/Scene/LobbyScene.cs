using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
	void Start()
    {
        Managers.UI.ShowSceneUI<UI_Lobby>();
    }

    public override void Clear()
    {

    }
}
