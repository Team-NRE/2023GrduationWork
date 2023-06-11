using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Stat;

public class UI_Mana : UI_Scene
{
    GameObject Mana1;
    GameObject Mana2;
    GameObject Mana3;

    Image Mana1_Img;
    Image Mana2_Img;
    Image Mana3_Img;

    PlayerStats pStat;

    public float manaRegeneration;

    public enum Manas
    {
        Mana1,
        Mana2,
        Mana3,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(Manas));
        Mana1 = Get<GameObject>((int)Manas.Mana1);
        Mana2 = Get<GameObject>((int)Manas.Mana2);
        Mana3 = Get<GameObject>((int)Manas.Mana3);

        Mana1_Img = Mana1.GetComponentInChildren<Image>();
        Mana2_Img = Mana2.GetComponentInChildren<Image>();
        Mana3_Img = Mana3.GetComponentInChildren<Image>();

        pStat = GameObject.FindWithTag("PLAYER").GetComponent<PlayerStats>();
    }

    public void Update()
    {
        ManaSystem();
    }

    public void ManaSystem()
    {
        pStat.nowMana = Time.deltaTime;

        Mana1_Img.fillAmount = Mathf.Lerp(Mana1_Img.fillAmount,
            pStat.nowMana / pStat._manaRegen, pStat.nowMana);

        Mana2_Img.fillAmount = Mathf.Lerp(Mana2_Img.fillAmount,
            ((pStat.nowMana) / pStat._manaRegen) - 1, pStat.nowMana);

        Mana3_Img.fillAmount = Mathf.Lerp(Mana3_Img.fillAmount,
            ((pStat.nowMana) / pStat._manaRegen) - 2, pStat.nowMana);
    }

}
