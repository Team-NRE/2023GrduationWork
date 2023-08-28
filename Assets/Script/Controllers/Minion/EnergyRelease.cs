/// ksPark
///
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using Stat;

public class EnergyRelease : MonoBehaviour
{
    float damage;
    float distance = 5.0f;

    [PunRPC]
    public void SummonEnergyRelease(Vector3 pos, float dam, float dis = 5.0f)
    {
        transform.position = pos;
        damage = dam * 3.5f;
        distance = dis;

        gameObject.SetActive(true);
    }

    public void TakeDamage()
    {
        Collider[] colls = Physics.OverlapSphere(
            transform.position, 
            distance,
            (1 << LayerMask.NameToLayer("Cyborg")) | (1 << LayerMask.NameToLayer("Human")) 
        );

        for (int i=0; i<colls.Length; i++) {
            Transform nowTarget = colls[i].transform;

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

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}