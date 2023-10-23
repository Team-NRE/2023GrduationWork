/// ksPark
///
/// Game Manager

using UnityEngine;
using Define;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

using Photon.Pun;
using Data;

public class GameManager
{
    #region Variable
    /// 유저 정보 관련
    public string nickname;

    /// 방 인원 관련
	public GameMode gameMode;
    public int remainHuman;
    public int remainCyborg;

    /// 게임 시점 관련
    private bool isPlayingGame = false;

    public bool isGameStart {
            get { return isPlayingGame; }
            set 
            {
                isPlayingGame = value;

                if (isPlayingGame.Equals(false))    return;

                /// RPC로 GameScene에 GameStart 함수 실행
                GameObject.FindObjectOfType<GameScene>().StartCoroutine("InitGameSetting");
            }
        }
    public bool isGameEnd {get; set;}

    CameraController mainCamera;
    public Vector3 endingCamPos;
    
    /// 플레이 시간 관련
    public double startTime;
	public double playTime; 

    /// 플레이어 리스폰 시간 관련
    public double respawnTime; //기본 Default = 3초
    public double respawnTimeValue;
    public double maxRespawnTime;

    public int respawnTurn;
    public int respawnMin;

    public float startRespawn;
    

    ///죽음 관련
    public bool isResur { get; set; }
    public bool isDie { get; set; }

    public PhotonView diedPlayerPV;
    public Stat.PlayerStats diedPlayerStat;
    
    /// 팀 킬 수 관련
    public int humanTeamKill;
    public int cyborgTeamKill;

    /// 플레이어 관련
    public GameObject myCharacter;
    public PlayerType myCharacterType;
    public PlayerTeam myCharacterTeam;

    public (PhotonView, PhotonView) humanTeamCharacter;
    public (PhotonView, PhotonView) cyborgTeamCharacter;

    /// 타일맵 관련
    public GridLayout grid;
    public Tilemap tilemap;
    public TileBase tileRoad, tileBuilding, tileMidWay, tileCenterArea;

    /// 중앙 오브젝트 관련
    public Stat.ObjStats neutralMobStat;
    public float healthValue; 
    #endregion

    public void Init()
    {
        /// 플레이 시간 관련
        startTime = 0;
        playTime = 0;

        /// 플레이어 리스폰 시간 관련
        respawnTime = 3; //기본 Default = 3초
        respawnTimeValue = 2;
        maxRespawnTime = 20;

        respawnTurn = 1;
        
        respawnMin = 0;

        startRespawn = 0.01f;

        /// 팀 킬 수 관련
        humanTeamKill = 0;
        cyborgTeamKill = 0;

        ///중앙 오브젝트 관련
        healthValue = 300f;
    }

   

    public void OnUpdate()
    {
        if (PhotonNetwork.CurrentRoom == null)
            return;

        if (isGameEnd)
        {
            // 메인 카메라 이동
            mainCamera.transform.position =
                Vector3.Lerp(
                    mainCamera.transform.position,
                    endingCamPos,
                    Time.deltaTime * 2f
                );
            
            return;
        }

        playTime = PhotonNetwork.Time - startTime;

        //리스폰 시간 업데이트
        respawnMin = ((int)playTime / 60);
        if(respawnMin == 1 * respawnTurn)
        {
            respawnTime += respawnTimeValue;
            respawnTurn++;
            
            if (respawnTime >= maxRespawnTime) { respawnTime = maxRespawnTime; }
            neutralMobStat.maxHealth += healthValue;
            neutralMobStat.nowHealth += healthValue;

            Debug.Log($"전체 캐릭터 부활 시간 : {respawnTime}초");
            Debug.Log($"중앙 오브젝트 최대 체력 : {neutralMobStat.maxHealth}, 현재 체력 : {neutralMobStat.nowHealth} ");
        }

        if (diedPlayerPV == null) return;
        if (diedPlayerPV.IsMine)
        {
            //플레이어 사망 시
            switch (isDie)
            {
                case true:
                    startRespawn += Time.deltaTime;

                    if (startRespawn >= diedPlayerStat.respawnTime)
                    {
                        respawnEvent();
                        isDie = false;
                        Debug.Log($"{diedPlayerPV.gameObject} 는 {diedPlayerStat.respawnTime}초 지나서 부활 완료");
                    }

                    break;


                case false:
                    startRespawn = 0.01f;
                    diedPlayerPV = null;

                    break;
            }
        }

    }

    /// 플레이어 킬 이벤트
    ///
    /// int attackerID : 가해자 View ID
    /// int deadUserID : 피해자 View ID
    public void killEvent(int attackerID, int deadUserID)
    {
        /// 예외 처리
        if (PhotonView.Find(attackerID) == null) return;
        if (PhotonView.Find(deadUserID) == null) return;


        /// RPC 실행
        PhotonView pv = PhotonView.Get(GameObject.Find("GameScene"));

        pv.RPC(
            "KillEvent",
            RpcTarget.All,
            attackerID,
            deadUserID
        );
    }

    /// 플레이어 죽음 이벤트
    ///
    /// int die ID : 죽은 플레이어 View ID
    public void DieEvent(int diedID)
    {
        // 예외 처리 
        if (PhotonView.Find(diedID) == null) return;

        //PhotonView
        diedPlayerPV = PhotonView.Find(diedID);
        diedPlayerPV.RPC("RemoteRespawnEnable", RpcTarget.All, diedID, false, 1);
        diedPlayerPV.RPC("RemoteRespawnEnable", RpcTarget.All, diedID, false, 2);

        //부활 유무
        diedPlayerStat = diedPlayerPV.gameObject.GetComponent<Stat.PlayerStats>();
        isResur = diedPlayerStat.isResurrection;
        diedPlayerStat.respawnTime = (isResur == true) ? 3.0f : (float)respawnTime;

        isDie = true;
    }

    public void respawnEvent()
    {
        diedPlayerPV.RPC("RemoteRespawnEnable", RpcTarget.All, diedPlayerPV.ViewID, true, 1);
    }

    /// 게임 종료 스크립트
    ///
    /// Vector3 _endCamPos : 종료되는 화면 위치 (파괴되는 Nexus 위치)
    public void setGameEnd(Vector3 _endCamPos)
    {
        isGameEnd = true;

        /// 카메라 비활성화
        mainCamera = Camera.main.GetComponent<CameraController>();
        endingCamPos = _endCamPos;
        mainCamera.enabled = false;

        /// 오브젝트 비활성화
        ObjectController[] objects = GameObject.FindObjectsOfType<ObjectController>();

        for (int i=0; i<objects.Length; i++)
        {
            // 각 오브젝트 별로 컴포넌트 비활성화
            switch (objects[i]._type)
            {
                case ObjectType.Nexus:
                    objects[i].GetComponent<MinionSummoner>().enabled = false;
                    break;
                case ObjectType.MeleeMinion:
                case ObjectType.RangeMinion:
                case ObjectType.SuperMinion:
                    objects[i].GetComponent<NavMeshAgent>().enabled = false;
                    objects[i].GetComponent<Animator>().enabled = false;
                    break;
                case ObjectType.Tower:
                case ObjectType.Neutral:
                    objects[i].GetComponent<Animator>().enabled = false;
                    break;
            }

            objects[i].enabled = false;
        }

        /// 플레이어 비활성화
        Players[] players = GameObject.FindObjectsOfType<Players>();

        for (int i=0; i<players.Length; i++)
        {
            players[i].enabled = false;
            players[i].GetComponent<NavMeshAgent>().enabled = false;
            players[i].GetComponent<Animator>().enabled = false;
        }
    }

    public GameObject RemoteTargetFinder(int id)
	{
        GameObject remoteTarget = PhotonView.Find(id).gameObject;

        if (remoteTarget == null) { return null; }

        return remoteTarget;
	}

    public int RemoteTargetIdFinder(GameObject collider)
	{
        return collider.GetComponent<PhotonView>().ViewID;
	}

    public int RemoteColliderId(Collider collider)
    {
        if (collider.gameObject.GetComponent<PhotonView>() == null) 
            return default;
        int colliderId = collider.gameObject.GetComponent<PhotonView>().ViewID;
        return colliderId;
    }

    //퍼센트 계산
    public float PercentageCount(double percent, double value, int decimalplaces)
    {
        return (float)System.Math.Round(percent / 100 * value, decimalplaces);
    }

    public ObjectPosArea GetPosAreaInMap(Vector3 pos)
    {
        if (!PhotonNetwork.IsMasterClient) return ObjectPosArea.Undefine;
        
        Vector3Int gridPos = grid.WorldToCell(pos);
        TileBase nowTileBase = tilemap.GetTile(gridPos);

        if (nowTileBase.Equals(tileRoad))       return ObjectPosArea.Road;
        if (nowTileBase.Equals(tileBuilding))   return ObjectPosArea.Building;
        if (nowTileBase.Equals(tileMidWay))     return ObjectPosArea.MidWay;
        if (nowTileBase.Equals(tileCenterArea)) return ObjectPosArea.CenterArea;

        return ObjectPosArea.Undefine;
    }

    public void Clear()
    {
        humanTeamKill = 0;
        cyborgTeamKill = 0;

        myCharacter = null;
        myCharacterType = default;
        myCharacterTeam = default;

        humanTeamCharacter = (null, null);
        cyborgTeamCharacter = (null, null);
    }
}