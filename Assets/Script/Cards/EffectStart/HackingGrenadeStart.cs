using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class HackingGrenadeStart : BaseEffect
{
    PlayerStats enemyStats;
    PhotonView _pv;

    int playerId;
    int enemylayer = default;

    bool isDebuff = false;
    float saveMana = 0;

    float time = 0.0f;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        playerId = userId;
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        enemylayer = player.GetComponent<PlayerStats>().enemyArea;
        
        _damage = 25.0f;
        _debuff = 2.5f;
    }

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
        if (other.layer == enemylayer)
        {
            //Ÿ���� �̴Ͼ�, Ÿ���� �� 
            if (!other.CompareTag("PLAYER"))
            {
                ObjStats oStats = other.GetComponent<ObjStats>();
                PlayerStats pStats = player.GetComponent<PlayerStats>();

                oStats.nowHealth -= _damage + (pStats.basicAttackPower * 0.5f);
            }

            //Ÿ���� �� Player�� ��
            if (other.CompareTag("PLAYER"))
            {
                enemyStats = other.GetComponent<PlayerStats>();
                PlayerStats pStats = player.GetComponent<PlayerStats>();

                enemyStats.receviedDamage = (playerId, _damage + (pStats.basicAttackPower * 0.5f));

                //HackingGrenade ī��
                if (enemyStats.nowHealth > 0)
                {
                    enemyStats.nowState = "Debuff";

                    enemyStats.nowMana = 0;
                    enemyStats.manaRegen = 0;

                    isDebuff = true;
                }
            }
        }
    }


    public void Update()
    {
        if (isDebuff == true)
        {
            time += Time.deltaTime;
            ManaRegenBack();
        }
    }

    private void ManaRegenBack()
    {
        if (time >= _debuff - 0.01f || enemyStats.nowState == "Health")
        {
            enemyStats.nowState = "Health";
            enemyStats.manaRegen = 0.25f;
            
            isDebuff = false;
            time = 0;
            Destroy(gameObject);
        }
    }
}
