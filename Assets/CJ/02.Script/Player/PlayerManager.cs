using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Manager -> Get / Set
public partial class PlayerManager : MonoBehaviour
{
    public enum State
    {
        IDLE, Walk, Attack, DIE
    }


    [Header("---Player Move---")]
    //NavMeshAgent
    public NavMeshAgent agent;
    //Transform
    public new Transform transform;
    //이동할 점
    public Vector3 Point;
    //속도
    public Vector3 velocity = Vector3.zero;

    Animator animator;
    public State state = State.IDLE;
    public bool isDie = false;


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






    private void Start()
    {
        //Move.cs
        transform = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.updateRotation = false;

        StartCoroutine(CheckPlayerState());
        StartCoroutine(PlayerAction());
    }

    IEnumerator CheckPlayerState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.1f);

            state = agent.remainingDistance < 0.1f ? State.IDLE : State.Walk;




        }
    }

    IEnumerator PlayerAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                case State.IDLE:
                    animator.SetBool("IsWalk", false);

                    break;

                case State.Walk:
                    animator.SetBool("IsWalk", true);
                    animator.SetBool("IsFire", false);

                    break;

                case State.Attack:
                    animator.SetBool("IsFire", true);

                    break;
            }

            yield return new WaitForSeconds(0.3f);
        }

    }

    void Update()
    {
        //Setting.cs
        KeyMapping(); //키 맵핑

    }

    void LateUpdate()
    {
        //Move.cs
        CameraMove(); //카메라 움직임
    }

}
