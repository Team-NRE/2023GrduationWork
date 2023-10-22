using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalStart : BaseEffect
{
    PhotonView _pv;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        //초기화
        base.CardEffectInit(userId);
        _pv = GetComponent<PhotonView>();
        
        //effect 위치
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 0, 0);

        //스텟 적용
        pStat.nowMana += 1;
    }
}
