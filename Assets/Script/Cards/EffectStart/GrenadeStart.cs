using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeStart : BaseEffect
{
    protected PhotonView _pv;
    protected int _layer;
    int _playerId;

    void Start()
    {

    }

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        PlayerStats stats = player.GetComponent<PlayerStats>();
        _layer = stats.playerArea;
        _playerId = userId;

        _damage = 25;
        _debuff = 1.02f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        int otherId = Managers.game.RemoteColliderId(other);
        if (otherId == default)
            return;
        _pv.RPC("RpcUpdate", RpcTarget.All, otherId, _playerId);
    }

    [PunRPC]
    public void RpcUpdate(int otherId, int playerId)
    {
        GameObject other = Managers.game.RemoteTargetFinder(otherId);
        GameObject user = Managers.game.RemoteTargetFinder(playerId);
        if (other.gameObject.tag == "OBJECT")
        {
            if (other.gameObject.layer != _layer)
            {
                ObjStats o_stats = other.GetComponent<ObjStats>();
                PlayerStats p_stats = user.gameObject.GetComponent<PlayerStats>();

                o_stats.nowHealth -= _damage + (p_stats.basicAttackPower * 0.5f);
            }
        }
        else if (other.gameObject.tag == "PLAYER")
        {
            if (other.gameObject.layer != _layer)
            {
                PlayerStats e_stats = other.GetComponent<PlayerStats>();
                PlayerStats p_stats = user.GetComponent<PlayerStats>();

                e_stats.receviedDamage = (playerId, _damage + (p_stats.basicAttackPower * 0.5f));
            }
        }
    }
}
