using UnityEngine;
using Stat;
using TMPro;

public class UI_StatBox : UI_Base
{
    // myCharacter PlayerStat
    public PlayerStats myStat;

    enum StatText
	{
		Attack_Text,
		Shield_Text,
		AttackSpeed_Text,
		Speed_Text,
        Strength_Text,
        Gold_Text,
	}

    float lastGold;

    public override void Init() 
    {
        Bind<TextMeshProUGUI>(typeof(StatText));
    }

    public override void UpdateInit()
    {
        if (Managers.game.myCharacter == null) return;

        GetStat();
        UpdateStat();
    }

    void GetStat()
    {
        if (myStat != null) return;
        myStat = Managers.game.myCharacter.GetComponent<PlayerStats>();
        lastGold = myStat.gold;
        Get<TextMeshProUGUI>((int)StatText.Gold_Text).text  = ((int)lastGold).ToString();
    }

    void UpdateStat()
    {
        if (myStat == null) return;
        Get<TextMeshProUGUI>((int)StatText.Attack_Text)      .text  = myStat.basicAttackPower   .ToString("F1");
        Get<TextMeshProUGUI>((int)StatText.Shield_Text)      .text  = myStat.defensePower       .ToString("F1");
        Get<TextMeshProUGUI>((int)StatText.AttackSpeed_Text) .text  = myStat.attackSpeed        .ToString("F3");
        Get<TextMeshProUGUI>((int)StatText.Speed_Text)       .text  = myStat.speed              .ToString("F1");
        Get<TextMeshProUGUI>((int)StatText.Strength_Text)    .text  = myStat.healthRegeneration .ToString("F1");

        if (lastGold != myStat.gold)
        {
            lastGold = Mathf.Lerp(lastGold, myStat.gold, 10f * Time.deltaTime);
            Get<TextMeshProUGUI>((int)StatText.Gold_Text).text  = ((int)lastGold).ToString();
        }
    }
}
