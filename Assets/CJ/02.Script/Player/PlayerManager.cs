using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class PlayerManager : MonoBehaviour
{
    public string KeyName;
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


    [Header("---Camera---")]
    //카메라 z축
    [Range(2.0f, 100.0f)]
    public float Cam_Z;
    //카메라 y축
    [Range(0.0f, 100.0f)]
    public float Cam_Y;
    public Vector3 MousePos;


    [Header("---Move Ignore Layer---")]
    public LayerMask Ignorelayer;


    [Header("---PlaneScale---")]
    public float planescale;


    [Header("---etc---")]
    public float ButtonPushTime;
    bool checkAttack = true;
    public LayerMask layerMask;

    private void Awake()
    {
        //공격사거리 세팅
        Projector projector = AttackRangeimg.GetComponent<Projector>();
        projector.orthographicSize = attackRange;
        GetAttackRange();
        
        //Move.cs
        transform = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.updateRotation = false;

        KeyName = "Q";
    }

    private void OnEnable()
    {
        state = State.IDLE;
    }

    void Update()
    {
        //Setting.cs
        KeyMapping(); //키 맵핑

        StartCoroutine(CheckAttackRoutine());
        StartCoroutine(CheckPlayerState());
        StartCoroutine(PlayerAnim());

        remainDistance = agent.remainingDistance;
        if (nowHealth <= 0) { state = State.DIE; }

    }

    IEnumerator CheckAttackRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        if (checkAttack == true)
        {
            Attack_Detection(transform.position, attackRange);
        }
    }

    IEnumerator CheckPlayerState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);

            state = agent.remainingDistance < 0.2f ? State.IDLE : State.Walk;

            //if (Input.GetButtonDown("Attack")) { state = State.Attack; }

            //if (Input.GetButton(KeyCode)) { state = State.Throw1; }
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
                    animator.SetBool("IsThrow1",false);

                    break;

                case State.Walk:
                    animator.SetBool("IsWalk", true);
                    animator.SetBool("IsIdle", false);
                    animator.SetBool("IsThrow1",false);
                    
                    break;

                case State.Attack:
                    animator.SetTrigger("Fire");
                    animator.SetBool("IsIdle", false);

                    break;

                case State.Throw1:
                    animator.SetBool("IsThrow1",true);
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


    void LateUpdate()
    {
        //Move.cs
        CameraMove(); //카메라 움직임
    }
}