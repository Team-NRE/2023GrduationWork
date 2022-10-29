using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class objectMove : MonoBehaviour
{
    public NavMeshAgent nav;
    public Stats stats;
    public Transform moveTarget;

    public GameObject milestone, enemyCamp;

    void Start()
    {
        stats = GetComponent<Stats>();
        nav = GetComponent<NavMeshAgent>();
        milestone = GameObject.Find("LowWayMilestone");
        enemyCamp = GameObject.Find("Cyborg Camp");
    }

    public void Move(string nowArea)
    {
        switch (nowArea)
        {
            case "tilePalette_4":   // 몹이 길에 있음
                nav.destination = (isReachMilestone()) ? enemyCamp.transform.position : milestone.transform.position;
                break;
            case "tilePalette_7":   // 몹이 길보다 높은 곳에 있음
                nav.destination = transform.position + Vector3.back;
                break;
            case "tilePalette_2":   // 몹이 길보다 낮은 곳에 있음
                nav.destination = transform.position + Vector3.forward;
                break;
            case "tilePalette_0":   // 몹이 길을 이탈함
            default:
                nav.destination = (isReachMilestone()) ? enemyCamp.transform.position : milestone.transform.position;
                break;
        }


    }

    private bool isReachMilestone()
    {
        return (transform.position.x >= 0);
    }
}
