using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsOfTheBattlefieldStart : BaseEffect
{
    PlayerStats _pStats;
    protected PhotonView _pv;

    float speed = default;
    float speed_Time = 0.01f;
    float effectTime = default;
    int _playerId;

    bool start = false;

    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        _pStats = player.GetComponent<PlayerStats>();
        _playerId = userId;
        speed = 2.0f;
        effectTime = 1.1f;
    }


    public void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All, _playerId);
    }

    [PunRPC]
    public void RpcUpdate(int playerId)
	{
        if (start == true)
        {
            speed_Time += Time.deltaTime;

            if (speed_Time >= effectTime - 0.01f)
            {
                _pStats.speed -= speed;
                start = false;

                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
