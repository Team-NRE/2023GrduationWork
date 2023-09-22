using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class ShieldStart : BaseEffect
{
    PlayerStats _pStats;
    float defence = default;
    float shield_Time = 0.01f;
    protected PhotonView _pv;

    bool start;

    private void Start()
    {
        _pv = GetComponent<PhotonView>();

    }

    public void StartShield(int _player, float _defence)
    {
        //_pStats = GameObject.Find(_player).GetComponent<PlayerStats>();
        _pStats = Managers.game.RemoteTargetFinder(_player).GetComponent<PlayerStats>();
        defence = _defence;
        //_pv = GetComponent<PhotonView>();
        start = true;
    }

    public void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All);
    }

    [PunRPC]
    public void RpcUpdate(int playerId)
	{
        if (start == true)
        {
            shield_Time += Time.deltaTime;

            if (shield_Time >= 1.9f)
            {
                _pStats.defensePower -= defence;
                start = false;
            }
        }
    }
}
