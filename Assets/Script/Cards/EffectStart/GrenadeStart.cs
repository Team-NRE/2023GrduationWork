using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class GrenadeStart : MonoBehaviour
{
    PlayerStats enemyStats;

    GameObject player = null;
    float damage = default;
    int enemylayer = default;
    float debuff = default;
    
    float saveMana = 0;
    bool isDebuff = false;

    float time = 0.0f;

    public void StartGrenade(string _player, float _damage, LayerMask _enemylayer, float _debuff = default)
    {
        player = GameObject.Find(_player);
        damage = _damage;
        enemylayer = _enemylayer;
        debuff = _debuff;
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
        if (other.gameObject.layer == enemylayer)
        {
            Debug.Log(other.gameObject.name);

            //타겟이 미니언, 타워일 시 
            if (other.gameObject.tag != "PLAYER")
            {
                ObjStats oStats = other.gameObject.GetComponent<ObjStats>();
                PlayerStats pStats = player.gameObject.GetComponent<PlayerStats>();

                oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.5f);
            }

            //타겟이 적 Player일 시
            if (other.gameObject.tag == "PLAYER")
            {
                enemyStats = other.gameObject.GetComponent<PlayerStats>();
                PlayerStats pStats = player.gameObject.GetComponent<PlayerStats>();

                enemyStats.nowHealth -= damage + (pStats.basicAttackPower * 0.5f);
                if (enemyStats.nowHealth <= 0) { pStats.kill += 1; }
                
                //HackingGrenade 카드
                if (debuff != default)
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
        if (time >= debuff - 0.02f || enemyStats.nowState == "Health")
        {
            enemyStats.manaRegen = saveMana;
            enemyStats.nowState = "Health";
            time = 0;
            isDebuff = false;
        }
    }
}
