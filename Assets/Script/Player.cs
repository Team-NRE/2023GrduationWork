using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Enums;
using static Define;


public class Player : BaseController
{

    //초기화
    public NavMeshAgent agent;
    public Animator animator;
    public new Transform transform;
    public PlayerStats p_Stat;
    public Status status;
    public KeyboardEvent kb;

    //이동할 점 / layer
    public Vector3 Point;
    public LayerMask Ignorelayer;

    public bool IsKey = false;


    public override void Init()
    {
        //자식객체 player의 컴포넌트 초기화
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        transform = GetComponent<Transform>();
        p_Stat = GetComponent<PlayerStats>();

        agent.acceleration = 80.0f;
        agent.updateRotation = false;
    }

    //플레이어 상태 체크
    public override void KeyDownAction(string name)
    {
        switch (name)
        {
            case "rightButton":
                status = Status.Walk;
                IsKey = true;
                playerMove();

                break;

            case "a":
                Invoke("idle", 0.4f);
                IsKey = true;
                status = Status.Attack;

                break;

            case "q":
            case "w":
            case "e":
            case "r":
                Invoke("idle", 0.4f);
                IsKey = true;
                status = Status.Throw1;
                

                break;
        }
    }

    void idle()
    {
        IsKey = false;

        if (agent.remainingDistance < 0.2f)
        {
            status = Status.IDLE;
        }
    }

    private void Update()
    {
        //HP < 0 이면 죽음 상태
        if (p_Stat.nowHealth <= 0) { status = Status.DIE; }
        if(IsKey == false) { idle(); }

        StartCoroutine(PlayerAnim());
    }

    public IEnumerator PlayerAnim()
    {
        yield return new WaitForSeconds(0.3f);

        switch (status)
        {
            case Status.IDLE:
                agent.isStopped = false;

                animator.SetBool("IsIdle", true);
                animator.SetBool("IsWalk", false);
                animator.SetBool("IsThrow1", false);
                animator.SetBool("IsFire", false);

                break;

            case Status.Walk:
                agent.isStopped = false;

                animator.SetBool("IsWalk", true);
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsThrow1", false);
                animator.SetBool("IsFire", false);

                break;

            case Status.Attack:
                agent.ResetPath();
                //agent.isStopped = true;

                animator.SetBool("IsFire", true);
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsThrow1", false);

                break;

            case Status.Throw1:
                agent.ResetPath();
                //agent.isStopped = true;

                animator.SetBool("IsThrow1", true);
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsFire", false);

                break;

            case Status.Throw2:
                animator.SetTrigger("Throw2");
                animator.SetBool("IsIdle", false);

                break;

            case Status.DIE:
                animator.SetBool("IsIdle", false);
                animator.SetTrigger("Die");
                this.enabled = false;

                StopAllCoroutines();

                break;
        }

    }


    //플레이어 이동 
    public void playerMove()
    {
        // ray로 마우스 위치 world 좌표로 받기.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //광선 그려주기
        //Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity, Color.green, 100f);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, ~(Ignorelayer)))
        {
            Point = raycastHit.point;

            //각도
            Point.y = 0f;
            float dx = Point.x - transform.position.x;
            float dz = Point.z - transform.position.z;
            float rotDegree = -(Mathf.Rad2Deg * Mathf.Atan2(dz, dx) - 90); //tan-1(dz/dx) = 각도
            //레어와 닿은 곳으로 회전
            transform.eulerAngles = new Vector3(0f, rotDegree, 0f);

            agent.SetDestination(Point);

        }
    }
}
