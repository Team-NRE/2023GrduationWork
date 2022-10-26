using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectController : MonoBehaviour
{
    public string status;
    public string camp;

    [Header ("- Stats Script")]
    [SerializeField] Stats stats;

    [Header ("- Children Script")]
    [SerializeField] objectMove moveScript;
    [SerializeField] objectAttack attackScript;
    [SerializeField] objectSummon summonScript;
    [SerializeField] objectDeath deathScript;

    void Start()
    {
        // Component 불러오기
        stats = this.GetComponent<Stats>();

        moveScript   = this.GetComponent<objectMove>();
        attackScript = this.GetComponent<objectAttack>();
        summonScript = this.GetComponent<objectSummon>();
        deathScript  = this.GetComponent<objectDeath>();
    }

    void FixedUpdate()
    {
        SetStatus();
    }

    void SetStatus()
    {
        /*
            현재 상태를 변경해주는 함수
        */

        // 상태 변경
        if (deathScript != null && status != "Death" && stats.GetStats("nowHealth") <= 0) 
        {   // 죽음 (현재 체력 체크)
            status = "Death";
            deathScript.Death();
        }
        else if (summonScript != null && status != "Summon")
        {   // 소환
            status = "Summon";
            summonScript.Summon();
        }
        else if (deathScript != null && status != "Attack" && GetNearbyEnemyObject().Length != 0)
        {   // 공격 (범위 내 적 확인)
            status = "Attack";
            attackScript.Attack();
        }
        else if (moveScript != null && status != "Move") 
        {   // 이동
            status = "Move";
            moveScript.Move();
        }
    }

    Collider[] GetNearbyEnemyObject()
    {
        /*
            공격 범위 내 적들을 구하는 함수
        */

        return Physics.OverlapSphere(
            transform.position, // 현재 위치
            stats.GetStats("attackRange"), // 공격 범위
            1 << (LayerMask.NameToLayer(camp == "Human" ? "Cyborg" : "Human")) // 적 진영
        );
    }
}
