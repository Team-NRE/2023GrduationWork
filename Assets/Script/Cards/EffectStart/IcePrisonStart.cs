using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class IcePrisonStart : BaseEffect
{
    float effectTime;
    float startEffect = 0.01f;
    protected PhotonView _pv;
    protected PhotonView _playerPV;

    PlayerStats pStat;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        effectTime = 3.0f;
        pStat = player.GetComponent<PlayerStats>();
        _playerPV = player.GetComponent<PhotonView>();

        this.gameObject.transform.parent = player.transform;
        this.gameObject.transform.localPosition = new Vector3(0, 0.3f, 0);

        _speed = pStat.speed;
        _playerPV.RPC("photonStatSet", RpcTarget.All, "defensePower", 9999f);
        _playerPV.RPC("photonStatSet", RpcTarget.All, "speed", -_speed);
    }

    private void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All);
    }

    [PunRPC]
    public void RpcUpdate()
	{
        startEffect += Time.deltaTime;

        if (startEffect > effectTime - 0.01f)
        {
            _playerPV.RPC("photonStatSet", RpcTarget.All, "defensePower", -9999f);
            _playerPV.RPC("photonStatSet", RpcTarget.All, "speed", _speed);

            PhotonNetwork.Destroy(gameObject);
        }
    }
}
