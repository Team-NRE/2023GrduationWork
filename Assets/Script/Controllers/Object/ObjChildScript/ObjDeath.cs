using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjDeath : MonoBehaviour
{
    [Header ("- Components")]
    [SerializeField] ObjController controller;
    [SerializeField] NavMeshAgent nav;
    [SerializeField] Animator animator;

    [Header ("- Variable")]
    [SerializeField] bool isDeath;

    void Start()
    {
        // Component 불러오기
        controller = GetComponent<ObjController>();
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        isDeath = false;
    }

    public void Death()
    {
        if (controller != null) controller.enabled = false;
        if (nav != null) nav.enabled = false;

        animator?.SetTrigger("Death");
        animator?.SetBool("Move", false);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
