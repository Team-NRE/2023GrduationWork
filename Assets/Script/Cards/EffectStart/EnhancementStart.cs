using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class EnhancementStart : BaseEffect
{
    PlayerStats _pStats;
    protected PhotonView _pv;

    float powerValue = 5;

    [PunRPC]
    public override IEnumerator CardEffectInit(int userId, float time)
    {
        player = GetRemotePlayer(userId);
        _effectTime = 3.0f;
        _pStats = player.GetComponent<PlayerStats>();
        transform.parent = player.transform;

        _pStats.basicAttackPower += powerValue;
        yield return new WaitForSeconds(time);
        _pStats.basicAttackPower -= powerValue;
    }
}