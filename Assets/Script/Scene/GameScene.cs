using Photon.Pun;
using Stat;
using Define;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameScene : BaseScene
{
    [Header("Kill Log Test")]
    public int test_attackerID;
    public int test_deadUserID;
    public bool test_isKillEvent = false;

    [Header("Card Inventory Test")]
    public string test_CardName;
    public bool test_CardAdd = false;
    public bool test_CardDelete = false;
    

    private void Update()
    {
        if (test_isKillEvent)
        {
            Managers.game.killEvent(test_attackerID, test_deadUserID);
            test_isKillEvent = false;
        }

        if (test_CardAdd)
        {
            BaseCard._initDeck.Add(test_CardName);
            BaseCard._MyDeck  .Add(test_CardName);
            test_CardAdd = false;
        }

        if (test_CardDelete)
        {
            BaseCard._initDeck.Remove(test_CardName);
            BaseCard._MyDeck  .Remove(test_CardName);
            test_CardDelete = false;
        }

        CheatKey();
    }

    protected override void Init()
    {
        SceneType = Scene.Game;
        Managers.UI.ShowSceneUI<UI_LoadingPage>();
    }

    private void LoadObjects()
    {

    }

    public override void Clear()
    {

    }

    public IEnumerator InitGameSetting()
    {
        /// 시작 시간 초기화
        Managers.game.startTime = PhotonNetwork.Time;

        // 타일맵 초기화
        InitTileMap();

        // 플레이어 생성
        Managers.game.myCharacter = PhotonNetwork.Instantiate(
            $"Prefabs/InGame/Player/{Managers.game.myCharacterType.ToString()}",
            Vector3.zero, 
            Quaternion.identity
        );

        yield return new WaitForSeconds(2.0f);

        Managers.UI.ShowSceneUI<UI_Setting>();
        Managers.UI.ShowSceneUI<UI_CardPanel>();
        Managers.UI.ShowSceneUI<UI_Popup>();
        Managers.UI.ShowSceneUI<UI_Minimap>();

        InitPlayerDefault();

        // 로딩 페이지 끄기
        FindObjectOfType<UI_LoadingPage>().gameObject.SetActive(false);

        //부활시간 재설정
        Managers.game.respawnTime = 3.0f;

        Debug.Log("Setting Finish");
    }

    private void InitTileMap()
    {
        Managers.game.grid    = FindObjectOfType<Grid>();
        Managers.game.tilemap = FindObjectOfType<Tilemap>();

        Managers.game.tileRoad       = Managers.Resource.Load<TileBase>("Texture/AI_Tile/tilePalette_9");
        Managers.game.tileBuilding   = Managers.Resource.Load<TileBase>("Texture/AI_Tile/tilePalette_1");
        Managers.game.tileMidWay     = Managers.Resource.Load<TileBase>("Texture/AI_Tile/tilePalette_10");
        Managers.game.tileCenterArea = Managers.Resource.Load<TileBase>("Texture/AI_Tile/tilePalette_2");
    }

    private void InitPlayerDefault()
    {
        BaseController[] bc = FindObjectsOfType<BaseController>();

        foreach (BaseController now in bc)
        {
            if (now._pStats == null) continue;

            if (now._pStats.playerArea == (int)Layer.Human)
            {
                if (Managers.game.humanTeamCharacter.Item1 == null)
                {
                    Managers.game.humanTeamCharacter.Item1 = now.gameObject.GetPhotonView();
                    Debug.Log($"{now.gameObject.name} , {Managers.game.humanTeamCharacter.Item1} ");
                }
                else
                {
                    Managers.game.humanTeamCharacter.Item2 = now.gameObject.GetPhotonView();
                    Debug.Log($"{now.gameObject.name} , {Managers.game.humanTeamCharacter.Item2} ");
                }
            }

            if (now._pStats.playerArea == (int)Layer.Cyborg)
            {
                if (Managers.game.cyborgTeamCharacter.Item1 == null)
                {
                    Managers.game.cyborgTeamCharacter.Item1 = now.gameObject.GetPhotonView();
                }
                else
                {
                    Managers.game.cyborgTeamCharacter.Item2 = now.gameObject.GetPhotonView();
                }
            }
        }
    }

    [PunRPC]
    public void KillEvent(int attackerID, int deadUserID)
    {
        GameObject attacker = PhotonView.Find(attackerID)?.gameObject;
        GameObject deadUser = PhotonView.Find(deadUserID)?.gameObject;

        // 예외 처리
        if (attacker == null)
        {
            return;
        }

        if (deadUser == null)
        {
            return;
        }

        // 가해자 처리
        if (attacker.tag == "PLAYER")
        {
            if (attacker.layer == LayerMask.NameToLayer("Human"))
            {
                Managers.game.humanTeamKill++;
            }

            if (attacker.layer == LayerMask.NameToLayer("Cyborg"))
            {
                Managers.game.cyborgTeamKill++;
            }

            attacker.GetComponent<PlayerStats>().kill++;
            attacker.GetComponent<PlayerStats>().gold += 500;
            attacker.GetComponent<PlayerStats>().experience += 100;
        }

        // 피해자 처리
        deadUser.GetComponent<PlayerStats>().death++;

        /// UI Event
        UI_KillLog killLog = Managers.UI.MakeSubItem<UI_KillLog>
            (Managers.UI.Root.transform.Find("UI_Popup/UI_KillLog"), "KillLog");

        killLog.Init(
            attacker,
            deadUser
        );
    }

    [PunRPC]
    public void addGnE(int targetId, Vector3 pos, float gold, float experience)
    {
        PlayerStats stat = Managers.game.RemoteTargetFinder(targetId).GetComponent<PlayerStats>();
        stat.gold += gold;
        stat.experience = experience;
            
        if (stat.gameObject.GetPhotonView().IsMine) summonCoinDrop(pos, gold);
    }

    public void summonCoinDrop(Vector3 pos, float gold)
    {
        GameObject coinDrop = Instantiate(Managers.Resource.Load<GameObject>("Prefabs/Particle/Effect_CoinDrop"), pos, transform.rotation);
        coinDrop.GetComponent<Particle_CoinDrop>().setInit(gold.ToString());
    }

    private void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.F12)) 
        {
            GameObject.Find("CyborgNexus")
            .GetComponent<PhotonView>().RPC(
                "photonStatSet", 
                RpcTarget.All, 
                "nowHealth", 
                -1000.0f
            );
        }
        if (Input.GetKeyDown(KeyCode.F11)) 
        {
            GameObject.Find("HumanNexus")
            .GetComponent<PhotonView>().RPC(
                "photonStatSet", 
                RpcTarget.All, 
                "nowHealth", 
                -1000.0f
            );
        }

        if (Input.GetKeyDown(KeyCode.F10)) 
        {
            Managers.game.myCharacter.GetComponent<PlayerStats>()
            .gold += 5000;
        }
    }
}