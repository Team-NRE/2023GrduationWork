using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_KeyEvent : UI_Popup
{
    enum Popup
    {
        UI_KDA,
        UI_KillLog,
        UI_StatBox,
        UI_StautsBar,
        UI_Store,
        UI_Deck,
        UI_Scoreboard,
        UI_Setting,
    }

    public override void Init()
    {
        base.Init();
        Bind<UI_CanvasFader>(typeof(Popup));

        Get<UI_CanvasFader>((int)Popup.UI_KDA)       .gameObject.SetActive(true);
        Get<UI_CanvasFader>((int)Popup.UI_KillLog)   .gameObject.SetActive(true);
        Get<UI_CanvasFader>((int)Popup.UI_StatBox)   .gameObject.SetActive(true);
        Get<UI_CanvasFader>((int)Popup.UI_StautsBar) .gameObject.SetActive(true);
        Get<UI_CanvasFader>((int)Popup.UI_Store)     .gameObject.SetActive(false);
        Get<UI_CanvasFader>((int)Popup.UI_Deck)      .gameObject.SetActive(false);
        Get<UI_CanvasFader>((int)Popup.UI_Scoreboard).gameObject.SetActive(false);
        Get<UI_CanvasFader>((int)Popup.UI_Setting)   .gameObject.SetActive(false);
    }

    public override void UpdateInit() 
    {
        KeyDownAction();
    }

    void KeyDownAction()
    {
        if (Input.GetKeyDown(KeyCode.P))      { CloseOtherPopupAndOnPopup(Popup.UI_Store); }
        if (Input.GetKeyDown(KeyCode.Tab))    { Get<UI_CanvasFader>((int)Popup.UI_Scoreboard).gameObject.SetActive(true); }
		if (Input.GetKeyUp(KeyCode.Tab))      { Get<UI_CanvasFader>((int)Popup.UI_Scoreboard).gameObject.SetActive(false); }
        if (Input.GetKeyDown(KeyCode.Escape)) { CloseOtherPopupAndOnPopup(Popup.UI_Setting); }
        if (Input.GetKeyDown(KeyCode.I))      { CloseOtherPopupAndOnPopup(Popup.UI_Deck); }
    }

    void CloseOtherPopupAndOnPopup(Popup p)
    {
        if(p != Popup.UI_Store)   Get<UI_CanvasFader>((int)Popup.UI_Store)   .HideUI();
        if(p != Popup.UI_Deck)    Get<UI_CanvasFader>((int)Popup.UI_Deck)    .HideUI();
        if(p != Popup.UI_Setting) Get<UI_CanvasFader>((int)Popup.UI_Setting) .HideUI();

        Get<UI_CanvasFader>((int)p).TurnUI();
    }
}
