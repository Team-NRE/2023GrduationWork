using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class SpeedStart : BaseEffect
{
    PlayerStats _pStats;
    protected PhotonView _pv;

    float speed = default;
    float speed_Time = 0.01f;

    bool start = false;

    public void StartSpeed(int _player, float _speed)
    {
        //_pStats = GameObject.Find(_player).GetComponent<PlayerStats>();
        _pStats = Managers.game.RemoteTargetFinder(_player).GetComponent<PlayerStats>();
        speed = _speed;
        _pv = GetComponent<PhotonView>();

        start = true;
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

            if (speed_Time >= 4.99f)
            {
                _pStats.speed -= speed;
                start = false;
            }
        }
    }
}
