/// ksPark
///
/// UI - Scoreboard
/// 인게임에서 Tab 키를 눌렀을 시 나오는 창

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Stat;
using Photon.Pun;

public class UI_Scoreboard : MonoBehaviour
{
    #region Variable
    [Header ("- Team")]
    [SerializeField] private TextMeshProUGUI humanTeamKill;
    [SerializeField] private TextMeshProUGUI cyborgTeamKill;

    [Header ("- Human P1")]
    [SerializeField] private Image humanP1Icon;
    [SerializeField] private TextMeshProUGUI humanP1Name;
    [SerializeField] private TextMeshProUGUI humanP1Kill;
    [SerializeField] private TextMeshProUGUI humanP1Death;

    [Header ("- Human P2")]
    [SerializeField] private Image humanP2Icon;
    [SerializeField] private TextMeshProUGUI humanP2Name;
    [SerializeField] private TextMeshProUGUI humanP2Kill;
    [SerializeField] private TextMeshProUGUI humanP2Death;

    [Header ("- Cyborg P1")]
    [SerializeField] private Image cyborgP1Icon;
    [SerializeField] private TextMeshProUGUI cyborgP1Name;
    [SerializeField] private TextMeshProUGUI cyborgP1Kill;
    [SerializeField] private TextMeshProUGUI cyborgP1Death;

    [Header ("- Cyborg P2")]
    [SerializeField] private Image cyborgP2Icon;
    [SerializeField] private TextMeshProUGUI cyborgP2Name;
    [SerializeField] private TextMeshProUGUI cyborgP2Kill;
    [SerializeField] private TextMeshProUGUI cyborgP2Death;


    [Space (20.0f)]
    public Sprite[] icons;

    PlayerStats humanP1Stats, humanP2Stats;
    PlayerStats cyborgP1Stats, cyborgP2Stats;
    #endregion

    private void OnEnable()
    {
        GetPlayerStats();
        SetIcon();
        SetNickname();
    }

    private void Update()
    {
        GetPlayerStats();
        SetTeamKill();
        SetKill();
        SetDeath();
    }

    private void GetPlayerStats()
    {
        if (humanP1Stats == null)
            humanP1Stats = Managers.game.humanTeamCharacter.Item1?.GetComponent<PlayerStats>();

        if (humanP2Stats == null)
            humanP2Stats = Managers.game.humanTeamCharacter.Item2?.GetComponent<PlayerStats>();

        if (cyborgP1Stats == null)
            cyborgP1Stats = Managers.game.cyborgTeamCharacter.Item1?.GetComponent<PlayerStats>();

        if (cyborgP2Stats == null)
            cyborgP2Stats = Managers.game.cyborgTeamCharacter.Item2?.GetComponent<PlayerStats>();
    }

    private void SetIcon()
    {
        humanP1Icon.sprite = GetIconTexture(humanP1Stats);
        humanP2Icon.sprite = GetIconTexture(humanP2Stats);
        cyborgP1Icon.sprite = GetIconTexture(cyborgP1Stats);
        cyborgP2Icon.sprite = GetIconTexture(cyborgP2Stats);
    }

    private void SetNickname()
    {
        humanP1Name.text = GetNickname(humanP1Stats);
        humanP2Name.text = GetNickname(humanP2Stats);
        cyborgP1Name.text = GetNickname(cyborgP1Stats);
        cyborgP2Name.text = GetNickname(cyborgP2Stats);
    }

    private void SetTeamKill()
    {
        humanTeamKill.text = Managers.game.humanTeamKill.ToString();
        cyborgTeamKill.text = Managers.game.cyborgTeamKill.ToString();
    }

    private void SetKill()
    {
        humanP1Kill.text = GetKill(humanP1Stats);
        humanP2Kill.text = GetKill(humanP2Stats);
        cyborgP1Kill.text = GetKill(cyborgP1Stats);
        cyborgP2Kill.text = GetKill(cyborgP2Stats);
    }

    private void SetDeath()
    {
        humanP1Death.text = GetDeath(humanP1Stats);
        humanP2Death.text = GetDeath(humanP2Stats);
        cyborgP1Death.text = GetDeath(cyborgP1Stats);
        cyborgP2Death.text = GetDeath(cyborgP2Stats);
    }

    private Sprite GetIconTexture(PlayerStats player)
    {
        if (player == null) return null;

        string iconName = $"Icon_{player.character}";

        for (int i=0; i<icons.Length; i++)
        {
            if (icons[i].name == iconName) return icons[i];
        }

        return null;
    }

    private string GetNickname(PlayerStats player)
    {
        if (player == null) return " ";
        return player.nickname;
    }

    private string GetKill(PlayerStats player)
    {
        if (player == null) return " ";
        return player.kill.ToString();
    }

    private string GetDeath(PlayerStats player)
    {
        if (player == null) return " ";
        return player.death.ToString();
    }
}
