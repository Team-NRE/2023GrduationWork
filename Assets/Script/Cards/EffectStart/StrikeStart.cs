using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class StrikeStart : BaseEffect
{
    [PunRPC]
    public override void CardEffectInit(int userId, int targetId)
    {
        //초기화
        base.CardEffectInit(userId, targetId);
        effectPV = target.GetComponent<PhotonView>();

        //effect 위치
        transform.parent = target.transform;
        transform.localPosition = new Vector3(0, 1.8f, 0);

        //스텟 적용 시간
        effectTime = 2.5f;
        startEffect = 0.01f;

        //스텟 적용
        powerValue = (50f, 0.7f);
        damageValue = powerValue.Item1 + (pStat.basicAttackPower * powerValue.Item2);

        //데미지 피해
        if (target.CompareTag("OBJECT"))
        {
            //초기화
            target_oStat = target.GetComponent<ObjStats>();

            //데미지
            target_oStat.nowHealth -= damageValue;

            //debuff
            speedValue = target_oStat.speed;
            target_oStat.speed = 0;
        }
        if (target.CompareTag("PLAYER")) 
        {
            //초기화
            target_pStat = target.GetComponent<PlayerStats>();

            //데미지
            target_pStat.receviedDamage = (playerId, damageValue);

            //debuff
            speedValue = target_pStat.speed;
            target_pStat.speed = 0;
            target_pStat.nowState = "Debuff";
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
                effectPV.RPC("photonStatSet", RpcTarget.All, "speed", speedValue);

                //현재 카드 삭제
                Destroy(gameObject);
            }
        }

        //타겟 : 플레이어
        if (target.CompareTag("PLAYER"))
        {
            ///스텟 적용 종료 (시간 종료 / 정화 사용시)
            if (startEffect > effectTime - 0.01f || target_pStat.nowState == "Health")
            {
                effectPV.RPC("photonStatSet", RpcTarget.All, "speed", speedValue);

                //현재 카드 삭제
                Destroy(gameObject);
            }
        }
    }
}
