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

    private UI_Card _cardStats;

    private string _NowState = null;
    private float _distance = default;


    //Range On/off
    private bool _IsRange = false;
    private bool _IsSkill = false;
    private bool _UseNonTargetSkill = false;
    private bool _UseTargetSkill = false;

    //PlayerAttackRange
    public List<GameObject> _attackRange = new List<GameObject>();

    //평타/스킬 쿨타임
    private float _SaveAttackSpeed = default;
    private float _SaveSkillCool = default;
    private int _SaveRangeNum = 5;

    //총알 위치
    private Transform _Proj_Parent;

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
        _Proj_Parent = GameObject.Find("Barrel_Location").transform;


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

            case Define.MouseEvent.Click:
                if (_IsSkill == true)
                {
                    if (_proj == Define.Projectile.Proj_Target_Attack)
                    {
                        MouseClickState(_MousePos.Item1, _MousePos.Item2);
                    }

                    if (_proj == Define.Projectile.Proj_Target_Skill)
                    {
                        MouseClickState(_MousePos.Item1, _MousePos.Item2, true);
                    }

                    if (_proj == Define.Projectile.Proj_NonTarget_Skill)
                    {
                        MouseClickState(_MousePos.Item1);
                    }
                }
                break;
        }
    }


    //마우스 클릭 시 대상 반환
    private void MouseClickState(Vector3 hitPosition = default, GameObject hitObject = null, bool leftMousebutton = false)
    {
        //hitObject가 없을 시
        if (hitObject == null)
        {
            _MovingPos = hitPosition;
            _NowState = "Skip";

            if (_stopSkill == false) { State = Define.State.Skill; }
        }

        //hitObject가 있을 시
        if (hitObject != null && leftMousebutton == false)
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

        //마우스 왼 클릭시
        if (hitObject != null && leftMousebutton == true)
        {
            int hitObjectLayer = hitObject.layer;

            if (hitObjectLayer == _pStats.enemyArea || hitObjectLayer == (int)Define.Layer.Neutral)
            {
                _MovingPos = hitPosition;
                _lockTarget = hitObject;

                if (_stopSkill == false) { State = Define.State.Skill; }
            }
        }
    }


    //Key event
    private void KeyDownAction(Define.KeyboardEvent _key)
    {

        switch (_key)
        {
            case Define.KeyboardEvent.Q:
                KeyPushState("Q");

                break;

            case Define.KeyboardEvent.W:
                KeyPushState("W");

                break;

            case Define.KeyboardEvent.E:
                KeyPushState("E");

                break;

            case Define.KeyboardEvent.R:
                KeyPushState("R");

                break;

            case Define.KeyboardEvent.A:
                KeyPushState("A");

                break;
        }

    }


    //키 누를시 상태 전환
    private void KeyPushState(string Keyname)
    {
        if (!_stopSkill) { _IsSkill = (_IsSkill == true ? false : true); }

        if (Keyname != "A")
        {
            _cardStats = GameObject.Find(Keyname).GetComponentInChildren<UI_Card>();

            switch (_cardStats._rangeType)
            {
                //Scale 고정
                case "Arrow":
                    _SaveRangeNum = 0;
                    _attackRange[0].SetActive(_IsSkill);
                    _proj = Define.Projectile.Proj_NonTarget_Skill;

                    if (_IsSkill == true)
                    {
                        //Arrow는 Scale 값 고정
                        _attackRange[0].GetComponent<AngleMissile>().Scale = 10.0f;
                    }

                    break;

                //Scale, Angle
                case "Cone":
                    _SaveRangeNum = 1;
                    _attackRange[1].SetActive(_IsSkill);
                    _proj = Define.Projectile.Proj_NonTarget_Skill;

                    if (_IsSkill == true)
                    {
                        //스킬 범위 크기
                        _attackRange[1].GetComponent<Cone>().Scale = 2 * _cardStats._rangeScale;
                        //스킬 각도
                        _attackRange[1].GetComponent<Cone>().Angle = _cardStats._rangeAngle;
                    }

                    break;

                //Scale
                case "Line":
                    _SaveRangeNum = 2;
                    _attackRange[2].SetActive(_IsSkill);
                    _proj = Define.Projectile.Proj_NonTarget_Skill;

                    if (_IsSkill == true)
                    {
                        //스킬 범위 크기
                        _attackRange[2].GetComponent<AngleMissile>().Scale = 2 * _cardStats._rangeScale;
                    }

                    break;

                //Scale, Range
                case "Point":
                    _SaveRangeNum = 3;
                    _attackRange[3].SetActive(_IsSkill);
                    _proj = Define.Projectile.Proj_NonTarget_Skill;

                    if (_IsSkill == true)
                    {
                        //스킬 범위 크기
                        _attackRange[3].GetComponent<Point>().Scale = 2 * _cardStats._rangeScale;
                        //스킬 거리
                        _attackRange[3].GetComponent<Point>().Range = _cardStats._rangeRange;
                    }

                    break;

                //Scale
                case "Range":
                    _SaveRangeNum = 4;
                    _attackRange[4].SetActive(_IsSkill);
                    _proj = Define.Projectile.Proj_Target_Skill;

                    if (_IsSkill == true)
                    {
                        //스킬 범위 크기
                        Projector proj = _attackRange[4].GetComponent<Projector>();
                        proj.orthographicSize = _cardStats._rangeScale;
                    }

                    break;
            }
        }

        if (Keyname == "A")
        {
            _SaveRangeNum = 4;
            _attackRange[4].SetActive(_IsSkill);
            _proj = Define.Projectile.Proj_Target_Attack;

            if (_IsSkill == true)
            {
                //스킬 범위 크기
                Projector proj = _attackRange[4].GetComponent<Projector>();
                proj.orthographicSize = _pStats.attackRange;
            }
        }
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
        //Idle
        if (_lockTarget == null && _agent.remainingDistance < 0.2f) { State = Define.State.Idle; }
        //Attack
        if (_NowState == "Attack" && _agent.remainingDistance <= _pStats._attackRange && _stopAttack == false) { State = Define.State.Attack; }
        //Skill
        if (_NowState == "Skill" && _stopSkill == false &&
            (_agent.remainingDistance <= _pStats._attackRange || _agent.remainingDistance <= _cardStats._rangeScale))
        {
            State = Define.State.Skill;
        }
    }

    [PunRPC]
    protected override void UpdateAttack()
    {
        Debug.Log(_pv.IsMine);
        //적이 공격 범위 밖에 있을 때 Moving 전환
        if (Vector3.Distance(this.transform.position, _MovingPos) > _pStats._attackRange)
        {
            //애니메이션 Moving으로 변한
            State = Define.State.Moving;
            _NowState = "Attack";

            return;
        }

        //적이 공격 범위 안에 있을 때
        else
        {
            if (!_stopAttack)
            {
                //공격속도 
                _stopAttack = true;

                _attackRange[4].SetActive(false);

                //Shoot55555
                Managers.Pool.Projectile_Pool("PoliceBullet", _Proj_Parent.position, _lockTarget.transform,
                5.0f, _pStats._basicAttackPower);


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
        _distance = Vector3.Distance(this.transform.position, _MovingPos);
        //적이 공격 범위 밖에 있을 때 Moving 전환
        if (_NowState != "Skip" && (_distance > _pStats._attackRange || _distance > _cardStats._rangeScale))
        {
            //애니메이션 Moving으로 변한
            State = Define.State.Moving;
            _NowState = "Skill";

            return;
        }

        else
        {
            if (!_stopSkill)
            {
                //스킬 쿨타임
                _stopSkill = true;

                //Skill
                GameObject go = new GameObject("Particle");
                go.transform.position = _MovingPos;
                _cardStats.cardEffect(go.transform);
                Destroy(go, 2.0f);
                _IsSkill = false;
                _attackRange[_SaveRangeNum].SetActive(_IsSkill);

                //타겟을 향해 회전 및 멈추기
                transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, _MovingPos) - transform.position);
                _agent.ResetPath();

                //애니메이션 Idle로 변환
                State = Define.State.Idle;

                return;
            }
        }
    }


    protected override void UpdateDie()
    {

    }


    protected override void StopAttack()
    {
        if (_SaveAttackSpeed == default)
        {
            _SaveAttackSpeed = 0.01f;
        }

        if (_SaveAttackSpeed != default)
        {
            _SaveAttackSpeed += Time.deltaTime;

            if (_SaveAttackSpeed >= _pStats._attackDelay)
            {
                Debug.Log("Attack Ready");
                _NowState = null;
                _stopAttack = false;
                _SaveAttackSpeed = default;
            }
        }
    }


    protected override void StopSkill()
    {
        if (_SaveSkillCool == default)
        {
            _SaveSkillCool = 0.01f;
        }

        if (_SaveSkillCool != default)
        {
            _SaveSkillCool += Time.deltaTime;

            if (_SaveSkillCool >= _pStats._cardCoolTime)
            {
                Debug.Log("Skill Ready");
                _NowState = null;
                _stopSkill = false;
                _SaveSkillCool = default;
            }
        }
    }

}
