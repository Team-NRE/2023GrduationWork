using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKitStart : BaseEffect
{
    GameObject player = null;
    protected PhotonView _pv;

    PlayerStats pStats;
    float healthRegen = default;
    int teamLayer = default;

    public void StartHealthKit(int _player, float _healthhRegen, int _teamLayer)
    {
        //player = GameObject.Find(_player);
        player = Managers.game.RemoteTargetFinder(_player);
        healthRegen = _healthhRegen;
        teamLayer = _teamLayer;
        _pv = GetComponent<PhotonView>();

        pStats = player.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        pStats.nowHealth += healthRegen;
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
}
