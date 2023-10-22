using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class ChargeStart : BaseEffect
{
    float _powerValue;

    float effectTime;
    float startEffect;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        ///초기화
        base.CardEffectInit(userId);
        
        ///effect 위치
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 0.2f, 0);

        ///스텟 적용 시간
        effectTime = 2.0f;
        startEffect = 0.01f;

        ///스텟 적용
        _speed = 0.5f;
        _powerValue = 10.0f;
        playerPV.RPC("photonStatSet", RpcTarget.All, "speed", _speed);
        playerPV.RPC("photonStatSet", RpcTarget.All, "basicAttackPower", _powerValue);
    }

    private void Update()
    {
        startEffect += Time.deltaTime;

        ///스텟 적용 종료
        if (startEffect > effectTime - 0.01f)
        {
            playerPV.RPC("photonStatSet", RpcTarget.All, "speed", -_speed);
            playerPV.RPC("photonStatSet", RpcTarget.All, "basicAttackPower", -_powerValue);

            Destroy(gameObject);

            return;
        }
    }
}
