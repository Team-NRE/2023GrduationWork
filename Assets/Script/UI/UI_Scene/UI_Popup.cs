using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AllIn1VfxToolkit.Demo.Scripts;

public class UI_Popup : UI_Scene
{
    enum Popup
    {
        UI_KDA,
        UI_KillLog,
        UI_StatBox,
        UI_StautsBar,
        UI_Scoreboard,
        UI_Store,
        UI_Deck,
        UI_Setting,
    }

    public override void Init()
    {
        Bind<AllIn1CanvasFader>(typeof(Popup));

        Get<AllIn1CanvasFader>((int)Popup.UI_KDA)       .gameObject.SetActive(true);
        Get<AllIn1CanvasFader>((int)Popup.UI_KillLog)   .gameObject.SetActive(true);
        Get<AllIn1CanvasFader>((int)Popup.UI_StatBox)   .gameObject.SetActive(true);
        Get<AllIn1CanvasFader>((int)Popup.UI_StautsBar) .gameObject.SetActive(true);
        Get<AllIn1CanvasFader>((int)Popup.UI_Scoreboard).gameObject.SetActive(false);
        Get<AllIn1CanvasFader>((int)Popup.UI_Store)     .gameObject.SetActive(false);
        Get<AllIn1CanvasFader>((int)Popup.UI_Deck)      .gameObject.SetActive(false);
        Get<AllIn1CanvasFader>((int)Popup.UI_Setting)   .gameObject.SetActive(false);
    }

    public override void UpdateInit() 
    {
        KeyDownAction();
    }

    public void KeyDownAction()
    {
        if (Input.GetKeyDown(KeyCode.P))      { Get<AllIn1CanvasFader>((int)Popup.UI_Store)     .TurnUI(); }
        if (Input.GetKeyDown(KeyCode.Tab))    { Get<AllIn1CanvasFader>((int)Popup.UI_Scoreboard).gameObject.SetActive(true); }
		if (Input.GetKeyUp(KeyCode.Tab))      { Get<AllIn1CanvasFader>((int)Popup.UI_Scoreboard).gameObject.SetActive(false); }
        if (Input.GetKeyDown(KeyCode.Escape)) { Get<AllIn1CanvasFader>((int)Popup.UI_Setting)   .TurnUI(); }
        if (Input.GetKeyDown(KeyCode.I))      { Get<AllIn1CanvasFader>((int)Popup.UI_Deck)      .TurnUI(); }
    }

}
