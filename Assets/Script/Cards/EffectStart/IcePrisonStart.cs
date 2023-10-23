using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class IcePrisonStart : BaseEffect
{
    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        //초기화
        base.CardEffectInit(userId);

        //effect 위치
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 0.3f, 0);

        //스텟 적용 시간
        effectTime = 3.0f;
        startEffect = 0.01f;

        //스텟 적용 
        speedValue = pStat.speed;

        //RPC 적용
        playerPV.RPC("photonStatSet", RpcTarget.All, "defensePower", 9999f);
        playerPV.RPC("photonStatSet", RpcTarget.All, "speed", -pStat.speed);
    }

    private void Update()
    {
        startEffect += Time.deltaTime;

        //스텟 적용 종료
        if (startEffect > effectTime - 0.01f)
        {
            playerPV.RPC("photonStatSet", RpcTarget.All, "defensePower", -9999f);
            playerPV.RPC("photonStatSet", RpcTarget.All, "speed", speedValue);

            Destroy(gameObject);

            return;
        }
    }

}
