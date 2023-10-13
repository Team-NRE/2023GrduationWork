using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeStart : BaseEffect
{
    float damage = default;
    int enemylayer = default;
    int playerId;
    protected PhotonView _pv;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        playerId = userId;
        base.CardEffectInit(userId);

        enemylayer = player.GetComponent<PlayerStats>().enemyArea;
        damage = 25.0f;
    }

    // 여긴 백퍼 버그가 발생할 예정
    public void OnTriggerEnter(Collider other)
    {
        int otherId = Managers.game.RemoteColliderId(other);
        if (otherId == default)
            return;
        _pv.RPC("RpcTrigger", RpcTarget.All, otherId);
    }

    [PunRPC]
    public void RpcUpdate(int otherId)
    {
         GameObject other = Managers.game.RemoteTargetFinder(otherId);
        if (other.layer == enemylayer)
        {
            //타겟이 미니언, 타워일 시 
            if (!other.CompareTag("PLAYER"))
            {
                ObjStats oStats = other.GetComponent<ObjStats>();
                PlayerStats pStats = player.GetComponent<PlayerStats>();

                oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.5f);
            }

            //타겟이 적 Player일 시
            if (other.CompareTag("PLAYER"))
            {
                PlayerStats enemyStats = other.GetComponent<PlayerStats>();
                PlayerStats pStats = player.GetComponent<PlayerStats>();

                enemyStats.receviedDamage = (playerId, damage + (pStats.basicAttackPower * 0.5f));
            }
        }
    }
}
