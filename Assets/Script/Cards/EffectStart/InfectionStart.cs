using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class InfectionStart : BaseEffect
{
    int playerId;
    float damage = default;
    int enemylayer = default;
    protected PhotonView _pv;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        playerId = userId;
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        PlayerStats stat = player.GetComponent<PlayerStats>();
        damage = 0.1f;
        enemylayer = stat.enemyArea;
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
        if (other.gameObject.layer == enemylayer)
        {
            Debug.Log(other.gameObject.name);

            //Ÿ���� �̴Ͼ�, Ÿ���� �� 
            if (other.gameObject.tag != "PLAYER")
            {
                ObjStats oStats = other.gameObject.GetComponent<ObjStats>();
                PlayerStats pStats = player.gameObject.GetComponent<PlayerStats>();

                oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.05f);
            }

            //Ÿ���� �� Player�� ��
            if (other.gameObject.tag == "PLAYER")
            {
                PlayerStats enemyStats = other.gameObject.GetComponent<PlayerStats>();
                PlayerStats pStats = player.gameObject.GetComponent<PlayerStats>();

                enemyStats.receviedDamage = (playerId, damage + (pStats.basicAttackPower * 0.05f));
                if (enemyStats.nowHealth <= 0) { pStats.kill += 1; }
            }
        }
    }
}
