using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectAttack : MonoBehaviour
{
    [Header ("- Stats Script")]
    [SerializeField] Stats stats;

    [Header ("- Attack Target")]
    [SerializeField] GameObject target;

    [Header ("- Components")]
    [SerializeField] UnityEngine.AI.NavMeshAgent nav;

    void Start()
    {
        // Component 불러오기
        stats = GetComponent<Stats>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void Attack(string nowArea, Collider[] enemyList)
    {
        GetTarget(enemyList);

        if (Vector3.Distance(transform.position, target.transform.position) > stats.GetStats("attackRange") * 0.01f)
        {
            nav.destination = target.transform.position;
        }
        else
        {
            PlayAttack();
        }
    }

    void GetTarget(Collider[] enemyList)
    {
        target = enemyList[0].gameObject;
    }
    
    void PlayAttack()
    {

    }
}
