using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using UnityEngine.UIElements;
using Photon.Pun;

public class InvincibleShieldStart : BaseEffect
{
    float effectTime;
    float startEffect = 0.01f;

    protected PhotonView _playerPV;

    void Start()
    {
        player = transform.parent.gameObject;
        _playerPV = player.GetComponent<PhotonView>();

        effectTime = 3.0f;

        _playerPV.RPC("photonStatSet", RpcTarget.All, "defensePower", 9999f);
    }

    private void Update()
    {
        startEffect += Time.deltaTime;

        if (startEffect > effectTime - 0.01f)
        {
            _playerPV.RPC("photonStatSet", RpcTarget.All, "defensePower", -9999f);

            Destroy(gameObject);

            return;
        }
    }
}
