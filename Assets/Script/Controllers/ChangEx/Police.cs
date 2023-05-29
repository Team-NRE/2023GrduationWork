using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.StatusIndicators.Components;
using UnityEngine.AI;
using Stat;


public class Police : BaseController
{
    //플레이어 스텟 초기화
    private PlayerStats _pStats;

    //Range On/off
    private bool IsRange = false;
    //총알 발사 여부
    private bool IsFired = false;

    //PlayerAttackRange
    public List<GameObject> _attackRange = new List<GameObject>();


    //총알 위치
    private Transform Proj_Parent;


    public void OnEnable()
    {
        GetComponent<CapsuleCollider>().enabled = true;
    }

    //start 초기화
    public override void Init()
    {
        //초기화
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _pStats = GetComponent<PlayerStats>();

        //액션 대리자 호출
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.MouseAction += MouseDownAction;

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


    //Mouse event
    void MouseDownAction(Define.MouseEvent evt)
    {
        /*
        switch (State)
        {
            case Define.State.Attack:
                if (IsRange == true && AttTarget_Set() != null)
                {
                    Shoot();
                    _state = Define.State.Attack;
                }

                break;
        }*/
        (Vector3, GameObject) _MP = Managers.Input.Get3DMousePosition((1 << 0 | 1 << 2));

        switch (evt)
        {
            case Define.MouseEvent.Press:
                MouseClickState(_MP.Item1, _MP.Item2);
                break;

            case Define.MouseEvent.Click:
                MouseClickState(_MP.Item1, _MP.Item2);

                break;

            case Define.MouseEvent.PointerDown:

                break;
        }
    }

    //마우스 클릭 시 대상 반환
    private void MouseClickState(Vector3 hitPosition = default, GameObject hitObject = null)
    {
        //hitObject가 없을 시
        if (hitObject == null) return;

        //hitObject가 있을 시
        if (hitObject != null)
        {
            int hitObjectLayer = hitObject.layer;

            //도로를 클릭 시
            if (hitObjectLayer == (int)Define.Layer.Road)
            {
                _MovingPos = hitPosition;
                _lockTarget = null;

                State = Define.State.Moving;
            }

            //적 or 중앙 obj 클릭 시
            //_pStats.enemyArea가 상수반환이 안되서 if문으로 대체
            if (hitObjectLayer == _pStats.enemyArea || hitObjectLayer == (int)Define.Layer.Neutral)
            {
                _MovingPos = hitPosition;
                _lockTarget = hitObject;

                State = Define.State.Moving;
            }
        }
    }

    protected override void UpdateIdle()
    {
        if (_agent.remainingDistance < 0.2f) { State = Define.State.Idle; }
    }


    protected override void UpdateMoving()
    {
        transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, _MovingPos) - transform.position);
        _agent.SetDestination(_MovingPos);

        //코루틴 우선순위 1. moving 2. 아래 조건
        if (_lockTarget == null && _agent.remainingDistance < 0.2f) { State = Define.State.Idle; }
        if (_lockTarget != null && _agent.remainingDistance <= _pStats._attackRange) { State = Define.State.Attack; }
    }


    protected override void UpdateAttack()
    {
        if (_agent.remainingDistance <= _pStats._attackRange)
        {
            //애니메이션 Attack으로 변환
            State = Define.State.Attack;

            //Shoot
            Managers.Pool.Projectile_Pool("PoliceBullet", Proj_Parent.position, _lockTarget.transform,
            _pStats._attackSpeed, _pStats._basicAttackPower);

            //Idle로 전환
            _agent.ResetPath();

            //애니메이션 Idle로 변환
            State = Define.State.Idle;
        }
    }

    


    //Update에서 발생하는 애니메이션 변환
    private IEnumerator Player_Update_State()
    {
        //부활 시
        if (_pStats.nowHealth > 0 && _state == Define.State.Die) { _state = Define.State.Idle; }


        yield return new WaitForSeconds(0.3f);
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
            float distance = Vector3.Distance(Managers.Input.Get3DMousePosition().Item1, coll.transform.position);

            if (CloseDistance > distance)
            {
                CloseDistance = distance;
                target = coll.gameObject;
            }
        }

        if (target != null)
        {
            //Player rotate
            transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, target.transform.position) - transform.position);
        }

        return target;
    }


    //bullet objectpooling pop
    private void Shoot()
    {
        Managers.Pool.Projectile_Pool("PoliceBullet", Proj_Parent.position, AttTarget_Set().transform,
            _pStats._attackSpeed, _pStats._basicAttackPower);

        //Attack Range off
        if (IsRange == true) { AttRange_Active(); }
    }


    /*
    //Key event
    public void KeyDownAction(Define.KeyboardEvent _key)
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
    */

}
