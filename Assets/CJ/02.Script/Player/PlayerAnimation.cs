using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimation : MonoBehaviour
{
    //public string KeyName;
    /*public string KeyCode
    {
        get { return KeyName; }
        set { value = KeyName; }

    }*/

    public enum State
    {
        IDLE, Walk, Attack, Throw1, Throw2, DIE
    }

    [Header("---Animation---")]
    Animator animator;
    bool isDie = false;
    public State state = State.IDLE;
    

    [Header("---etc---")]
    public float ButtonPushTime;

    public NavMeshAgent agent;
    PlayerManager pm;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        pm = GetComponent<PlayerManager>();

        //keyname
        //KeyName = "Q";
    }

    //시작 시
    private void OnEnable()
    {
        state = State.IDLE;
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
            state = agent.remainingDistance < 0.2f ? State.IDLE : State.Walk;

            //if (Input.GetButtonDown("Attack")) { state = State.Attack; }

            //if (Input.GetButton(KeyCode)) { state = State.Throw1; }

            //HP < 0 이면 죽음 상태
            if (pm.nowHealth <= 0) { state = State.DIE; }
        }
    }

    IEnumerator PlayerAnim()
    {
        while (!isDie)
        {
            switch (state)
            {
                case State.IDLE:
                    animator.SetBool("IsIdle", true);
                    animator.SetBool("IsWalk", false);
                    animator.SetBool("IsThrow1", false);

                    break;

                case State.Walk:
                    animator.SetBool("IsWalk", true);
                    animator.SetBool("IsIdle", false);
                    animator.SetBool("IsThrow1", false);

                    break;

                case State.Attack:
                    animator.SetTrigger("Fire");
                    animator.SetBool("IsIdle", false);

                    break;

                case State.Throw1:
                    animator.SetBool("IsThrow1", true);
                    animator.SetBool("IsIdle", false);

                    break;

                case State.Throw2:
                    animator.SetTrigger("Throw2");
                    animator.SetBool("IsIdle", false);

                    break;

                case State.DIE:
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
