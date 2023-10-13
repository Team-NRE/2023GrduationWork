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

    bool isDebuff;

    float time; 
    float damage = default;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        playerId = userId;
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        enemylayer = player.GetComponent<PlayerStats>().enemyArea;

        isDebuff = false;

        time = 0.01f;
        damage = 25.0f;
        _debuff = 2.5f;
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

                if(enemyStats.nowHealth > 0)
                {
                    // 같은 팀원에게 새로운 effect 인스턴스 화
                    GameObject HackingEffect = Managers.Resource.Instantiate($"Particle/Effect_HackingGrenade2", other.transform);
                    HackingEffect.transform.localPosition = new Vector3(0, 1.2f, 0);
                    
                    enemyStats.nowState = "Debuff";
                    enemyStats.nowMana = 0f;
                    enemyStats.manaRegen = 0f;

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
