using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class ShieldStart : BaseEffect
{
    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        //초기화
        base.CardEffectInit(userId);
        
        //effect 위치
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 1.12f, 0);

        //스텟 적용 시간
        effectTime = 3.0f;
        startEffect = 0.01f;

        //스텟 적용 
        double shieldPercent = 20;
        shieldValue = Managers.game.PercentageCount(shieldPercent, pStat.maxHealth, 1);

        //RPC 적용
        playerPV.RPC("photonStatSet", RpcTarget.All, "firstShield", shieldValue);
        playerPV.RPC("photonStatSet", RpcTarget.All, "shield", shieldValue);

    }

    private void Update()
    {
        startEffect += Time.deltaTime;

        if (startEffect > effectTime - 0.01f)
        {
            //초기 쉴드와 지금의 쉴드가 같지 않다면
            if (pStat.firstShield != pStat.shield)
            {
                pStat.shield = 0;
                playerPV.RPC("photonStatSet", RpcTarget.All, "firstShield", -shieldValue);
                playerPV.RPC("photonStatSet", RpcTarget.All, "shield", pStat.firstShield);

                Destroy(gameObject);

                return;
            }

            //초기 쉴드와 지금의 쉴드가 같다면
            if (pStat.firstShield == pStat.shield)
            {
                playerPV.RPC("photonStatSet", RpcTarget.All, "firstShield", -shieldValue);
                playerPV.RPC("photonStatSet", RpcTarget.All, "shield", -shieldValue);

                Destroy(gameObject);

                return;
            }
        }
    }
}
