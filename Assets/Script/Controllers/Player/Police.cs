using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.StatusIndicators.Components;
using UnityEngine.AI;
using Stat;
using Photon.Pun;


public class Police : BaseController
{
    //플레이어 스텟 초기화
    private PlayerStats _pStats;

    //Range On/off
    private bool IsRange = false;

    //PlayerAttackRange
    public List<GameObject> _attackRange = new List<GameObject>();

    //평타 쿨타임 (공격 속도)
    private float _SaveAttackSpeed = default;

    //총알 위치
    public Transform Proj_Parent;

    public void OnEnable()
    {
        GetComponent<CapsuleCollider>().enabled = true;
    }

    //start 초기화
    public override void Init()
    {
        _pv = GetComponent<PhotonView>();
        //초기화
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _pStats = GetComponent<PlayerStats>();

        //액션 대리자 호출
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.MouseAction += MouseDownAction;
        Managers.Input.KeyAction -= KeyDownAction;
        Managers.Input.KeyAction += KeyDownAction;


        //총알 위치
        //Proj_Parent = GameObject.Find("Barrel_Location").transform;


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
    private void MouseDownAction(Define.MouseEvent evt)
    {
        (Vector3, GameObject) _MousePos = Managers.Input.Get3DMousePosition((1 << 0 | 1 << 2));
        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                MouseClickState(_MousePos.Item1, _MousePos.Item2);

                break;

            case Define.MouseEvent.Press:
                MouseClickState(_MousePos.Item1, _MousePos.Item2);

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

                if (_stopAttack == false) { State = Define.State.Attack; }
            }
        }
    }


    //Key event
    private void KeyDownAction(Define.KeyboardEvent _key)
    {

        switch (_key)
        {
            case Define.KeyboardEvent.Q:

                KeyPushState();

                break;

            case Define.KeyboardEvent.W:
                KeyPushState();

                break;

            case Define.KeyboardEvent.E:
                KeyPushState();

                break;

            case Define.KeyboardEvent.R:
                KeyPushState();

                break;

            case Define.KeyboardEvent.A:


                break;

        }

    }


    private void KeyPushState()
    {
        //사거리 표시 

    }


    protected override void UpdateIdle()
    {
        if (_agent.remainingDistance < 0.2f)
        {
            State = Define.State.Idle;
        }
    }


    protected override void UpdateMoving()
    {
        if (_pv.IsMine)
        {
            transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, _MovingPos) - transform.position);
            _agent.SetDestination(_MovingPos);

            // 조건 만족 시 상태 변환
            if (_lockTarget == null && _agent.remainingDistance < 0.2f) { State = Define.State.Idle; }
            if (_lockTarget != null && _agent.remainingDistance <= _pStats._attackRange && _stopAttack == false) { State = Define.State.Attack; }
        }
        else
        {
            Debug.Log("else moving");

            // 수신된 좌표로 보간한 이동처리
            transform.position = Vector3.Lerp(transform.position,
                                              receivePos,
                                              Time.deltaTime * damping);

            // 수신된 회전값으로 보간한 회전처리
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  receiveRot,
                                                  Time.deltaTime * damping);

        }
    }


    protected override void UpdateAttack()
    {
        //적이 공격 범위 밖에 있을 때 Moving 전환
        if (Vector3.Distance(this.transform.position, _MovingPos) > _pStats._attackRange)
        {
            //애니메이션 Moving으로 변한
            State = Define.State.Moving;

            return;
        }

        //적이 공격 범위 안에 있을 때
        else
        {
            if (!_stopAttack)
            {
                //공격속도 
                _stopAttack = true;

                //Shoot
                //Managers.Pool.Projectile_Pool("PoliceBullet", Proj_Parent.position, _lockTarget.transform,
                //5.0f, _pStats._basicAttackPower);

                //타겟을 향해 회전 및 멈추기
                transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, _MovingPos) - transform.position);
                _agent.ResetPath();

                //애니메이션 Idle로 변환
                State = Define.State.Idle;

                return;
            }
        }
    }


    protected override void UpdateSkill()
    {
        Debug.Log("UpdateSkill");
    }

    protected override void UpdateDie()
    {

    }

    protected override void StopAttack()
    {
        if (_SaveAttackSpeed == default) { _SaveAttackSpeed = 0.01f; }
        if (_SaveAttackSpeed != default)
        {
            _SaveAttackSpeed += Time.deltaTime;

            if (_SaveAttackSpeed >= _pStats._attackDelay)
            {
                _stopAttack = false;
                _SaveAttackSpeed = default;
            }
        }
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
            5.0f, _pStats._basicAttackPower);

        //Attack Range off
        if (IsRange == true) { AttRange_Active(); }
    }
}