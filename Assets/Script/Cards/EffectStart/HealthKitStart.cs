using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKitStart : BaseEffect
{
    PlayerStats playerStats;

    protected PhotonView _pv;

    int teamLayer = default;
    int _playerId;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);

        playerStats = player.GetComponent<PlayerStats>();
        teamLayer = playerStats.playerArea;
        
        _playerId = userId;

        _buff = 1f;

        this.gameObject.transform.parent = player.transform;
    }

    public void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All);
    }

    [PunRPC]
    public void RpcUpdate()
    {
        playerStats.nowHealth += _buff;
    }

    public void OnTriggerStay(Collider other)
    {
        int otherId = Managers.game.RemoteColliderId(other);
        if (otherId == default)
            return;
        _pv.RPC("RpcTrigger", RpcTarget.All, _playerId, otherId);
    }

    [PunRPC]
    public void RpcTrigger(int playerId, int otherId)
	{
        GameObject other = Managers.game.RemoteTargetFinder(otherId);

        if (other == null)
            return;
        
        if (other.gameObject.layer == teamLayer)
        {
            switch (other.gameObject.tag)
            {
                case "PLAYER":
                    PlayerStats pStats = other.gameObject.GetComponent<PlayerStats>();
                    pStats.nowHealth += _buff;

                    break;

                case "OBJECT":
                    ObjStats oStats = other.gameObject.GetComponent<ObjStats>();
                    oStats.nowHealth += _buff;

                    break;
            }
        }
    }
}
