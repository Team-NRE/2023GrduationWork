using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiantCrystalStart : BaseEffect
{
    protected PhotonView _pv;
    protected PlayerStats _stats;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        _stats = player.GetComponent<PlayerStats>();

        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 0, 0);

        _stats.nowMana += 3;
    }
}
