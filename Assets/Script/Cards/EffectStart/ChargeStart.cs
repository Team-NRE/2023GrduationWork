using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class ChargeStart : BaseEffect
{
    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        //초기화
        base.CardEffectInit(userId);
        
        //effect 위치
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 0.2f, 0);

        //스텟 적용 시간
        effectTime = 2.0f;
        startEffect = 0.01f;

        //스텟 적용
        speedValue = 0.5f;
        powerValue = (10.0f, 0);

        //RPC 적용
        playerPV.RPC("photonStatSet", RpcTarget.All, "speed", speedValue);
        playerPV.RPC("photonStatSet", RpcTarget.All, "basicAttackPower", powerValue.Item1);
    }

    private void Update()
    {
        startEffect += Time.deltaTime;

        //스텟 적용 종료
        if (startEffect > effectTime - 0.01f)
        {
            playerPV.RPC("photonStatSet", RpcTarget.All, "speed", -speedValue);
            playerPV.RPC("photonStatSet", RpcTarget.All, "basicAttackPower", -powerValue.Item1);

            Destroy(gameObject);

            return;
        }
    }
}
