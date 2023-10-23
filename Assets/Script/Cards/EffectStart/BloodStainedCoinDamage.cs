using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodStainedCoinDamage : BaseEffect
{
    [PunRPC]
    public override void CardEffectInit(int userId, int remoteTargetId)
    {
        //초기화
        base.CardEffectInit(userId, remoteTargetId);

        //effect 위치
        transform.parent = target.transform;
        transform.localPosition = new Vector3(0, 0.8f, 0);
        
        //스텟 적용 시간
        effectTime = 2.0f;
        startEffect = 0.01f;
    }


    public void Update()
    {
        startEffect += Time.deltaTime;

        //스텟 적용 종료
        if (startEffect > effectTime - 0.01f)
        {
            Destroy(gameObject);

            return;
        }
    }
}
