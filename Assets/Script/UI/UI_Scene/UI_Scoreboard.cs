/// ksPark
///
/// UI - Scoreboard
/// 인게임에서 Tab 키를 눌렀을 시 나오는 창

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Stat;
using Photon.Pun;

class ScoreboardPlayer
{
    public PhotonView photonView;
    GameObject parent;
    PlayerStats stat;

    Image icon;
    TextMeshProUGUI name;
    TextMeshProUGUI level;
    TextMeshProUGUI kill;
    TextMeshProUGUI death;

    public ScoreboardPlayer(PhotonView playerObj, GameObject uiParent)
    {
        photonView = playerObj;
        parent = uiParent;

        InitContent();
    }

    private void InitContent()
    {
        icon    = FindObject<Image>("Icon");
        name    = FindObject<TextMeshProUGUI>("Name");
        level   = FindObject<TextMeshProUGUI>("LevelText");
        kill    = FindObject<TextMeshProUGUI>("KillText");
        death   = FindObject<TextMeshProUGUI>("DeathText");
    }

    public void UpdateContent()
    {
        if (stat == null) 
        {
            stat = photonView?.GetComponent<PlayerStats>();
            parent.SetActive(stat != null);
            return;
        }

        if (icon != null && icon.sprite == null) 
            icon.sprite = Managers.Resource.Load<Sprite>($"Texture/UI/Character_Img/Icon_{stat.character}");
        if (name != null && name.text == "null") 
            name.text = stat.nickname;

        if (level != null) level.text  = stat.level.ToString();
        if (kill != null)  kill.text   = stat.kill.ToString();
        if (death != null) death.text  = stat.death.ToString();
    }

    private T FindObject<T>(string name)
    {
        T[] nowList = parent.GetComponentsInChildren<T>();

        for(int i=0; i<nowList.Length; i++)
        {
            if (nowList[i].ToString().Substring(0, name.Length).Equals(name))
            {
                return nowList[i];
            }
        }

        return default(T);
    }
}

public class UI_Scoreboard : MonoBehaviour
{
    #region Variable
    [Header ("- Team")]
    [SerializeField] private TextMeshProUGUI humanTeamKill;
    [SerializeField] private TextMeshProUGUI cyborgTeamKill;

    ScoreboardPlayer[] playerList;
    #endregion

    private void Awake()
    {
        playerList = new ScoreboardPlayer[4];
        FindObject<HorizontalLayoutGroup>("Content").spacing = (Managers.game.gameMode == Define.GameMode.Single ? 0 : 75);
    }

    private void OnEnable()
    {
        if (Managers.game.humanTeamCharacter.Item1 != null && playerList[0] == null)
            playerList[0] = new ScoreboardPlayer(Managers.game.humanTeamCharacter.Item1,  FindObject<VerticalLayoutGroup>("Scoreboard_Player1").gameObject);
        if (Managers.game.humanTeamCharacter.Item2 != null && playerList[1] == null)
            playerList[1] = new ScoreboardPlayer(Managers.game.humanTeamCharacter.Item2,  FindObject<VerticalLayoutGroup>("Scoreboard_Player2").gameObject);
        if (Managers.game.cyborgTeamCharacter.Item1 != null && playerList[2] == null)
            playerList[2] = new ScoreboardPlayer(Managers.game.cyborgTeamCharacter.Item1, FindObject<VerticalLayoutGroup>("Scoreboard_Player3").gameObject);
        if (Managers.game.cyborgTeamCharacter.Item2 != null && playerList[3] == null)
            playerList[3] = new ScoreboardPlayer(Managers.game.cyborgTeamCharacter.Item2, FindObject<VerticalLayoutGroup>("Scoreboard_Player4").gameObject);
    }

    private void Update()
    {
        SetTeamKill();

        for (int i=0; i<playerList.Length; i++)
        {
            if (playerList[i] == null) continue;
            playerList[i].UpdateContent();
        }
    }

    private void SetTeamKill()
    {
        humanTeamKill.text = Managers.game.humanTeamKill.ToString();
        cyborgTeamKill.text = Managers.game.cyborgTeamKill.ToString();
    }

    private T FindObject<T>(string name)
    {
        T[] nowList = this.gameObject.GetComponentsInChildren<T>(true);

        for(int i=0; i<nowList.Length; i++)
        {
            if (nowList[i].ToString().Substring(0, name.Length).Equals(name))
            {
                return nowList[i];
            }
        }

        return default(T);
    }
}
