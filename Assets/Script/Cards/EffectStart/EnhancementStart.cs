using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class EnhancementStart : MonoBehaviour
{
    PlayerStats _pStats;
    PhotonView _pv;

    float basicAttackPower = default;
    float power_Time = 0.01f;

    bool start = false;

    public void StartEnhancement(int _player, float _basicAttackPower)
    {
        //_pStats = GameObject.Find(_player).GetComponent<PlayerStats>();
        _pStats = Managers.game.RemoteTargetFinder(_player).GetComponent<PlayerStats>();
        basicAttackPower = _basicAttackPower;

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
            power_Time += Time.deltaTime;

            if (power_Time >= 4.99f)
            {
                _pStats.basicAttackPower -= basicAttackPower;
                start = false;
            }
        }
    }
}
