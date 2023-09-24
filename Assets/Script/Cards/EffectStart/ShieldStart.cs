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
    protected int _playerId;

    bool start;

    private void Start()
    {
        _pv = GetComponent<PhotonView>();
    }

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        base.CardEffectInit(userId);
        _pStats = player.GetComponent<PlayerStats>();
        defence = 50.0f;
        _playerId = userId;

        this.gameObject.transform.parent = player.transform;
        this.gameObject.transform.localPosition = new Vector3(0, 1.12f, 0);

        start = true;
    }

    public void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All, _playerId);
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
