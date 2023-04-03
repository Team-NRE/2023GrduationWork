using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class objectDeath : MonoBehaviour
{
    [Header ("- Components")]
    [SerializeField] objectController controller;
    [SerializeField] NavMeshAgent nav;
    [SerializeField] Animator animator;

    [Header ("- Variable")]
    [SerializeField] bool isDeath;

    void Start()
    {
        // Component 불러오기
        controller = GetComponent<objectController>();
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
