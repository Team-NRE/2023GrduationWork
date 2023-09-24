using System.Collections;
using UnityEngine;
using Photon.Pun;
using Stat;

public class AmuletOfSteelStart : BaseEffect
{
    PlayerStats _pStats;

    float shieldValue = default;
    float shieldRatioPerHealth = 0.5f;
    float shieldTime = 5.0f;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        base.CardEffectInit(userId);
        _pStats = player.GetComponent<PlayerStats>();
        transform.parent = _pStats.transform;
        shieldValue = _pStats.maxHealth * shieldRatioPerHealth;

        _pStats.StartCoroutine(DelayBuff());
    }

    IEnumerator DelayBuff()
    {
        _pStats.shield += shieldValue;
        yield return new WaitForSeconds(shieldTime);
        _pStats.shield -= shieldValue;
    }
}
