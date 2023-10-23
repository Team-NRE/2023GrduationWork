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
        //초기화
        base.CardEffectInit(userId);
        
        //effect 위치
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 0.1f, 0);

        //스텟 적용
        pStat.nowState = "Health";
    }
}
