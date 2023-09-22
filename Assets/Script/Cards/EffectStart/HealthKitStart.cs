using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKitStart : BaseEffect
{
    protected PhotonView _pv;

    PlayerStats pStats;
    float healthRegen = default;
    int teamLayer = default;
    int _playerId;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        healthRegen = 0.5f;
        pStats = player.GetComponent<PlayerStats>();
        teamLayer = player.GetComponent<PlayerStats>().playerArea;
        _playerId = userId;

        this.gameObject.transform.parent = player.transform;
    }

    private void Update()
    {
        //pStats.nowHealth += healthRegen;
        _pv.RPC("RpcUpdate", RpcTarget.All, _playerId);
    }

    public void OnTriggerStay(Collider other)
    {
        int id = Managers.game.RemoteTargetIdFinder(other.gameObject);
        _pv.RPC("RpcTrigger", RpcTarget.All, id);
    }

    [PunRPC]
    public void RpcTrigger(int otherId)
	{
        GameObject other = Managers.game.RemoteTargetFinder(otherId);
        if (other.gameObject.layer == teamLayer && other.gameObject.tag != "PLAYER")
        {
            ObjStats oStats = other.gameObject.GetComponent<ObjStats>();
            oStats.nowHealth += healthRegen;
        }
    }

    [PunRPC]
    public void RpcUpdate(int id)
    {
        GameObject target = GetRemotePlayer(id);
        PlayerStats stats = target.GetComponent<PlayerStats>();
        stats.nowHealth += healthRegen;
    }
}
