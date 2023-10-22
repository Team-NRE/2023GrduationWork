using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class DeadlySpeedStart : BaseEffect
{
    PhotonView _pv;

    float _powerValue;
    float _speedValue;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        //초기화
        base.CardEffectInit(userId);

        //effect 위치
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 0.2f, 0);
        
        //스텟 적용
        _effectTime = 4.0f;
        _speed = 1.0f;
        _powerValue = 20f;
        _speedValue = 0.5f;
        pStat.StartCoroutine(DelayBuff());
    }

    IEnumerator DelayBuff()
    {
        pStat.speed += _speed;
        pStat.basicAttackPower += _powerValue;
        pStat.attackSpeed += _speedValue;

        yield return new WaitForSeconds(_effectTime);

        pStat.speed -= _speed;
        pStat.basicAttackPower -= _powerValue;
        pStat.attackSpeed -= _speedValue;
    }
}
