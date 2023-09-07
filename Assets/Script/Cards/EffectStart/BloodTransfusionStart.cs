using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class BloodTransfusionStart : MonoBehaviour
{
    GameObject player = null;
    GameObject Obj = null;

    float damage = default;


    public void StartBloodTransfusion(string _player, float _damage)
    {
        player = GameObject.Find(_player);
        Obj = transform.parent.gameObject;

        damage = _damage;
    }

    private void Update()
    {
        //타겟이 미니언, 타워일 시 
        if (Obj.tag != "PLAYER")
        {
            ObjStats oStats = Obj.GetComponent<ObjStats>();
            PlayerStats pStats = player.GetComponent<PlayerStats>();

            oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.7f);
            pStats.nowHealth += damage + (pStats.basicAttackPower * 0.7f);

            BaseCard._lockTarget = null;
            GetComponent<BloodTransfusionStart>().enabled = false;
        }

        //타겟이 적 Player일 시
        if (Obj.tag == "PLAYER")
        {
            PlayerStats enemyStats = Obj.GetComponent<PlayerStats>();
            PlayerStats pStats = player.GetComponent<PlayerStats>();

            enemyStats.receviedDamage = (damage + (pStats.basicAttackPower * 0.7f));
            pStats.nowHealth += damage + (pStats.basicAttackPower * 0.7f);
            if (enemyStats.nowHealth <= 0) { pStats.kill += 1; }

            BaseCard._lockTarget = null;
            GetComponent<BloodTransfusionStart>().enabled = false;
        }
    }
}
