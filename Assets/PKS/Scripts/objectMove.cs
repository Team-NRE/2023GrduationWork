using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class objectMove : MonoBehaviour
{
    public NavMeshAgent nav;
    public Stats stats;
    public Transform moveTarget;

    void Start()
    {
        stats = GetComponent<Stats>();
        nav = gameObject.AddComponent(typeof(NavMeshAgent)) as NavMeshAgent;
    }

    public void Move()
    {


    }

    
}
