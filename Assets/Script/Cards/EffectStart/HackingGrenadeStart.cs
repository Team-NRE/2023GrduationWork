using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class HackingGrenadeStart : BaseEffect
{
    PlayerStats enemyStats;
    protected PhotonView _pv;

    //float damage = default;
    int enemylayer = default;
    //float debuff = default;

    float saveMana = 0;
    bool isDebuff = false;

    float time = 0.0f;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        _damage = 25.0f;
        _debuff = 1.02f;
    }

    public void Update()
    {
        if (isDebuff == true)
        {
            time += Time.deltaTime;
            ManaRegenBack();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        int otherId = Managers.game.RemoteColliderId(other);
        _pv.RPC("RpcUpdate", RpcTarget.All, otherId);
    }

    [PunRPC]
    public void RpcUpdate(int otherId)
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

                oStats.nowHealth -= _damage + (pStats.basicAttackPower * 0.5f);
            }

            //Ÿ���� �� Player�� ��
            if (other.gameObject.tag == "PLAYER")
            {
                enemyStats = other.gameObject.GetComponent<PlayerStats>();
                PlayerStats pStats = player.gameObject.GetComponent<PlayerStats>();

                enemyStats.receviedDamage = (_pv.ViewID, _damage + (pStats.basicAttackPower * 0.5f));
                if (enemyStats.nowHealth <= 0) { pStats.kill += 1; }

                //HackingGrenade ī��
                if (_debuff != default)
                {
                    enemyStats.nowState = "Debuff";

                    saveMana = enemyStats.manaRegen;
                    enemyStats.manaRegen = 0;

                    isDebuff = true;
                }
            }
        }
    }

    private void ManaRegenBack()
    {
        if (time >= _debuff - 0.02f || enemyStats.nowState == "Health")
        {
            enemyStats.manaRegen = saveMana;
            enemyStats.nowState = "Health";
            time = 0;
            isDebuff = false;
        }
    }
}
