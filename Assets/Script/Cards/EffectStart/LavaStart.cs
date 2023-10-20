using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class LavaStart : BaseEffect
{
    int playerId;
    float damage = default;
    int enemylayer = default;
    int _playerId;
    protected PhotonView _pv;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        playerId = userId;
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        _playerId = userId;
        enemylayer = player.GetComponent<PlayerStats>().enemyArea;
    }

    public void OnTriggerStay(Collider other)
    {
        int otherId = Managers.game.RemoteColliderId(other);
        if (otherId == default)
            return;
        _pv.RPC("RpcTrigger", RpcTarget.All, otherId);
    }

    [PunRPC]
    public void RpcTrigger(int otherId)
	{
        GameObject other = Managers.game.RemoteTargetFinder(otherId);
        GameObject user = Managers.game.RemoteTargetFinder(_playerId);

        if (other == null || user == null)
            return;

        if (other.gameObject.layer == enemylayer)
        {
            //타겟이 미니언, 타워일 시 
            if (!other.gameObject.CompareTag("PLAYER"))
            {
                ObjStats oStats = other.gameObject.GetComponent<ObjStats>();
                PlayerStats pStats = user.gameObject.GetComponent<PlayerStats>();

                oStats.nowHealth -= pStats.basicAttackPower * 0.02f;
            }

            //타겟이 적 Player일 시
            if (other.gameObject.CompareTag("PLAYER"))
            {
                PlayerStats enemyStats = other.gameObject.GetComponent<PlayerStats>();
                PlayerStats pStats = user.gameObject.GetComponent<PlayerStats>();

                enemyStats.receviedDamage = (playerId, pStats.basicAttackPower * 0.02f);
            }
        }
    }
}
