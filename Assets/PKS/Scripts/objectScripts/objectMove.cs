using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class objectMove : MonoBehaviour
{
    [Header ("- Components")]
    [SerializeField] NavMeshAgent nav;
    [SerializeField] Animator animator;

    public GameObject milestone, enemyCamp;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    public void Move(string nowArea, string camp)
    {
        animator?.SetBool("Move", true);

        switch (nowArea)
        {
            case "tilePalette_4":   // 몹이 길에 있음
                nav.destination = (isReachMilestone(camp)) ? enemyCamp.transform.position : milestone.transform.position;
                break;
            case "tilePalette_7":   // 몹이 길보다 높은 곳에 있음
                nav.destination = transform.position + Vector3.back;
                break;
            case "tilePalette_2":   // 몹이 길보다 낮은 곳에 있음
                nav.destination = transform.position + Vector3.forward;
                break;
            case "tilePalette_0":   // 몹이 길을 이탈함
            default:
                nav.destination = (isReachMilestone(camp)) ? enemyCamp.transform.position : milestone.transform.position;
                break;
        }
    }

    private bool isReachMilestone(string camp)
    {
        return ((camp == "Human") ? transform.position.x >= 0 : transform.position.x <= 0);
    }
}
