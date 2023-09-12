using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsOfTheBattlefieldStart : MonoBehaviour
{
    PlayerStats _pStats;
    PhotonView _pv;

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

                Destroy(gameObject);
            }
        }
    }
}
