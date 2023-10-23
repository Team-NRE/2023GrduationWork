using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class BloodTransfusionStart : BaseEffect
{
    [PunRPC]
    public override void CardEffectInit(int userId, int targetId)
    {
        //초기화
        base.CardEffectInit(userId, targetId);

        //effect 위치
        transform.parent = target.transform;
        transform.localPosition = new Vector3(0, 0.8f, 0);

        //스텟 적용
        powerValue = (50.0f, 0.7f);
        damageValue = powerValue.Item1 + (pStat.basicAttackPower * powerValue.Item2);

        //데미지 피해
        if (!target.gameObject.CompareTag("PLAYER"))
        {
            ObjStats target_oStat = target.GetComponent<ObjStats>();
            target_oStat.nowHealth -= damageValue;

            pStat.nowHealth += damageValue;
        }

        if (target.gameObject.CompareTag("PLAYER"))
        {
            target_pStat = target.GetComponent<PlayerStats>();
            target_pStat.receviedDamage = (playerId, damageValue);

            pStat.nowHealth += target_pStat.receviedDamage.Item2;
        }
    }
}
