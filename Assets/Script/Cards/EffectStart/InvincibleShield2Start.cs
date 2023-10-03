using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleShield2Start : BaseEffect
{
    protected PhotonView _pv;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        this.gameObject.transform.parent = player.transform;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        int otherId = Managers.game.RemoteColliderId(other);
        if (otherId == default)
            return;
        GameObject otherShield = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_InvincibleShield_1", other.transform.position, Quaternion.identity);
        otherShield.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, otherId);
    }
}
