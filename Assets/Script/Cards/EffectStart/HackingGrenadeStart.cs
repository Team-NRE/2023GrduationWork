using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class HackingGrenadeStart : BaseEffect
{
    PhotonView _pv;

    int playerId;
    int enemylayer = default;

    float damage = default;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        base.CardEffectInit(userId);
        _pv = GetComponent<PhotonView>();
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
    public void RpcTrigger(int otherId)
    {
        GameObject other = Managers.game.RemoteTargetFinder(otherId);
        if (other.layer != enemylayer) return;
        if (other.layer == enemylayer)
        {
            if(!other.CompareTag("PLAYER"))
            {
                ObjStats oStats = other.GetComponent<ObjStats>();
                PlayerStats pStats = player.GetComponent<PlayerStats>();

                oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.5f);
            }

            if (other.CompareTag("PLAYER"))
            {
                PlayerStats enemyStats = other.GetComponent<PlayerStats>();
                PlayerStats pStats = player.GetComponent<PlayerStats>();

                enemyStats.receviedDamage = (playerId, damage + (pStats.basicAttackPower * 0.5f));

                GameObject HackingEffect = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_HackingGrenade2", other.transform.position, Quaternion.identity);
                HackingEffect.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId, otherId);
            }
        }
    }
}
