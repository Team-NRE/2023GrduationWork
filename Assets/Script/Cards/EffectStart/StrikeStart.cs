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
    ObjStats oStats;

    bool IsEffect = false;
    public void StartStrike(int _player, float _damage, float _effectTime)
    {
        //player = GameObject.Find(_player);
        player = Managers.game.RemoteTargetFinder(_player);
        Obj = transform.parent.gameObject;

        damage = _damage;
        effectTime = _effectTime;
    }

    public void Update()
    {
        if (BaseCard._lockTarget == null) { Destroy(gameObject); }
        if (BaseCard._lockTarget != null)
        {
            switch (IsEffect)
            {
                case false:
                    //타겟이 미니언, 타워일 시 
                    if (Obj.tag != "PLAYER")
                    {
                        oStats = Obj.GetComponent<ObjStats>();
                        PlayerStats pStats = player.GetComponent<PlayerStats>();

                        oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.5f);

                        saveSpeed = oStats.speed;

                        oStats.speed = 0;

                        IsEffect = true;

                        break;
                    }

                    //타겟이 적 Player일 시
                    if (Obj.tag == "PLAYER")
                    {
                        enemyStats = Obj.GetComponent<PlayerStats>();
                        PlayerStats pStats = player.GetComponent<PlayerStats>();

                        enemyStats.receviedDamage = (damage + (pStats.basicAttackPower * 0.5f));
                        if (enemyStats.nowHealth <= 0) { pStats.kill += 1; }

                        saveSpeed = enemyStats.speed;
                        enemyStats.speed = 0;

                        IsEffect = true;

                        break;
                    }

                    break;

                case true:
                    StartEffect += Time.deltaTime;

                    if (StartEffect > effectTime - 0.01f)
                    {
                        //타겟이 미니언, 타워일 시 
                        if (Obj.tag != "PLAYER")
                        {
                            oStats.speed = saveSpeed;

                            Destroy(gameObject);
                        }

                        //타겟이 미니언, 타워일 시 
                        if (Obj.tag == "PLAYER")
                        {
                            enemyStats.speed = saveSpeed;

                            Destroy(gameObject);
                        }
                    }


                    break;
            }

        }
    }
}
