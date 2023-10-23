using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class DeadlySpeedStart : BaseEffect
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
        startEffect = 0.01f;
        effectTime = 4.0f;

        //스텟 적용
        speedValue = 1.0f;
        powerValue = (20f, 0);
        attackSpeedValue = 0.5f;
        healthRegenValue = 20f;

        //RPC 적용
        playerPV.RPC("photonStatSet", RpcTarget.All, "speed", speedValue);
        playerPV.RPC("photonStatSet", RpcTarget.All, "basicAttackPower", powerValue.Item1);
        playerPV.RPC("photonStatSet", RpcTarget.All, "attackSpeed", attackSpeedValue);
        playerPV.RPC("photonStatSet", RpcTarget.All, "healthRegeneration", healthRegenValue);
    }


    private void Update()
    {
        startEffect += Time.deltaTime;

        ///스텟 적용 종료
        if (startEffect > effectTime - 0.01f)
        {
            playerPV.RPC("photonStatSet", RpcTarget.All, "speed", -speedValue);
            playerPV.RPC("photonStatSet", RpcTarget.All, "basicAttackPower", -powerValue.Item1);
            playerPV.RPC("photonStatSet", RpcTarget.All, "attackSpeed", -attackSpeedValue);
            playerPV.RPC("photonStatSet", RpcTarget.All, "healthRegeneration", -healthRegenValue);

            Destroy(gameObject);

            return;
        }
    }
}
