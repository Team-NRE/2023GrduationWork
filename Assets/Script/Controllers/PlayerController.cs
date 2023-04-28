using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.StatusIndicators.Components;
using UnityEngine.AI;


public class PlayerController : BaseController
{
    //키 입력
    private bool IsRange = false;
    //private bool _used = false;  //Announce GetDeck is first or not

    //PlayerInHandCard
    public List<BaseCard> _inHand = new List<BaseCard>();
    //PlayerDeckBase
    public List<string> _baseDeck = new List<string>();
    //PlayerAttackRange
    public List<GameObject> _attackRange = new List<GameObject>();


    //초기화
    private Animator animator { get; set; }
    private NavMeshAgent agent { get; set; }
    private Transform Proj_Parent;

    public override void OnEnable()
    {
        base.OnEnable();
    }

    //start에서 Player 세팅 초기화
    public override void Setting()
    {
        //초기화
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        //Object Pool
        Projectile_Pool("PoliceBullet", "Barrel_Location");

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
    public override void KeyDownAction(Define.KeyboardEvent _key)
    {
        switch (_key)
        {
            case Define.KeyboardEvent.A:
                AttRange_Active();

                break;

            case Define.KeyboardEvent.Q:
                _state = Define.State.Skill;

                break;

            case Define.KeyboardEvent.W:
                _state = Define.State.Skill;

                break;

            case Define.KeyboardEvent.E:
                _state = Define.State.Skill;

                break;

            case Define.KeyboardEvent.R:
                _state = Define.State.Skill;

                break;
        }
    }


    //플레이어 마우스 event에 해당하는 Action
    public override void MouseDownAction(string _button)
    {
        switch (_button)
        {
            case "rightButton":
                playerMove(Get3DMousePosition(Define.Layer.Road, Define.Layer.Cyborg));
                _state = Define.State.Moving;

                break;

            case "leftButton":
                if (IsRange == true && AttTarget_Set() != null)
                {
                    Shoot();
                    _state = Define.State.Attack;
                }

                break;
        }
    }

    private void Update()
    {
        StartCoroutine(Player_Update_State());
        StartCoroutine(PlayerAnim());
    }


    //Idle 애니메이션 -> Invoke -> 잠시 다른 키 애니메이션 시간 벌어주기
    private void idle()
    {
        //_keyboard = Define.KeyboardEvent.NoInput;

        if (agent.remainingDistance < 0.2f)
        {
            _state = Define.State.Idle;
        }
    }


    //플레이어가 Update에서 일어나는 State 변화들
    private IEnumerator Player_Update_State()
    {
        //적을 판별해 적에게 움직이다 적이 사정거리 안에 들어오면 Shoot 
        if (agent.remainingDistance <= _pStats._attackRange && _layer == Define.Layer.Cyborg)
        {
            Shoot();
            _state = Define.State.Attack;
        }

        //
        if (_pStats.nowHealth > 0 && _state == Define.State.Die) { _state = Define.State.Idle; }

        //HP < 0 이면 죽음 상태
        if (_pStats.nowHealth <= 0) { _state = Define.State.Die; }

        yield return new WaitForSeconds(0.3f);
    }


    //Animator 파라미터 설정
    private IEnumerator PlayerAnim()
    {
        switch (_state)
        {
            case Define.State.Idle:
                animator.SetBool("IsIdle", true);
                animator.SetBool("IsWalk", false);
                animator.SetBool("IsThrow1", false);
                animator.SetBool("IsFire", false);

                break;

            case Define.State.Moving:
                Invoke("idle", 0.2f);

                animator.SetBool("IsWalk", true);
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsThrow1", false);
                animator.SetBool("IsFire", false);

                break;

            case Define.State.Attack:
                Invoke("idle", 0.2f);

                agent.ResetPath();

                animator.SetBool("IsFire", true);
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsThrow1", false);

                break;

            case Define.State.Skill:
                Invoke("idle", 0.2f);

                agent.ResetPath();

                animator.SetBool("IsThrow1", true);
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsFire", false);

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



    //플레이어 이동 
    private void playerMove(Vector3 mouseposition)
    {
        //mouseposition은 Road, Cyborg만 탐지.
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


    //AttackRange를 활용한 어택 타겟 설정
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
            float distance = Vector3.Distance(Get3DMousePosition(Define.Layer.Road, Define.Layer.Cyborg), coll.transform.position);

            //가장 가까운 거리 설정 및 타겟 설정
            if (CloseDistance > distance)
            {
                CloseDistance = distance;
                target = coll.gameObject;
            }
        }

        if (target != null)
        {
            //Player 회전
            transform.rotation = Quaternion.LookRotation(FlattenVector(target.transform.position) - transform.position);
        }

        return target;
    }


    //muzzle / 총알 나가야 됨.
    private void Shoot()
    {
        Managers.Pool.Pop(Projectile_Pool("PoliceBullet").Item1, 
            Projectile_Pool("PoliceBullet").Item2).GetComponent<Poolable>().Proj_Target_Init(AttTarget_Set());

        //마우스 오른쪽 클릭 시 공격 후 레이어 초기화
        if (_layer == Define.Layer.Cyborg) { _layer = Define.Layer.Default; }

        //사거리 off
        if (IsRange == true) { AttRange_Active(); }
    }
}
