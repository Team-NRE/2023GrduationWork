using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using static UnityEngine.GraphicsBuffer;

public class BloodstainedCoinStart : MonoBehaviour
{
    GameObject player = null;
    
    Transform target = null;

    float damage = default;

    PlayerStats enemyStats;

    public void StartBloodstainedCoin(string _player, float _damage)
    {
        player = GameObject.Find(_player);
        target = BaseCard._lockTarget.transform;
        BaseCard._lockTarget = null;

        damage = _damage;
    }



    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.Slerp(transform.position, target.position + Vector3.up, Time.deltaTime * 4.0f);
            Vector3 thisPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 targetPos = new Vector3(target.position.x, 0, target.position.z);

            if (Vector3.Distance(thisPos, targetPos) <= 0.5f)
            {
                //effect
                GameObject _effectObject = Managers.Resource.Instantiate($"Particle/Effect_BloodstainedCoin");
                _effectObject.transform.parent = target.transform;
                _effectObject.transform.localPosition = new Vector3(0, 0.8f, 0);


                //타겟이 미니언, 타워일 시 
                if (target.tag != "PLAYER")
                {
                    ObjStats oStats = target.GetComponent<ObjStats>();
                    PlayerStats pStats = player.GetComponent<PlayerStats>();

                    oStats.nowHealth -= damage + (pStats.basicAttackPower);

                    target = null;
                }

                //타겟이 적 Player일 시
                if (target.tag == "PLAYER")
                {
                    enemyStats = target.GetComponent<PlayerStats>();
                    PlayerStats pStats = player.GetComponent<PlayerStats>();

                    enemyStats.nowHealth -= (damage + (pStats.basicAttackPower));

                    target = null;

                    if (enemyStats.nowHealth <= 0)
                    {
                        pStats.kill += 1;
                        pStats.gold += 100;
                    }
                }
                Destroy(gameObject, 0.1f);
                Destroy(_effectObject, 0.5f);
            }
        }
    }
}
