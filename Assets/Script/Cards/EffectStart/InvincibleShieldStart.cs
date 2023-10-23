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
        //�ʱ�ȭ
        player = transform.parent.gameObject;
        playerPV = player.GetComponent<PhotonView>();

        //���� ���� �ð�
        effectTime = 3.0f;
        startEffect = 0.01f;

        //RPC ����
        playerPV.RPC("photonStatSet", RpcTarget.All, "defensePower", 9999f);
    }

    private void Update()
    {
        startEffect += Time.deltaTime;

        //���� ���� ����
        if (startEffect > effectTime - 0.01f)
        {
            playerPV.RPC("photonStatSet", RpcTarget.All, "defensePower", -9999f);

            Destroy(gameObject);

            return;
        }
    }
}
