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
    }

    void UpdateStat()
    {
        if (myStat == null) return;
        Get<TextMeshProUGUI>((int)StatText.Attack_Text)      .text  = myStat.basicAttackPower   .ToString();
        Get<TextMeshProUGUI>((int)StatText.Shield_Text)      .text  = myStat.defensePower       .ToString();
        Get<TextMeshProUGUI>((int)StatText.AttackSpeed_Text) .text  = myStat.attackSpeed        .ToString();
        Get<TextMeshProUGUI>((int)StatText.Speed_Text)       .text  = myStat.speed              .ToString();
        Get<TextMeshProUGUI>((int)StatText.Strength_Text)    .text  = myStat.healthRegeneration .ToString();
        Get<TextMeshProUGUI>((int)StatText.Gold_Text)        .text  = myStat.gold               .ToString();
    }
}
