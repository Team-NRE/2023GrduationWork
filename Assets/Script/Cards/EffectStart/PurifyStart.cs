using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurifyStart : BaseEffect
{
    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        base.CardEffectInit(userId);
        PlayerStats _stats = player.GetComponent<PlayerStats>();
        
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 0.1f, 0);

        _stats.nowState = "Health";
    }
}
