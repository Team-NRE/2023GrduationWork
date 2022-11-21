using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class objectDeath : MonoBehaviour
{
    [Header ("- Stats Script")]
    [SerializeField] Stats stats;

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

        isDeath = false;
    }

    void FixedUpdate()
    {
        if (isDeath && animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
            Destroy(gameObject);
    }

    public void Death()
    {
        if (controller != null) controller.enabled = false;
        if (nav != null) nav.enabled = false;

        animator?.SetTrigger("Death");
        animator?.SetBool("Move", false);
        isDeath = true;
    }
}
