using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Stat;
using Define;

public class UI_Mana : UI_Scene
{
    GameObject Mana1;
    GameObject Mana2;
    GameObject Mana3;

    Image Mana1_Img;
    Image Mana2_Img;
    Image Mana3_Img;

    PlayerStats pStat;
    PlayerType pType;

    public float manaRegeneration;

    public enum Manas
    {
        Mana1,
        Mana2,
        Mana3,
    }

    public override void Init()
    {
        pStatAction();
        
        Bind<GameObject>(typeof(Manas));
        Mana1 = Get<GameObject>((int)Manas.Mana1);
        Mana2 = Get<GameObject>((int)Manas.Mana2);
        Mana3 = Get<GameObject>((int)Manas.Mana3);

        Mana1_Img = Mana1.GetComponentInChildren<Image>();
        Mana2_Img = Mana2.GetComponentInChildren<Image>();
        Mana3_Img = Mana3.GetComponentInChildren<Image>();
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

    public void Update()
    {
        ManaSystem();
    }

    public void ManaSystem()
    {
        pStat.nowMana += Time.deltaTime;

        Mana1_Img.fillAmount = Mathf.Lerp(Mana1_Img.fillAmount,
            pStat.nowMana / pStat.manaRegen, pStat.nowMana);

        Mana2_Img.fillAmount = Mathf.Lerp(Mana2_Img.fillAmount,
            ((pStat.nowMana) / pStat.manaRegen) - 1, pStat.nowMana);

        Mana3_Img.fillAmount = Mathf.Lerp(Mana3_Img.fillAmount,
            ((pStat.nowMana) / pStat.manaRegen) - 2, pStat.nowMana);
    }

}