using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class EnhancementStart : BaseEffect
{
    PlayerStats _pStats;
    protected PhotonView _pv;

    float powerValue = 15;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        base.CardEffectInit(userId);
        _effectTime = 5.0f;
        _pStats = player.GetComponent<PlayerStats>();
        transform.parent = player.transform;
        _pStats.StartCoroutine(DelayBuff());
    }

    IEnumerator DelayBuff()
    {
        _pStats.basicAttackPower += powerValue;
        yield return new WaitForSeconds(_effectTime);
        _pStats.basicAttackPower -= powerValue;
    }
}