using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//GameManager에서는 인게임에서의 네트워크 콜백을 제어한다.
/*
    1. 게임 초기화
    2. 게임 내에서 사망시 사용할 콜백 -> 네트워크 콜백
    3. 게임 내에서 부활시 사용할 콜백

    이외에 참고 사항
    1. 미니언 생성은 오브젝트에 귀속되어 있음
    2. 미니언 이벤트(생성, 사망 등) 모두 오브젝트 귀속 오브젝트 서먼에 있음 -> 현재 리팩토링 중

    캐릭터 스크립트 내에서 게임 매니저의 콜백을 인스턴스 형태로 받아온다.
 */

public class GameManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    // 캐릭터 초기 생성
    private void CreatePlayer()
    {

    }

    //캐릭터 부활
    private void ReSpawnPlayer()
    {
        //덱 및 카드 정보는 캐릭터 컴포넌트로 들어가 있음 -> 죽을때 이거까지 날리면 안됌
    }
    
    //캐릭터 사망
    private void DeathPlayer()
    {

    }
}
