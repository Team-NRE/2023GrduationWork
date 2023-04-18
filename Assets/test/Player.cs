using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;
using Werewolf.StatusIndicators.Components;


public class Player : BaseTest
{
    //초기화
    NavMeshAgent agent;
    Animator animator;
    new Transform transform;
    PlayerStats p_Stat;
    public Define.State status;
    BaseTest besttest { get; set; }

    //이동할 점 / layer
    public Vector3 Point;
    public LayerMask Ignorelayer;

    //키 입력
    public bool IsKey = false;
    bool IsRange = false;
    bool _used = false;  //Announce GetDeck is first or not

    //PlayerInHandCard
    List<BaseTest> _inHand = new List<BaseTest>();
    //PlayerDeckBase
    List<string> _baseDeck = new List<string>();

    //PlayerAttackRange
    public List<GameObject> _attackRange = new List<GameObject>();

    private void Update()
    {

        //HP < 0 이면 죽음 상태
        if (p_Stat.nowHealth <= 0) { status = Define.State.Die; }
        if (IsKey == false) { idle(); } //키 입력없으면 Idle 상태

        StartCoroutine(PlayerAnim());
    }

    public override void BaseSetting()
    {
        //자식객체 player의 컴포넌트 초기화
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        transform = GetComponent<Transform>();
        p_Stat = GetComponent<PlayerStats>();


        //Range List Setting 
        GetComponentInChildren<SplatManager>().enabled = false;
        GameObject[] loadAttackRange = Resources.LoadAll<GameObject>("Prefabs/AttackRange");
        foreach (GameObject attackRange in loadAttackRange)
        {
            GameObject Prefab = Instantiate(attackRange);
            Prefab.transform.parent = transform.GetChild(0);
            Prefab.transform.localPosition = Vector3.zero;
            Prefab.SetActive(true);

            _attackRange.Add(Prefab);
        }
        GetComponentInChildren<SplatManager>().enabled = true;

    }


    //플레이어 상태 체크
    public override void KeyDownAction(string name)
    {
        switch (name)
        {
            case "rightButton":
                playerMove(Get3DMousePosition());
                Invoke("idle", 0.2f);
                IsKey = true;
                status = Define.State.Moving;

                break;

            case "a":
                AttRange_Active();
                break;

            case "leftButton":
                if (IsRange == true)
                {
                    
                    Invoke("idle", 0.4f);
                    IsKey = true;
                    status = Define.State.Attack;
                    AttRange_Active();
                }

                break;

            case "q":
            case "w":
            case "e":
            case "r":
                
                Invoke("idle", 0.4f);
                IsKey = true;
                status = Define.State.Skill;

                break;
        }
    }



    void idle()
    {
        IsKey = false;

        if (agent.remainingDistance < 0.2f)
        {
            status = Define.State.Idle;
        }
    }



    public IEnumerator PlayerAnim()
    {
        yield return new WaitForSeconds(0.3f);

        switch (status)
        {
            case Define.State.Idle:
                agent.isStopped = false;

                animator.SetBool("IsIdle", true);
                animator.SetBool("IsWalk", false);
                animator.SetBool("IsThrow1", false);
                animator.SetBool("IsFire", false);

                break;

            case Define.State.Moving:
                agent.isStopped = false;

                animator.SetBool("IsWalk", true);
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsThrow1", false);
                animator.SetBool("IsFire", false);

                break;

            case Define.State.Attack:
                agent.ResetPath();
                //agent.isStopped = true;

                animator.SetBool("IsFire", true);
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsThrow1", false);

                break;

            case Define.State.Skill:
                agent.ResetPath();
                //agent.isStopped = true;

                animator.SetBool("IsThrow1", true);
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsFire", false);

                break;

            case Define.State.Card:
                animator.SetTrigger("Throw2");
                animator.SetBool("IsIdle", false);

                break;

            case Define.State.Die:
                animator.SetBool("IsIdle", false);
                animator.SetTrigger("Die");
                this.enabled = false;

                StopAllCoroutines();

                break;
        }

    }



    protected Vector3 FlattenVector(Vector3 mousepositon)
    {
        return new Vector3(mousepositon.x, transform.position.y, mousepositon.z);
    }



    //플레이어 이동 
    public void playerMove(Vector3 mouseposition)
    {
        //LookRotation = forward 방향이 vector3(x,0,z)가 가리키는 방향을 바라보도록 회전
        transform.rotation = Quaternion.LookRotation(FlattenVector(mouseposition) - transform.position);
        agent.SetDestination(mouseposition);
    }



    //플레이어 평타
    public void AttRange_Active()
    {
        if (IsRange == true || IsRange == false)
        {
            IsRange = !IsRange;
            _attackRange[0].SetActive(IsRange);
        }
    }
}
