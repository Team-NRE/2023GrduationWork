/// ksPark
///
/// Game Manager

using UnityEngine;
using Define;
using UnityEngine.AI;

using Photon.Pun;
using System.Collections;
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
    public double startTime = 0;
	public double playTime = 0;

    /// 플레이어 리스폰 시간 관련
    public double respawnTime = 3; //기본 Default = 3초

    public int respawnMin = 0;
    public int respawnTurn = 1;

    public float startRespawn = 0.01f;

    public bool isDie { get; set; }
    public bool isResur { get; set; }

    public PhotonView diedPlayerPV;

    /// 팀 킬 수 관련
    public int humanTeamKill = 0;
    public int cyborgTeamKill = 0;

    /// 플레이어 관련
    public GameObject myCharacter;
    public PlayerType myCharacterType;
    public PlayerTeam myCharacterTeam;

    public (PhotonView, PhotonView) humanTeamCharacter;
    public (PhotonView, PhotonView) cyborgTeamCharacter;
    #endregion


    public void OnUpdate()
    {
        if (isGameEnd)
        {
            // 메인 카메라 이동
            mainCamera.transform.position =
                Vector3.Lerp(
                    mainCamera.transform.position,
                    endingCamPos,
                    Time.deltaTime * 2f
                );
        }

        playTime = PhotonNetwork.Time - startTime;

        //리스폰 시간 업데이트
        respawnMin = ((int)playTime / 60);
        if(respawnMin == 1 * respawnTurn)
        {
            respawnTime += 2;
            respawnTurn += 1;
            Debug.Log($"전체 캐릭터 부활 시간 : {respawnTime}초");
        }

        //플레이어 사망 시
        switch (isDie)
        {
            case true:
                //부활 = 3초
                double FinishRespawn = (isResur == true) ? 3.0f : respawnTime;
                startRespawn += Time.deltaTime;
                if (startRespawn >= FinishRespawn) 
                {
                    respawnEvent();
                    isDie = false;
                    Debug.Log($" {FinishRespawn}초 지나서 부활 완료");
                }

                break;


            case false:
                startRespawn = 0.01f;
                diedPlayerPV = null;

                break;
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
        diedPlayerPV.RPC("RemoteRespawnEnable", RpcTarget.All, diedID, false);
        
        //부활 유무
        isResur = diedPlayerPV.gameObject.GetComponent<Stat.PlayerStats>().isResurrection;

        isDie = true;
    }

    public void respawnEvent()
    {
        diedPlayerPV.RPC("RemoteRespawnEnable", RpcTarget.All, diedPlayerPV.ViewID, true);
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

        foreach (ObjectController obj in objects)
        {
            // 각 오브젝트 별로 컴포넌트 비활성화
            switch (obj._type)
            {
                case ObjectType.Nexus:
                    obj.GetComponent<MinionSummoner>().enabled = false;
                    break;
                case ObjectType.MeleeMinion:
                case ObjectType.RangeMinion:
                case ObjectType.SuperMinion:
                    obj.GetComponent<NavMeshAgent>().enabled = false;
                    obj.GetComponent<Animator>().enabled = false;
                    break;
                case ObjectType.Tower:
                case ObjectType.Neutral:
                    obj.GetComponent<Animator>().enabled = false;
                    break;
            }

            obj.enabled = false;
        }
    }

    public GameObject RemoteTargetFinder(int id)
	{
        GameObject remoteTarget = PhotonView.Find(id).gameObject;
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
}