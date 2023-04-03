using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Enums;

public class PlayerAnimation : MonoBehaviour
{
    //public string KeyName;
    /*public string KeyCode
    {
        get { return KeyName; }
        set { value = KeyName; }

    }*/

    //스탯 참조
    public Status status;

    [Header("---Animation---")]
    Animator animator;
    bool isDie = false;
    
    [Header("---etc---")]
    public float ButtonPushTime;

    public NavMeshAgent agent;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
        //keyname
        //KeyName = "Q";
    }

    //시작 시
    private void OnEnable()
    {
        status = Status.IDLE;
    }

    void Update()
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

            //if (Input.GetButtonDown("Attack")) { status = Status.Attack; }

            //if (Input.GetButton(KeyCode)) { state = State.Throw1; }

            //HP < 0 이면 죽음 상태
            if (PlayerManager.Player_Instance.player_stats.nowHealth <= 0) { status = Status.DIE; }
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
