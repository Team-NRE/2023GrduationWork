using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Enums;

public class PlayerManager : MonoBehaviour
{
    [Header("---Instance---")]
    public static PlayerManager Player_Instance;
    public PlayerMove player_move;
    public PlayerAttack player_att;
    public PlayerKey player_key;
    public PlayerStats player_stats;

    [Header("---.etc---")]
    //스탯 참조
    public Status status;

    //초기화
    public NavMeshAgent agent;
    public Animator animator;
    
    //bool
    bool isDie = false; //플레이어 사망 여부
 
    private void Awake()
    {
        Player_Instance = this;

        //자식객체 player의 컴포넌트 초기화
        animator = GetComponentInChildren<Animator>();
        agent = GetComponentInChildren<NavMeshAgent>();
    }

    //시작 시
    private void OnEnable()
    {
        status = Status.IDLE;
    }

    public void Update()
    {
        //플레이어 상태 체크
        StartCoroutine(CheckPlayerState());

        //플레이어 애니메이션 체크
        StartCoroutine(PlayerAnim());  
    }

    IEnumerator CheckPlayerState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);

            //남은 거리로 Walk/IDLE 판별
            status = agent.remainingDistance < 0.2f ? Status.IDLE : Status.Walk;

            //if (Input.GetKeyDown(KeyCode.A)) { status = Status.Attack; }

            //if (Input.GetKeyDown(KeyCode.Q)) { status = Status.Throw1; }

            //HP < 0 이면 죽음 상태
            if (player_stats.nowHealth <= 0) { status = Status.DIE; }
        }
    }

    IEnumerator PlayerAnim()
    {
        while (!isDie)
        {
            switch (status)
            {
                case Status.IDLE:
                    animator.SetBool("IsIdle", true);
                    animator.SetBool("IsWalk", false);
                    animator.SetBool("IsThrow1", false);

                    break;

                case Status.Walk:
                    animator.SetBool("IsWalk", true);
                    animator.SetBool("IsIdle", false);
                    animator.SetBool("IsThrow1", false);

                    break;

                case Status.Attack:
                    animator.SetTrigger("Fire");
                    animator.SetBool("IsIdle", false);

                    break;

                case Status.Throw1:
                    animator.SetBool("IsThrow1", true);
                    animator.SetBool("IsIdle", false);

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
            yield return new WaitForSeconds(0.3f);
        }
    } 


}