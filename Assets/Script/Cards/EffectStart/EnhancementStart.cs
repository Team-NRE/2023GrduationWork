using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class EnhancementStart : BaseEffect
{
    PlayerStats _pStats;
    protected PhotonView _pv;
    protected int _playerId;

    float basicAttackPower = default;
    float power_Time = 0.01f;

    bool start = false;

    private void Start()
    {
        _pv = GetComponent<PhotonView>();
    }

    public void StartEnhancement(int _player, float _basicAttackPower)
    {
        _playerId = _player;
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
