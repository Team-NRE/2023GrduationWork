/// ksPark
///
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Stat;

public class Missile : MonoBehaviour
{

    static List<Transform> _allObjectTransforms;
    float damage;
    float distance = 5.0f;

    [SerializeField]
    GameObject explosionParticle;

    public void SummonMissile(List<Transform> objList, Vector3 pos, float dam, float dis = 5.0f)
    {
        _allObjectTransforms = objList;
        transform.position = pos;
        damage = dam;
        distance = dis;

        this.gameObject.SetActive(true);
    }

    public void TakeDamage()
    {
        explosionParticle.SetActive(true);

        for (int i=0; i<_allObjectTransforms.Count; i++)
        {
            Transform nowTarget = _allObjectTransforms[i];

            if (Vector3.Distance(transform.position, nowTarget.position) <= distance)
            {
                //타겟이 미니언, 타워일 시 
                if (nowTarget.tag != "PLAYER")
                {
                    ObjStats _Stats = nowTarget.GetComponent<ObjStats>();
                    _Stats.nowHealth -= damage;
                }

                //타겟이 적 Player일 시
                if (nowTarget.tag == "PLAYER")
                {
                    PlayerStats _Stats = nowTarget.GetComponent<PlayerStats>();
                    _Stats.nowHealth -= damage;
                }
            }
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
