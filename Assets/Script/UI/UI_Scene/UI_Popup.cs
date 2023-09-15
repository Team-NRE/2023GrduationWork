using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Scene
{
    GameObject KDA;
    GameObject KillLog;
    GameObject StatBox;
    GameObject StautsBar;
    GameObject Scoreboard;
    GameObject Setting;
    GameObject Store;
    public GameObject Deck;

    public bool IsSetting = false;
    bool IsStore = false;
    public bool IsDeck = false;
    public bool FirstOn = false;

    public enum Popup
    {
        UI_KDA,
        UI_KillLog,
        UI_StatBox,
        UI_StautsBar,
        UI_Scoreboard,
        UI_Setting,
        UI_Store,
        UI_Deck,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(Popup));
        KDA = Get<GameObject>((int)Popup.UI_KDA);
        KillLog = Get<GameObject>((int)Popup.UI_KillLog);
        StatBox = Get<GameObject>((int)Popup.UI_StatBox);
        StautsBar = Get<GameObject>((int)Popup.UI_StautsBar);
        Scoreboard = Get<GameObject>((int)Popup.UI_Scoreboard);
        Setting = Get<GameObject>((int)Popup.UI_Setting);
        Store = Get<GameObject>((int)Popup.UI_Store);
        Deck = Get<GameObject>((int)Popup.UI_Deck);


        KDA.SetActive(true);
        KillLog.SetActive(true);
        StatBox.SetActive(true);
        StautsBar.SetActive(true);
        Scoreboard.SetActive(false);
        Setting.SetActive(IsSetting);
        Store.SetActive(IsSetting);
        Deck.SetActive(IsSetting);
    }

    public override void UpdateInit() 
    {
        KeyDownAction();
    }

    public void KeyDownAction()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            
            IsStore = (IsStore == false ? true : false);

            Store.SetActive(IsStore);
        }

        if (Input.GetKey(KeyCode.Tab)) { Scoreboard.SetActive(true); }
		if (Input.GetKeyUp(KeyCode.Tab)) { Scoreboard.SetActive(false); }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsSetting = (IsSetting == false ? true : false);

            Setting.SetActive(IsSetting);
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            
            IsDeck = (IsDeck == false ? true : false);

            Deck.SetActive(IsDeck);
        }
    }

}
