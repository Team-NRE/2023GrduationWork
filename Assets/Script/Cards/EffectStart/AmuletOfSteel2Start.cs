using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmuletOfSteel2Start : BaseEffect
{
    protected PhotonView _pv;

    int teamLayer = default;


    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);

        teamLayer = player.GetComponent<PlayerStats>().playerArea;

        GameObject ShieldEffect = Managers.Resource.Instantiate($"Particle/Effect_AmuletofSteel", player.transform);
        ShieldEffect.transform.localPosition = new Vector3(0, 1.12f, 0);
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
        if (other.tag != "PLAYER") return;
        if (other.layer != teamLayer) return;
        if (other.layer == teamLayer && other.tag == "PLAYER")
        {
            GameObject ShieldEffect = Managers.Resource.Instantiate($"Particle/Effect_AmuletofSteel", other.transform);
            ShieldEffect.transform.localPosition = new Vector3(0, 1.12f, 0);
        }

        //ShieldEffect.GetComponent<PhotonView>().RPC(
        //"CardEffectInit",
        //RpcTarget.All,
        //other.gameObject.GetComponent<PhotonView>().ViewID
        //);

    }
}