using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Stat;
using Define;

public class UI_KD : UI_Popup
{
    TextMeshProUGUI killText;
    TextMeshProUGUI DeathText;

    PlayerStats pStat;
    PlayerType pType;

    public override void Init()
    {
        killText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        DeathText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        pStatAction();
    }

    public void pStatAction()
    {
        switch (pType)
        {
            case Define.PlayerType.Police:
                pStat = GameObject.Find("Police").GetComponent<PlayerStats>();

                break;

            case Define.PlayerType.Firefight:
                pStat = GameObject.Find("Firefight").GetComponent<PlayerStats>();

                break;

            case Define.PlayerType.Lightsaber:
                pStat = GameObject.Find("Lightsaber").GetComponent<PlayerStats>();

                break;

            case Define.PlayerType.Monk:
                pStat = GameObject.Find("Monk").GetComponent<PlayerStats>();

                break;
        }
    }


    public override void UpdateInit()
    {
        killText.text = $"{pStat.kill.ToString()}";
        DeathText.text = $"{pStat.death.ToString()}";
    }
}
