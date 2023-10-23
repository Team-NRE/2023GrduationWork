using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectionStart : BaseEffect
{
    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        //�ʱ�ȭ
        base.CardEffectInit(userId);
        
        //effect ��ġ
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 0.2f, 0);

        //���� ����
        pStat.isResurrection = true;
    }
}
