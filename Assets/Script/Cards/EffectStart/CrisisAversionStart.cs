using Define;
using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrisisAversionStart : BaseEffect
{
    PhotonView _pv;
    PhotonView _playerPV;

    float effectTime;
    float startEffect = 0.01f;

    bool invincibleTime;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);

        _playerPV = player.GetComponent<PhotonView>();

        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 0.3f, 0);

        effectTime = 1.5f;
        
        invincibleTime = true;
        _playerPV.RPC("photonStatSet", RpcTarget.All, "defensePower", 9999f);
    }

    private void Update()
    {
        startEffect += Time.deltaTime;

        if (startEffect > effectTime - 0.01f)
        {
            switch (invincibleTime)
            {
                case true:
                    //무적 끝
                    _playerPV.RPC("photonStatSet", RpcTarget.All, "defensePower", -9999f);

                    //체력 회복량 상승 (1초당 hp+75)
                    _playerPV.RPC("photonStatSet", RpcTarget.All, "healthRegeneration", 75f);
                    //이동속도 상승 (+1.5)
                    _playerPV.RPC("photonStatSet", RpcTarget.All, "speed", 1.5f);
                    //공속 상승 (+0.2)
                    _playerPV.RPC("photonStatSet", RpcTarget.All, "attackSpeed", 0.2f);
                    //공격력 상승 (+30)
                    _playerPV.RPC("photonStatSet", RpcTarget.All, "basicAttackPower", 30f);

                    invincibleTime = false;
                    startEffect = 0.01f;
                    effectTime = 5.5f;

                    return;

                case false:
                    //체력 회복량 상승 (1초당 hp+75)
                    _playerPV.RPC("photonStatSet", RpcTarget.All, "healthRegeneration", -75f);
                    //이동속도 상승 (+1.5)
                    _playerPV.RPC("photonStatSet", RpcTarget.All, "speed", -1.5f);
                    //공속 상승 (+0.2)
                    _playerPV.RPC("photonStatSet", RpcTarget.All, "attackSpeed", -0.2f);
                    //공격력 상승 (+30)
                    _playerPV.RPC("photonStatSet", RpcTarget.All, "basicAttackPower", -30f);

                    Destroy(gameObject);

                    return;
            }
        }
    }
}
