using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class objectAttack : MonoBehaviour
{
    [Header ("- Stats Script")]
    [SerializeField] Stats stats;

    [Header ("- Attack Target")]
    [SerializeField] GameObject target;

    [Header ("- Components")]
    [SerializeField] NavMeshAgent nav;
    [SerializeField] Animator animator;

    [Header ("- Variable")]
    [SerializeField] bool isAttack;

    void Start()
    {
        // Component 불러오기
        stats = GetComponent<Stats>();
        nav = GetComponent<NavMeshAgent>();

        isAttack = false;
    }

    public void Attack(string nowArea, Collider[] enemyList)
    {
        target = GetTarget(enemyList);

        if (Vector3.Distance(transform.position, target.transform.position) > stats.GetStats("attackRange"))
        {
            FollowTarget();
        }
        else
        {
            PlayAttack();
        }
    }

    GameObject GetTarget(Collider[] enemyList)
    {
        return enemyList[0]?.gameObject;
    }

    void FollowTarget()
    {
        nav.destination = target.transform.position;
        animator.SetBool("Move", true);
    }
    
    void PlayAttack()
    {
        if (!isAttack && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
        {
            TakeDamage();
            isAttack = true;
        }

        if (stats.GetStats("attackCoolingTime") <= 0)
        {
            // Animation 설정
            animator.SetBool("Move", false);
            animator.SetTrigger("Attack");

            // 공격 쿨링 타임 초기화
            stats.SetStats("attackCoolingTime", 1 / stats.GetStats("attackSpeed"));
            isAttack = false;
        }
    }

    public void TakeDamage()
    {
        target.GetComponent<Stats>().AddStats("nowHealth", -stats.GetStats("attackPower"));
        Debug.Log("Hit!");
    }
}
