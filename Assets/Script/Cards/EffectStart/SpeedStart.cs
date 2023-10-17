using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class SpeedStart : BaseEffect
{
    PhotonView _playerPV;

    float effectTime;
    float startEffect = 0.01f;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        ///초기화
        base.CardEffectInit(userId);
        _playerPV = player.GetComponent<PhotonView>();

        ///effect 위치
        this.gameObject.transform.parent = player.transform;
        this.gameObject.transform.localPosition = new Vector3(0, 0.2f, 0);

        ///스텟 적용 시간
        effectTime = 3.5f;

        ///스텟 적용
        _speed = 0.5f;
        _playerPV.RPC("photonStatSet", RpcTarget.All, "speed", _speed);
    }

    public void Update()
    {
        startEffect += Time.deltaTime;

        if (startEffect >= effectTime - 0.01f)
        {
            _playerPV.RPC("photonStatSet", RpcTarget.All, "speed", -_speed);

            Destroy(gameObject);

            return;
        }
    }
}
