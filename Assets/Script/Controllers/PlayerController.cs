using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.StatusIndicators.Components;
using UnityEngine.AI;


public class PlayerController : BaseController
{
    //Range On/off
    private bool IsRange = false;
    private bool IsEnemy = false;
    //private bool _used = false;  //Announce GetDeck is first or not

    //PlayerInHandCard
    public List<UI_Card> _inHand = new List<UI_Card>();
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
        GetComponent<CapsuleCollider>().enabled = true;
    }

    //start 초기화
    public override void Setting()
    {
        //초기화
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        //총알 위치
        Proj_Parent = GameObject.Find("Barrel_Location").transform;


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

        //AttackRange 초기화
        Projector projector = _attackRange[0].GetComponent<Projector>();
        projector.orthographicSize = _pStats._attackRange;

        //Prefab off
        GetComponentInChildren<SplatManager>().enabled = true;

        //Object Target 정하는 리스트
        ObjectController._allObjectTransforms.Add(transform);

    }


    //Key event
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


    //Mouse event
    public override void MouseDownAction(string _button)
    {
        switch (_button)
        {
            case "rightButton":
                playerMove(Get3DMousePosition());
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


    //Invoke 통한 Idle 외 다른 Animation 작동 시간 벌어주기
    private void idle()
    {
        if (agent.remainingDistance < 0.2f)
        {
            _state = Define.State.Idle;
        }
    }


    //Update에서 발생하는 애니메이션 변환
    private IEnumerator Player_Update_State()
    {
        //오른쪽 클릭 공격
        if (agent.remainingDistance <= _pStats._attackRange && (int)_layer == _pStats.enemyArea)
        {
            Shoot();
            _state = Define.State.Attack;
        }

        //부활 시
        if (_pStats.nowHealth > 0 && _state == Define.State.Die) { _state = Define.State.Idle; }

        //사망 시
        if (_pStats.nowHealth <= 0) { _state = Define.State.Die; }

        yield return new WaitForSeconds(0.3f);
    }


    //Animator coroutine
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
                GetComponent<CapsuleCollider>().enabled = false;
                this.enabled = false;

                StopAllCoroutines();

                break;
        }

        yield return new WaitForSeconds(0.3f);
    }


    //Ray 통한 Move
    private void playerMove(Vector3 mouseposition)
    {
        transform.rotation = Quaternion.LookRotation(FlattenVector(mouseposition) - transform.position);
        agent.SetDestination(mouseposition);
    }


    //AttackRange On/Off
    private void AttRange_Active()
    {
        if (IsRange == true || IsRange == false)
        {
            IsRange = !IsRange;
            _attackRange[0].SetActive(IsRange);
        }
    }


    //AttackRange On -> Target 설정
    private GameObject AttTarget_Set()
    {
        //Target 초기화
        GameObject target = null;
        //가장 가까운 거리의 적 초기화
        float CloseDistance = Mathf.Infinity;

        //적 탐지
        Collider[] colls = Physics.OverlapSphere(transform.position, _pStats._attackRange, 1 << _pStats.enemyArea);

        //타겟 설정
        foreach (Collider coll in colls)
        {
            float distance = Vector3.Distance(Get3DMousePosition(), coll.transform.position);

            if (CloseDistance > distance)
            {
                CloseDistance = distance;
                target = coll.gameObject;
            }
        }

        if (target != null)
        {
            //Player rotate
            transform.rotation = Quaternion.LookRotation(FlattenVector(target.transform.position) - transform.position);
        }

        return target;
    }


    //bullet objectpooling pop
    private void Shoot()
    {
        Managers.Pool.Projectile_Pool("PoliceBullet", Proj_Parent.position, AttTarget_Set().transform,
            _pStats._attackSpeed, _pStats._basicAttackPower);

        if ((int)_layer == _pStats.enemyArea) { _layer = Define.Layer.Default; }

        //Attack Range off
        if (IsRange == true) { AttRange_Active(); }
    }
}
