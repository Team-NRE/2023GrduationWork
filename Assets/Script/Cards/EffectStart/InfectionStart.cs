using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class InfectionStart : BaseEffect
{
    int _playerId;
    int enemylayer = default;

    protected PhotonView _pv;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        _playerId = userId;
        PlayerStats stat = player.GetComponent<PlayerStats>();
        enemylayer = stat.enemyArea;
    }

    public void OnTriggerStay(Collider other)
    {
        int otherId = Managers.game.RemoteColliderId(other);
        if (otherId == default)
            return; 
        _pv.RPC("RpcTrigger", RpcTarget.All, _playerId, otherId);   
    }

    [PunRPC]
    public void RpcTrigger(int playerId, int otherId)
	{
        if (otherId == default)
            return;

        GameObject other = Managers.game.RemoteTargetFinder(otherId);
        GameObject player = Managers.game.RemoteTargetFinder(playerId);

        if (other == null || player == null)
            return;

        if (other.gameObject.layer == enemylayer)
        {
            if (other.gameObject.tag != "PLAYER")
            {
                ObjStats oStats = other.gameObject.GetComponent<ObjStats>();
                PlayerStats pStats = player.gameObject.GetComponent<PlayerStats>();

                oStats.nowHealth -= pStats.basicAttackPower * 0.01f;
            }

            //Ÿ���� �� Player�� ��
            if (other.gameObject.tag == "PLAYER")
            {
                PlayerStats enemyStats = other.gameObject.GetComponent<PlayerStats>();
                PlayerStats pStats = player.gameObject.GetComponent<PlayerStats>();

                enemyStats.receviedDamage = (playerId, pStats.basicAttackPower * 0.01f);
            }
        }
    }
}
