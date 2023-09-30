using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class DeadlySpeedStart : BaseEffect
{
    PlayerStats _pStats;
    protected PhotonView _pv;

    float _powerValue;
    float _speedValue;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        base.CardEffectInit(userId);
        _effectTime = 3.0f;
        _pStats = player.GetComponent<PlayerStats>();
        transform.parent = player.transform;
        _speed = 1.0f;
        _powerValue = 0.5f;
        _speedValue = 0.5f;

        _pStats.StartCoroutine(DelayBuff());
    }

    IEnumerator DelayBuff()
    {
        _pStats.speed += _speed;
        _pStats.basicAttackPower += _powerValue;
        _pStats.attackSpeed += _speedValue;
        yield return new WaitForSeconds(_effectTime);
        _pStats.speed -= _speed;
        _pStats.basicAttackPower -= _powerValue;
        _pStats.attackSpeed -= _speedValue;
    }
}
