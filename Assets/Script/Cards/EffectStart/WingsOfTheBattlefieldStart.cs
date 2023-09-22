using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsOfTheBattlefieldStart : BaseEffect
{
    PlayerStats _pStats;
    protected PhotonView _pv;

    float speed = default;
    float speed_Time = 0.01f;
    float effectTime = default;

    bool start = false;

    public void StartWings(string _player, float _speed, float _effectTime)
    {
        _pStats = GameObject.Find(_player).GetComponent<PlayerStats>();
        speed = _speed;
        effectTime = _effectTime;

        start = true;
    }

    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        speed = 2.0f;
        effectTime = 1.1f;
    }


    public void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All);
    }

    [PunRPC]
    public void RpcUpdate()
	{
        if (start == true)
        {
            speed_Time += Time.deltaTime;

            if (speed_Time >= effectTime - 0.01f)
            {
                _pStats.speed -= speed;
                start = false;

                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
