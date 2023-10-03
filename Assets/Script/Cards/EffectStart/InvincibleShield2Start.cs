using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleShield2Start : BaseEffect
{
    PhotonView _pv;
    public override void CardEffectInit(int userId, int targetId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId, targetId);
    }

    void Update()
    {
        
    }
}
