/// ksPark
///
/// Game Manager

using UnityEngine;
using Define;
using UnityEngine.AI;

using Photon.Pun;

public class GameManager
{
    #region Variable
    /// 게임 엔딩 관련
    public bool isGameEnd {get; set;}

    CameraController mainCamera;
    public Vector3 endingCamPos;
    
    /// 플레이 타임 관련
	public float playTime = 0;
    #endregion

    private void Awake()
    {

    }

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

        playTime += Time.deltaTime;
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
            switch(obj._type)
            {
                case ObjectType.Nexus:
                    obj.GetComponent<MinionSummoner>().enabled = false;
                    break;
                case ObjectType.Melee:
                case ObjectType.Range:
                case ObjectType.Super:
                    obj.GetComponent<NavMeshAgent>().enabled = false;
                    obj.GetComponent<Animator>().enabled = false;
                    break;
                case ObjectType.Turret:
                case ObjectType.Neutral:
                    obj.GetComponent<Animator>().enabled = false;
                    break;
            }

            obj.enabled = false;
        }
    }
}
