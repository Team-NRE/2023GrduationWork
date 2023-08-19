using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class StrikeStart : MonoBehaviour
{
    GameObject player = null;
    GameObject Obj = null;

    float damage = default;
    float effectTime = default;
    float StartEffect = 0.01f;
    float saveSpeed = default;

    PlayerStats enemyStats;
    public void StartStrike(string _player, float _damage, float _effectTime)
    {
        player = GameObject.Find(_player);
        Obj = transform.parent.gameObject;

        damage = _damage;
        effectTime = _effectTime;
    }

    private void Update()
    {
        if (BaseCard._lockTarget != null)
        {
            //타겟이 미니언, 타워일 시 
            if (Obj.tag != "PLAYER")
            {
                ObjStats oStats = Obj.GetComponent<ObjStats>();
                PlayerStats pStats = player.GetComponent<PlayerStats>();

                oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.5f);

                BaseCard._lockTarget = null;

            }

            //타겟이 적 Player일 시
            if (Obj.tag == "PLAYER")
            {
                enemyStats = Obj.GetComponent<PlayerStats>();
                PlayerStats pStats = player.GetComponent<PlayerStats>();

                enemyStats.nowHealth -= (damage + (pStats.basicAttackPower * 0.5f));
                if (enemyStats.nowHealth <= 0) { pStats.kill += 1; }

                BaseCard._lockTarget = null;

                saveSpeed = enemyStats.speed;
                enemyStats.speed = 0;
            }
        }

        if (enemyStats.speed == 0)
        {
            StartEffect += Time.deltaTime;

            if (StartEffect > effectTime - 0.01f)
            {
                enemyStats.speed = saveSpeed;
                Destroy(gameObject);
            }
        }

    }
}
