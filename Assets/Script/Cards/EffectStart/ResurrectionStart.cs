using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectionStart : BaseEffect
{
    PhotonView _pv;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);

        this.gameObject.transform.parent = player.transform;
        this.gameObject.transform.localPosition = new Vector3(0, 0.2f, 0);

        player.GetComponent<PlayerStats>().isResurrection = true;
    }

    void Update()
    {
        
    }
}
