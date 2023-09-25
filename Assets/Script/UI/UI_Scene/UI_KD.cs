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
        pStat = Managers.game.myCharacter?.GetComponent<PlayerStats>();

        killText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        DeathText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public override void UpdateInit()
    {
        if (pStat == null)
        {
            pStat = Managers.game.myCharacter?.GetComponent<PlayerStats>();
            return;
        }

        killText.text = $"{pStat.kill.ToString()}";
        DeathText.text = $"{pStat.death.ToString()}";
    }
}