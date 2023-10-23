using Define;
using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrisisAversionStart : BaseEffect
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
        startEffect = 0.01f;
        effectTime = 1.5f;

        //스텟 적용
        invincibleTime = true;
        healthRegenValue = 75f;
        speedValue = 1.5f;
        attackSpeedValue = 1.0f;
        powerValue = (30f, 0);

        //RPC 적용
        playerPV.RPC("photonStatSet", RpcTarget.All, "defensePower", 9999f);
    }

    private void Update()
    {
        startEffect += Time.deltaTime;

        //스텟 적용 종료
        if (startEffect > effectTime - 0.01f)
        {
            switch (invincibleTime)
            {
                ///1.5초 동안 무적
                case true:
                    ///무적 끝
                    playerPV.RPC("photonStatSet", RpcTarget.All, "defensePower", -9999f);

                    ///체력 회복량 상승 (1초당 hp+75)
                    playerPV.RPC("photonStatSet", RpcTarget.All, "healthRegeneration", healthRegenValue);
                    ///이동속도 상승 (+1.5)
                    playerPV.RPC("photonStatSet", RpcTarget.All, "speed", speedValue);
                    ///공속 상승 (+0.2)
                    playerPV.RPC("photonStatSet", RpcTarget.All, "attackSpeed", attackSpeedValue);
                    ///공격력 상승 (+30)
                    playerPV.RPC("photonStatSet", RpcTarget.All, "basicAttackPower", powerValue.Item1);

                    invincibleTime = false;
                    startEffect = 0.01f;
                    effectTime = 5.5f;

                    return;

                ///5.5초동안 체력 회복량, 이동속도, 공속, 공격력 상승
                case false:
                    ///체력 회복량 상승 (1초당 hp+75)
                    playerPV.RPC("photonStatSet", RpcTarget.All, "healthRegeneration", -healthRegenValue);
                    ///이동속도 상승 (+1.5)
                    playerPV.RPC("photonStatSet", RpcTarget.All, "speed", -speedValue);
                    ///공속 상승 (+0.2)
                    playerPV.RPC("photonStatSet", RpcTarget.All, "attackSpeed", -attackSpeedValue);
                    ///공격력 상승 (+30)
                    playerPV.RPC("photonStatSet", RpcTarget.All, "basicAttackPower", -powerValue.Item1);

                    Destroy(gameObject);

                    return;
            }
        }
    }
}
