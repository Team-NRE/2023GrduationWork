using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class LavaStart : BaseEffect
{
    float damage = default;
    int enemylayer = default;
    protected PhotonView _pv;

    [PunRPC]
    public override void CardEffectInit(int userId, int targetId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId, targetId);
        enemylayer = target.gameObject.layer;
        damage = 0.1f;
    }

    public void OnTriggerStay(Collider other)
    {
        int otherId = Managers.game.RemoteColliderId(other);
        if (otherId == default)
            return;
        _pv.RPC("RpcTrigger", RpcTarget.All, otherId);
    }

    [PunRPC]
    public void RpcTrigger(int otherId, int playerId)
	{
        GameObject other = Managers.game.RemoteTargetFinder(otherId);
        GameObject user = Managers.game.RemoteTargetFinder(playerId);

        if (other.gameObject.layer == enemylayer)
        {
            Debug.Log(other.gameObject.name);

            //타겟이 미니언, 타워일 시 
            if (other.gameObject.tag != "PLAYER")
            {
                ObjStats oStats = other.gameObject.GetComponent<ObjStats>();
                PlayerStats pStats = user.gameObject.GetComponent<PlayerStats>();

                oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.01f);
            }

            //타겟이 적 Player일 시
            if (other.gameObject.tag == "PLAYER")
            {
                PlayerStats enemyStats = other.gameObject.GetComponent<PlayerStats>();
                PlayerStats pStats = user.gameObject.GetComponent<PlayerStats>();

                enemyStats.receviedDamage = damage + (pStats.basicAttackPower * 0.01f);
                if (enemyStats.nowHealth <= 0) { pStats.kill += 1; }
            }
        }
    }
}
