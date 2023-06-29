/// ksPark
///
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Stat;

public class EnergyRelease : MonoBehaviour
{
    static List<Transform> _allObjectTransforms;
    float damage;
    float distance = 5.0f;

    public void SummonEnergyRelease(List<Transform> objList, Transform pos, float dam, float dis = 5.0f)
    {
        _allObjectTransforms = objList;
        transform.position = pos.position;
        damage = dam * 3.5f;
        distance = dis;

        gameObject.SetActive(true);
    }

    public void TakeDamage()
    {
        for (int i=0; i<_allObjectTransforms.Count; i++)
        {
            Transform nowTarget = _allObjectTransforms[i];

            if (Vector3.Distance(transform.position, nowTarget.position) <= distance)
            {
                // 임시방편, 사용자인지 검사하는 코드 추가할 것.
                if (nowTarget.transform.position == transform.position) continue;

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