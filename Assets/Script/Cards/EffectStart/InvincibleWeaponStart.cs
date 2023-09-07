using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class InvincibleWeaponStart : MonoBehaviour
{
    GameObject player = null;
    float damage = default;
    int enemylayer = default;


    public void StartWeapon(string _player, float _damage, int _enemylayer)
    {
        player = GameObject.Find(_player);
        damage = _damage;
        enemylayer = _enemylayer;
    }


    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == enemylayer)
        {
            Debug.Log(other.gameObject.name);

            //타겟이 미니언, 타워일 시 
            if (other.gameObject.tag != "PLAYER")
            {
                ObjStats oStats = other.gameObject.GetComponent<ObjStats>();
                PlayerStats pStats = player.gameObject.GetComponent<PlayerStats>();

                oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.02f);
            }

            //타겟이 적 Player일 시
            if (other.gameObject.tag == "PLAYER")
            {
                PlayerStats enemyStats = other.gameObject.GetComponent<PlayerStats>();
                PlayerStats pStats = player.gameObject.GetComponent<PlayerStats>();

                enemyStats.receviedDamage = damage + (pStats.basicAttackPower * 0.02f);
                if (enemyStats.nowHealth <= 0) { pStats.kill += 1; }
            }
        }
    }
}
