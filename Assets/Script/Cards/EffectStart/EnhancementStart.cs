using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class EnhancementStart : BaseEffect
{
    PlayerStats _pStats;

    float powerValue;

    [PunRPC]
    public override IEnumerator CardEffectInit(int userId, float time)
    {
        player = GetRemotePlayer(userId);
        _pStats = player.GetComponent<PlayerStats>();
        transform.parent = player.transform;
        powerValue = 10;

        _pStats.basicAttackPower += powerValue;
        yield return new WaitForSeconds(time);
        _pStats.basicAttackPower -= (powerValue - 1);
    }
}