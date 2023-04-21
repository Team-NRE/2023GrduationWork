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

    public GameObject muzzle;
    public Transform barrelLocation;

    public float dis;

    public override void OnEnable()
    {
        base.OnEnable();

        _state = Define.State.Idle;
    }


    private void Update()
    {
        if (agent.remainingDistance <= _pStats.attackRange)
        {

        }

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


        //공격사거리 세팅
        Projector projector = _attackRange[0].GetComponent<Projector>();
        projector.orthographicSize = _pStats._attackRange;

        //Prefab 다시 off
        GetComponentInChildren<SplatManager>().enabled = true;
    }



    //플레이어 키 event에 해당하는 Action 
    public override void KeyDownAction(string name)
    {
        switch (name)
        {
            case "rightButton":
                player_Move(Get3DMousePosition(Define.Layer.Road, Define.Layer.Cyborg));
                Invoke("idle", 0.2f);
                IsKey = true;
                _state = Define.State.Moving;

                break;

            case "a":
                AttRange_Active();

                break;

            case "leftButton":
                if (IsRange == true && AttTarget_Set() != null)
                {
                    Shoot(AttTarget_Set());
                    Invoke("idle", 0.4f);
                    IsKey = true;
                    _state = Define.State.Attack;
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
    public void player_Move(Vector3 mouseposition)
    {
        //mouseposition은 Road, Cyborg만 탐지.
        if (_layer == Define.Layer.Road)
        {
            //LookRotation = forward 방향이 vector3(x,0,z)가 가리키는 방향을 바라보도록 회전
            transform.rotation = Quaternion.LookRotation(FlattenVector(mouseposition) - transform.position);
            agent.SetDestination(mouseposition);
        }

        if(_layer == Define.Layer.Cyborg)
        {
            Debug.Log(_layer);
            Debug.Log("Attack");
        }


        //마우스 위치에 적이 있으면 판별해줘야함 -> 적에 대한 정보 필요함 -> 어캐 구별하지? layer? 
        //(1) 마우스 좌표 - 적 position 이 거의 차이 안나면 목표 설정
        //(2) 목표를 향해 setdestination 중 (남은 거리 <= 사거리) -> _state = Define.state.Attack
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


    //어택 타겟 설정
    private GameObject AttTarget_Set()
    {
        //타겟 초기화
        GameObject target = null;
        //가장 가까운 거리 초기화
        float CloseDistance = Mathf.Infinity;

        //플레이어 근처 적 식별
        Collider[] colls = Physics.OverlapSphere(transform.position, _pStats._attackRange, 1 << ((int)Define.Layer.Cyborg));

        //식별된 적 모두 중 하나만 선별
        foreach (Collider coll in colls)
        {
            //마우스 포인터, 식별된 적 사이의 거리 구하기
            float distance = Vector3.Distance(Get3DMousePosition(Define.Layer.Road), coll.transform.position);

            //가장 가까운 거리 설정 및 타겟 설정
            if (CloseDistance > distance)
            {
                CloseDistance = distance;
                target = coll.gameObject;
            }
        }

        return target;
    }


    //타겟을 향해 Rotate / muzzle / 총알 나가야 됨.
    public void Shoot(GameObject target)
    {
        //Player 회전
        transform.rotation = Quaternion.LookRotation(FlattenVector(target.transform.position) - transform.position);

        //총알 발사 이미지
        if (muzzle)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzle, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, 0.7f);
        }

        //총알 projectile


        //사거리 off
        AttRange_Active();
    }
}
