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

    //총알 위치
    private Transform _Proj_Parent;

    //UI_Card 접근
    private UI_Card _cardStats;

    //PlayerAttackRange
    public List<GameObject> _attackRange = new List<GameObject>();

    //현재 상태
    private string _NowState = null;
    private string _NowKey = null;

    //Range On/off
    public string _PushKeyInfo = null;
    public bool _IsRange = false;

    //타겟 유무
    private bool _IsTarget = false;

    //범위 넘버 저장
    private int _SaveRangeNum;


    //평타/스킬 쿨타임
    private float _SaveAttackSpeed = default;
    private float _SaveSkillCool = default;


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
        (Vector3, GameObject) _mousePos = Managers.Input.Get3DMousePosition((1 << 0 | 1 << 2));

        switch (evt)
        {
            //마우스 오른쪽 버튼 클릭 시
            case Define.MouseEvent.PointerDown:
                //공격 타입
                _proj = Define.Projectile.Attack_Proj;

                //타겟이 있을 때 마우스 좌표 오브젝트의 Layer 판별
                MouseClickState(evt, _mousePos.Item1, _mousePos.Item2);

                //사거리 Off
                if (_IsRange == true)
                {
                    KeyPushState("MouseRightButton");
                }

                break;

            //마우스 오른쪽 버튼 누르고 있을 시
            case Define.MouseEvent.Press:
                //공격 타입
                _proj = Define.Projectile.Attack_Proj;

                //타겟이 있을 때 마우스 좌표 오브젝트의 Layer 판별
                MouseClickState(evt, _mousePos.Item1, _mousePos.Item2);

                //사거리 Off
                if (_IsRange == true)
                {
                    KeyPushState("MouseRightButton");
                }

                break;

            //마우스 왼쪽 버튼 클릭 시
            case Define.MouseEvent.LeftButton:
                //Range가 On일 때만 좌클릭 시
                if (_IsRange == true)
                {
                    switch (_IsTarget)
                    {
                        //타겟이 있을 때 (Range)
                        case true:
                            //타겟이 있을 때 마우스 좌표 오브젝트의 Layer 판별
                            MouseClickState(evt, _mousePos.Item1, _mousePos.Item2);

                            break;

                        //타겟이 없을 때 (Arrow, Cone, Line, Point)
                        case false:
                            //타겟 좌표 설정
                            _MovingPos = _mousePos.Item1;

                            //스킬 상태로 전환
                            State = Define.State.Skill;

                            //논타겟 스킬이여서 움직임 없음.
                            _NowState = "SkipMove";

                            break;
                    }
                }

                //Range Off일 때 아무일도 없음.
                else
                {
                    Debug.Log("Range Off 입니다.");
                    return;
                }

                break;
        }
    }


    //마우스 좌표 대상에 따른 State 변환
    private void MouseClickState(Define.MouseEvent evt, Vector3 mousePos = default, GameObject lockTarget = null)
    {
        //대상이 도로일 때 && 마우스 오른쪽 버튼 클릭 시
        if (lockTarget.layer == (int)Define.Layer.Road)
        {
            switch (evt)
            {
                case Define.MouseEvent.PointerDown:
                    //좌표 설정
                    _MovingPos = mousePos;

                    //State Moving 변환
                    State = Define.State.Moving;

                    break;

                case Define.MouseEvent.Press:
                    //좌표 설정
                    _MovingPos = mousePos;

                    //State Moving 변환
                    State = Define.State.Moving;

                    break;

                case Define.MouseEvent.LeftButton:


                    break;
            }

        }

        //적 or 중앙 obj 클릭 시
        //_pStats.enemyArea가 상수반환이 안되서 if문으로 대체
        if (lockTarget.layer == _pStats.enemyArea || lockTarget.layer == (int)Define.Layer.Neutral)
        {
            //좌표 설정
            _MovingPos = mousePos;
            //타겟 오브젝트 설정
            _lockTarget = lockTarget;

            //Attack or Skill
            switch (_proj)
            {
                //Attack
                case Define.Projectile.Attack_Proj:
                    State = Define.State.Attack;

                    break;

                //Skill
                case Define.Projectile.Skill_Proj:
                    State = Define.State.Skill;

                    break;
            }
        }
    }


    //Key event
    private void KeyDownAction(Define.KeyboardEvent _key)
    {
        //키보드 입력 시
        switch (_key)
        {
            case Define.KeyboardEvent.Q:
                _PushKeyInfo = "Q";
                KeyPushState(_PushKeyInfo);

                break;

            case Define.KeyboardEvent.W:
                _PushKeyInfo = "W";
                KeyPushState(_PushKeyInfo);

                break;

            case Define.KeyboardEvent.E:
                _PushKeyInfo = "E";
                KeyPushState(_PushKeyInfo);

                break;

            case Define.KeyboardEvent.R:
                _PushKeyInfo = "R";
                KeyPushState(_PushKeyInfo);

                break;

            case Define.KeyboardEvent.A:
                KeyPushState("A");

                break;
        }

    }


    //키 누를시 상태 전환
    private void KeyPushState(string Keyname)
    {
        //스킬 사거리 표시 On/Off 관리
        //이전 키와 같은 키를 눌렀을 경우 
        if (Keyname != "MouseRightButton" && Keyname == _NowKey)
        {
            //사거리 On/Off 관리
            _IsRange = (_IsRange == true ? false : true);
        }
        //이전 키와 다른 키를 눌렀을 때
        if (Keyname != _NowKey)
        {
            //사거리 On/Off 관리
            switch (_IsRange)
            {
                //이전 키 사거리가 켜져있을 경우
                case true:
                    //이전 키 사거리를 Off
                    _attackRange[_SaveRangeNum].SetActive(false);
                    if (Keyname == "MouseRightButton") { _IsRange = false; }

                    break;

                //이전 키 사거리가 꺼져있을 경우
                case false:
                    //현재 키 사거리 On
                    _IsRange = true;
                    break;
            }
        }

        //마우스 우클릭이 아니면 사거리 접근
        if (Keyname != "MouseRightButton")
        {
            //현재 키 != A
            if (Keyname != "A")
            {
                //해당 키 밑의 스크립트 찾아주기
                _cardStats = GameObject.Find(Keyname).GetComponentInChildren<UI_Card>();

                //카드의 Range 타입에 따른 세팅 변환
                switch (_cardStats._rangeType)
                {
                    //활 모양 Range 
                    case "Arrow":
                        //번호 저장
                        _SaveRangeNum = 0;
                        //사거리 On/Off
                        _attackRange[0].SetActive(_IsRange);

                        //사거리가 On일 때
                        if (_IsRange == true)
                        {
                            _NowKey = Keyname;

                            //스킬 타입
                            _proj = Define.Projectile.Skill_Proj;

                            //논타겟
                            _IsTarget = false;

                            //Arrow는 Scale 값 고정
                            _attackRange[0].GetComponent<AngleMissile>().Scale = 10.0f;
                        }

                        break;

                    //콘 모양 Range
                    case "Cone":
                        //번호 저장
                        _SaveRangeNum = 1;
                        //사거리 On/ Off
                        _attackRange[1].SetActive(_IsRange);

                        //사거리가 On일 때
                        if (_IsRange == true)
                        {
                            _NowKey = Keyname;

                            //스킬 타입
                            _proj = Define.Projectile.Skill_Proj;

                            //논타겟
                            _IsTarget = false;

                            //스킬 범위 크기
                            _attackRange[1].GetComponent<Cone>().Scale = 2 * _cardStats._rangeScale;
                            //스킬 각도
                            _attackRange[1].GetComponent<Cone>().Angle = _cardStats._rangeAngle;
                        }

                        break;

                    //선 모양 Range
                    case "Line":
                        //번호 저장
                        _SaveRangeNum = 2;

                        //사거리 On/Off
                        _attackRange[2].SetActive(_IsRange);

                        //사거리 On 일 때
                        if (_IsRange == true)
                        {
                            _NowKey = Keyname;

                            //스킬 타입
                            _proj = Define.Projectile.Skill_Proj;

                            //논타겟
                            _IsTarget = false;

                            //스킬 범위 크기
                            _attackRange[2].GetComponent<AngleMissile>().Scale = 2 * _cardStats._rangeScale;
                        }

                        break;

                    //포인트 모양 Range
                    case "Point":
                        //번호 저장
                        _SaveRangeNum = 3;

                        //사거리 On/Off
                        _attackRange[3].SetActive(_IsRange);

                        //사거리 On일 때
                        if (_IsRange == true)
                        {
                            _NowKey = Keyname;

                            //스킬 타입
                            _proj = Define.Projectile.Skill_Proj;

                            //논타겟
                            _IsTarget = false;

                            //스킬 범위 크기
                            _attackRange[3].GetComponent<Point>().Scale = 2 * _cardStats._rangeScale;
                            //스킬 거리
                            _attackRange[3].GetComponent<Point>().Range = _cardStats._rangeRange;
                        }

                        break;

                    //원 모양 Range
                    case "Range":
                        //번호 저장
                        _SaveRangeNum = 4;

                        //사거리 On/Off
                        _attackRange[4].SetActive(_IsRange);

                        //사거리 On일 때
                        if (_IsRange == true)
                        {
                            _NowKey = Keyname;

                            //타겟
                            _IsTarget = true;

                            //스킬 범위 크기
                            Projector projector = _attackRange[4].GetComponent<Projector>();

                            //스킬 타입
                            _proj = Define.Projectile.Skill_Proj;
                            projector.orthographicSize = _cardStats._rangeScale;
                        }

                        break;

                    //즉시 사용
                    case "None":
                        //스킬 상태로 전환
                        State = Define.State.Skill;

                        //자기 자신
                        _MovingPos = this.transform.position;

                        //논타겟 스킬이여서 움직임 없음.
                        _NowState = "SkipMove";

                        break;
                }
            }

            //현재 키 == A
            if (Keyname == "A")
            {
                //번호 저장
                _SaveRangeNum = 4;

                //사거리 On/Off
                _attackRange[4].SetActive(_IsRange);

                //사거리 On일 때
                if (_IsRange == true)
                {
                    _NowKey = Keyname;

                    //타겟
                    _IsTarget = true;

                    //스킬 범위 크기
                    Projector projector = _attackRange[4].GetComponent<Projector>();

                    //평타 타입
                    _proj = Define.Projectile.Attack_Proj;
                    projector.orthographicSize = _pStats.attackRange;
                }
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
        transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, _MovingPos) - transform.position);
        _agent.SetDestination(_MovingPos);

        // 조건 만족 시 상태 변환
        //Idle
        if (_agent.remainingDistance < 0.2f) { State = Define.State.Idle; }
        //Attack
        if (_NowState == "Attack" && _agent.remainingDistance <= _pStats._attackRange)
        {
            State = Define.State.Attack;
        }
        //Skill
        if (_NowState == "Skill" && _agent.remainingDistance <= _cardStats._rangeScale)
        {
            State = Define.State.Skill;
        }
    }


    protected override void UpdateAttack()
    {
        //적이 공격 범위 밖에 있을 때 Moving 전환
        if (Vector3.Distance(this.transform.position, _MovingPos) > _pStats._attackRange)
        {
            //애니메이션 Moving으로 변한
            State = Define.State.Moving;
            //현재 상태는 Attack -> Moving에서 Attack으로 와야함.
            _NowState = "Attack";

            return;
        }

        //적이 공격 범위 안에 있을 때
        else
        {
            if (_stopAttack == false)
            {
                //Range Off
                _IsRange = false;
                _attackRange[_SaveRangeNum].SetActive(_IsRange);

                //플레이어 평타 타입에 따른 변환
                //원거리일시
                switch (_pStats.AttackType)
                {
                    //원거리
                    case Define.PlayerAttackType.LongRange:
                        //Shoot
                        //Managers.Pool.Projectile_Pool("PoliceBullet", _Proj_Parent.position, _lockTarget.transform,
                        //5.0f, _pStats._basicAttackPower);

                        Debug.Log("Shoot");

                        break;

                    //근거리
                    case Define.PlayerAttackType.ShortRange:
                        Debug.Log("근접 공격");

                        break;

                }

                //타겟을 향해 회전 및 멈추기
                transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, _MovingPos) - transform.position);
                _agent.ResetPath();

                //평타 쿨타임
                _stopAttack = true;

                //애니메이션 Idle로 변환
                State = Define.State.Idle;

                return;
            }
        }
    }


    protected override void UpdateSkill()
    {
        //이동을 스킵 안함 && 적이 공격 범위 밖에 있을 때 Moving 전환 
        if (_NowState != "SkipMove" && Vector3.Distance(this.transform.position, _MovingPos) > _cardStats._rangeScale)
        {
            //애니메이션 Moving으로 변한
            State = Define.State.Moving;
            //현재 상태는 Skill -> Moving에서 Skill로 와야함.
            _NowState = "Skill";

            return;
        }

        else
        {
            if (_stopSkill == false)
            {
                //Range Off
                _IsRange = false;
                if (_cardStats._rangeType != "None")
                {
                    _attackRange[_SaveRangeNum].SetActive(_IsRange);
                }

                if (_MovingPos != default)
                {
                    //Skill On
                    GameObject go = new GameObject("Particle");
                    go.transform.position = _MovingPos;
                    _cardStats.cardEffect(go.transform);
                    _cardStats.InitCard();
                    Destroy(go, 2.0f);

                    //타겟을 향해 회전 및 멈추기
                    transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, _MovingPos) - transform.position);
                    _agent.ResetPath();
                }

                //스킬 쿨타임
                _stopSkill = true;

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
            _NowState = null;
        }

        if (_SaveAttackSpeed != default)
        {
            _SaveAttackSpeed += Time.deltaTime;

            if (_SaveAttackSpeed >= _pStats._attackDelay)
            {
                Debug.Log("Attack Ready");
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
            _lockTarget = null;
            _MovingPos = default;
            _NowState = null;

            Managers.Input.MouseAction = null;
            Managers.Input.KeyAction = null;
        }

        if (_SaveSkillCool != default)
        {
            _SaveSkillCool += Time.deltaTime;

            //스킬 시전 시간동안 키 입력 X
            if (_SaveSkillCool >= _cardStats._CastingTime)
            {
                Debug.Log("스킬 시전 완료");
                _stopSkill = false;
                _SaveSkillCool = default;
                Managers.Input.MouseAction += MouseDownAction;
                Managers.Input.KeyAction += KeyDownAction;
            }
        }
    }

}
