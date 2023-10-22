using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class StrikeStart : BaseEffect
{
    PhotonView _pv;
    PhotonView _pvTarget;

    float effectTime = default;
    float startEffect;

    int _targetId;
    int _playerId;

    PlayerStats pStats;

    PlayerStats target_pStats;
    ObjStats target_oStats;

    

    [PunRPC]
    public override void CardEffectInit(int userId, int targetId)
    {
        base.CardEffectInit(userId, targetId);

        //초기화
        _pv = GetComponent<PhotonView>();
        _pvTarget = target.GetComponent<PhotonView>();
        pStats = player.GetComponent<PlayerStats>();

        //effect 위치
        transform.parent = target.transform;
        transform.localPosition = new Vector3(0, 1.8f, 0);

        //effect 발동시간
        effectTime = 2.5f;
        startEffect = 0.01f;

        //target 데미지, debuff 적용
        if (target.CompareTag("OBJECT"))
        {
            //초기화
            target_oStats = target.GetComponent<ObjStats>();

            //데미지
            target_oStats.nowHealth -= 15.0f + pStats.basicAttackPower * 0.7f;

            //debuff
            _speed = target_oStats.speed;
            target_oStats.speed = 0;
        }
        if (target.CompareTag("PLAYER")) 
        {
            //초기화
            target_pStats = target.GetComponent<PlayerStats>();

            //데미지
            target_pStats.receviedDamage = (_playerId, 15.0f + (pStats.basicAttackPower * 0.7f));

            //debuff
            _speed = target_pStats.speed;
            target_pStats.speed = 0;
            target_pStats.nowState = "Debuff";
        }
    }

    public void Update()
    {
        startEffect += Time.deltaTime;

        //타겟 : 미니언
        if (target.CompareTag("OBJECT"))
        {
            ///스텟 적용 종료 (시간 종료)
            if (startEffect > effectTime - 0.01f)
            {
                _pvTarget.RPC("photonStatSet", RpcTarget.All, "speed", _speed);

                //현재 카드 삭제
                Destroy(gameObject);
            }
        }

        //타겟 : 플레이어
        if (target.CompareTag("PLAYER"))
        {
            ///스텟 적용 종료 (시간 종료 / 정화 사용시)
            if (startEffect > effectTime - 0.01f || target_pStats.nowState == "Health")
            {
                _pvTarget.RPC("photonStatSet", RpcTarget.All, "speed", _speed);

                //현재 카드 삭제
                Destroy(gameObject);
            }
        }
    }
}
