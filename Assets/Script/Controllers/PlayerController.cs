using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.StatusIndicators.Components;
using UnityEngine.AI;

public class PlayerController : BaseController
{
    //키 입력
    public bool IsKey = false;
    private bool IsRange = false;
    private bool _used = false;  //Announce GetDeck is first or not


    //PlayerInHandCard
    public List<BaseCard> _inHand = new List<BaseCard>();
    //PlayerDeckBase
    public List<string> _baseDeck = new List<string>();
    //PlayerAttackRange
    public List<GameObject> _attackRange = new List<GameObject>();


    //초기화
    private Animator animator { get; set; }
    private NavMeshAgent agent { get; set; }


    public override void OnEnable()
    {
        base.OnEnable();

        _state = Define.State.Idle;
    }


    private void Update()
    {
        //HP < 0 이면 죽음 상태
        if (_pStats.nowHealth <= 0)
        {
            _state = Define.State.Die;
            IsKey = true;
        }

        if (IsKey == false) { idle(); } //키 입력없으면 Idle 상태

        StartCoroutine(PlayerAnim());
    }


    //start에서 Player 세팅 초기화
    public override void Setting()
    {
        //초기화
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();


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



    //플레이어 키 event에 해당하는 Action 
    public override void KeyDownAction(string name)
    {
        switch (name)
        {
            case "rightButton":
                playerMove(Get3DMousePosition());
                Invoke("idle", 0.2f);
                IsKey = true;
                _state = Define.State.Moving;
                //Debug.Log(_state);

                break;

            case "a":
                AttRange_Active();
                break;

            case "leftButton":
                if (IsRange == true)
                {
                    Invoke("idle", 0.4f);
                    IsKey = true;
                    _state = Define.State.Attack;
                    //Debug.Log(_state);
                    AttRange_Active();
                }

                break;

            case "q":
            case "w":
            case "e":
            case "r":
                Invoke("idle", 0.4f);
                IsKey = true;
                _state = Define.State.Skill;

                break;
        }
    }


    //Idle 애니메이션
    private void idle()
    {
        IsKey = false;

        if (agent.remainingDistance < 0.2f)
        {
            _state = Define.State.Idle;
        }
    }


    //Animator 파라미터 설정
    private IEnumerator PlayerAnim()
    {
        switch (_state)
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

                animator.SetBool("IsFire", true);
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsThrow1", false);

                break;

            case Define.State.Skill:
                agent.ResetPath();

                animator.SetBool("IsThrow1", true);
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsFire", false);

                break;

            case Define.State.Card:
                animator.SetTrigger("Throw2");
                animator.SetBool("IsIdle", false);

                break;

            case Define.State.Die:
                animator.SetTrigger("Die");
                animator.SetBool("IsIdle", false);
                inputAction.Disable();
                this.enabled = false;

                StopAllCoroutines();

                break;
        }
        yield return new WaitForSeconds(0.3f);

    }


    //마우스 좌표에 따른 PlayerRotate
    protected Vector3 FlattenVector(Vector3 mousepositon)
    {
        return new Vector3(mousepositon.x, transform.position.y, mousepositon.z);
    }


    //플레이어 이동 
    private void playerMove(Vector3 mouseposition)
    {
        //LookRotation = forward 방향이 vector3(x,0,z)가 가리키는 방향을 바라보도록 회전
        transform.rotation = Quaternion.LookRotation(FlattenVector(mouseposition) - transform.position);
        agent.SetDestination(mouseposition);
    }



    //플레이어 평타 On/off
    private void AttRange_Active()
    {
        if (IsRange == true || IsRange == false)
        {
            IsRange = !IsRange;
            _attackRange[0].SetActive(IsRange);
        }
    }


}
