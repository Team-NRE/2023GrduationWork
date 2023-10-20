using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleShield2Start : BaseEffect
{
    protected PhotonView _pv;

    int teamLayer = default;
    int _otherId;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);

        teamLayer = player.GetComponent<PlayerStats>().playerArea;

        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 1.12f, 0);

        GameObject ShieldEffect = Managers.Resource.Instantiate($"Particle/Effect_InvincibleShield", player.transform);
        ShieldEffect.transform.localPosition = new Vector3(0, 1.12f, 0);
    }

    public void OnTriggerEnter(Collider other)
    {
        int otherId = Managers.game.RemoteColliderId(other);
        _otherId = otherId;
        if (_otherId == default)
            return;
        _pv.GetComponent<PhotonView>().RPC("RpcTrigger", RpcTarget.All, _otherId);
    }

    [PunRPC]
    public void RpcTrigger(int otherId)
    {
        GameObject other = Managers.game.RemoteTargetFinder(otherId);
        if (other == null)
            return;

        if (other.tag != "PLAYER") return;
        if (other.layer != teamLayer) return;
        if (other.layer == teamLayer && other.tag == "PLAYER")
        {
            GameObject ShieldEffect = Managers.Resource.Instantiate($"Particle/Effect_InvincibleShield", other.transform);
            ShieldEffect.transform.localPosition = new Vector3(0, 1.12f, 0);
        }
    }
}
