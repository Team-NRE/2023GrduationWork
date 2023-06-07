using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Scene
{
    GameObject KDA;
    GameObject KillLog;
    GameObject Scoreboard;
    GameObject Setting;
    GameObject Store;

    bool IsSetting = false;
    bool IsStore = false;

    public enum Popup
    {
        KDA,
        KillLog,
        Scoreboard,
        Setting,
        Store,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(Popup));
        KDA = Get<GameObject>((int)Popup.KDA);
        KillLog = Get<GameObject>((int)Popup.KillLog);
        Scoreboard = Get<GameObject>((int)Popup.Scoreboard);
        Setting = Get<GameObject>((int)Popup.Setting);
        Store = Get<GameObject>((int)Popup.Store);

        KDA.SetActive(true);
        KillLog.SetActive(true);
        Scoreboard.SetActive(false);
        Setting.SetActive(IsSetting);
        Store.SetActive(IsStore);
    }

    public void Update()
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
    }

}
