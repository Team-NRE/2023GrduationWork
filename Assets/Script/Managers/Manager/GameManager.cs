/// ksPark
///
/// Game Manager

using UnityEngine;
using Define;
using UnityEngine.AI;

using Photon.Pun;
using System.Collections;

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
    public bool isGameStart {get; set;}
    public bool isGameEnd {get; set;}

    CameraController mainCamera;
    public Vector3 endingCamPos;
    
    /// 플레이 시간 관련
    public double startTime = 0;
	public double playTime = 0;

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
}