using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class ChargeStart : BaseEffect
{
    PlayerStats _pStats;
    protected PhotonView _pv;

    float _powerValue;
    

    [PunRPC]
    public override IEnumerator CardEffectInit(int userId, float time)
    {

        player = GetRemotePlayer(userId);
        this.gameObject.transform.parent = player.transform;
        this.gameObject.transform.localPosition = new Vector3(0, 0.2f, 0);
        _pStats = player.GetComponent<PlayerStats>();
        _speed = 0.5f;
        _powerValue = 10.0f;

        _pStats.speed += _speed;
        _pStats.basicAttackPower += _powerValue;
        yield return new WaitForSeconds(time);
        _pStats.speed -= _speed;
        _pStats.basicAttackPower -= _powerValue;
    }

    IEnumerator DelayBuff()
    {
        _pStats.speed += _speed;
        _pStats.basicAttackPower += _powerValue;
        yield return new WaitForSeconds(_effectTime);
        _pStats.speed -= _speed;
        _pStats.basicAttackPower -= _powerValue;
    }
}
