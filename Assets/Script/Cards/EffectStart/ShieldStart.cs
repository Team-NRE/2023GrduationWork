using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class ShieldStart : BaseEffect
{
    PlayerStats _pStats;
    float defence = default;
    float shield_Time = 0.01f;
    protected PhotonView _pv;

    bool start;

    private void Start()
    {
        _pv = GetComponent<PhotonView>();
    }

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        
        base.CardEffectInit(userId);
        _pStats = player.GetComponent<PlayerStats>();
        defence = 50.0f;
        start = true;
    }

    public void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All);
    }

    [PunRPC]
    public void RpcUpdate(int playerId)
	{
        if (start == true)
        {
            shield_Time += Time.deltaTime;

            if (shield_Time >= 1.9f)
            {
                _pStats.defensePower -= defence;
                start = false;
            }
        }
    }
}
