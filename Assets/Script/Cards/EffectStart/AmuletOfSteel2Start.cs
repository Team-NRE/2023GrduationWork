using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmuletOfSteel2Start : BaseEffect
{
    protected PhotonView _pv;

    PlayerStats pStats;
    float healthRegen = default;
    int teamLayer = default;
    int _playerId;

    void Start()
    {
        _pv = GetComponent<PhotonView>();
    }

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);

        pStats = player.GetComponent<PlayerStats>();
        teamLayer = player.GetComponent<PlayerStats>().playerArea;

        _playerId = userId;
    }

    public void OnTriggerEnter(Collider other)
    {
        int otherId = Managers.game.RemoteColliderId(other);
        if (otherId == default)
            return;
        _pv.GetComponent<PhotonView>().RPC("RpcTrigger", RpcTarget.All, otherId);
    }

    [PunRPC]
    public void RpcTrigger(int otherId)
    {
        GameObject other = Managers.game.RemoteTargetFinder(otherId);
        if (other.gameObject.layer == teamLayer)
        {
            switch (other.gameObject.tag)
            {
                case "PLAYER":
                    GameObject ShieldEffect = Managers.Resource.Instantiate($"Prefabs/Particle/Effect_AmuletofSteel");

                    ShieldEffect.transform.parent = player.transform;
                    ShieldEffect.transform.localPosition = new Vector3(0, 1.12f, 0);

                    break;

            }
            //ShieldEffect.GetComponent<PhotonView>().RPC(
            //"CardEffectInit",
            //RpcTarget.All,
            //other.gameObject.GetComponent<PhotonView>().ViewID
            //);
        }
    }
}