using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using UnityEngine.UIElements;
using Photon.Pun;

public class InvincibleShieldStart : BaseEffect
{
    void Start()
    {
        //초기화
        player = transform.parent.gameObject;
        playerPV = player.GetComponent<PhotonView>();

        //스텟 적용 시간
        effectTime = 3.0f;
        startEffect = 0.01f;

        //RPC 적용
        playerPV.RPC("photonStatSet", RpcTarget.All, "defensePower", 9999f);
    }

    private void Update()
    {
        startEffect += Time.deltaTime;

        //스텟 적용 종료
        if (startEffect > effectTime - 0.01f)
        {
            playerPV.RPC("photonStatSet", RpcTarget.All, "defensePower", -9999f);

            Destroy(gameObject);

            return;
        }
    }
}
