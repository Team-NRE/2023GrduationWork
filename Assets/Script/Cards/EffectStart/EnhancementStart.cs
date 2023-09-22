using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class EnhancementStart : BaseEffect
{
    PlayerStats _pStats;
    protected PhotonView _pv;

    float basicAttackPower = default;
    float power_Time = 0.01f;

    bool start = false;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        base.CardEffectInit(userId);
        _pv = GetComponent<PhotonView>();
        _pStats = player.GetComponent<PlayerStats>();

        _damage = 15;
        _effectTime = 5.0f;

        basicAttackPower = _pStats.basicAttackPower;

        start = true;
    }

    public void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All);
    }

    [PunRPC]
    public void RpcUpdate()
    {
        if (start == true)
        {
            power_Time += Time.deltaTime;

            if (power_Time >= 4.99f)
            {
                _pStats.basicAttackPower -= basicAttackPower;
                start = false;
            }
        }
    }
}