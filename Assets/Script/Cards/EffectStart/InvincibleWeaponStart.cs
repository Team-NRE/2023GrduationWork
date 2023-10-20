using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class InvincibleWeaponStart : BaseEffect
{
    int playerId;
    int enemylayer = default;

    protected PhotonView _pv;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        playerId = userId;
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);

        enemylayer = player.GetComponent<PlayerStats>().enemyArea;
    }

    public void OnTriggerStay(Collider other)
    {
        int otherId =  Managers.game.RemoteColliderId(other);
        if (otherId == default)
            return;
        _pv.RPC("RpcTrigger", RpcTarget.All, otherId);
    }

    [PunRPC]
    public void RpcTrigger(int targetId)
	{
        GameObject other = GetRemotePlayer(targetId);
        if (other.gameObject.layer == enemylayer)
        {
            //타겟이 미니언, 타워일 시 
            if (other.gameObject.tag != "PLAYER")
            {
                ObjStats oStats = other.gameObject.GetComponent<ObjStats>();
                PlayerStats pStats = player.gameObject.GetComponent<PlayerStats>();

                oStats.nowHealth -= pStats.basicAttackPower * 0.03f;
            }

            //타겟이 적 Player일 시
            if (other.gameObject.tag == "PLAYER")
            {
                PlayerStats enemyStats = other.gameObject.GetComponent<PlayerStats>();
                PlayerStats pStats = player.gameObject.GetComponent<PlayerStats>();

                enemyStats.receviedDamage = (playerId, pStats.basicAttackPower * 0.03f);
            }
        }
    }
}
