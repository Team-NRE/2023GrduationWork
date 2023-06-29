using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.StatusIndicators.Components;
using UnityEngine.AI;
using Stat;
using Photon.Pun;


public class Police : BaseController
{
<<<<<<< HEAD
    //플레이어 스텟 초기화
    private PlayerStats _pStats;
    //총알 위치
    public Transform _Proj_Parent;
=======
    //총알 위치
    private Transform _Proj_Parent;
    private GameObject _bullet;
    public GameObject target;
>>>>>>> SinglePlayVersion
    //UI_Card 접근
    private UI_Card _cardStats;
    protected BaseProjectile _baseProj;

    //PlayerAttackRange
    public List<GameObject> _attackRange = new List<GameObject>();

    //현재 상태
    private string _NowState = null;

    //타겟 유무
    private bool _IsTarget = false;
    private bool _IsRange = false;

    //범위 넘버 저장
    private int _SaveRangeNum;


    //평타/스킬 쿨타임
    private float _SaveAttackSpeed = default;
    private float _SaveSkillCool = default;

<<<<<<< HEAD
    public void OnEnable()
    {
        GetComponent<CapsuleCollider>().enabled = true;
    }


	//start 초기화
	public override void Init()
    {
        //_pv = GetComponent<PhotonView>();
        //초기화
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _pStats = GetComponent<PlayerStats>();
        _pStats.playerArea = this.gameObject.layer;
=======
    private LayerMask ignore;


    public void OnEnable()
    {
        _state = Define.State.Idle;

>>>>>>> SinglePlayVersion
        //액션 대리자 호출
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.MouseAction += MouseDownAction;
        Managers.Input.KeyAction -= KeyDownAction;
        Managers.Input.KeyAction += KeyDownAction;
    }

        _baseProj = Managers.Resource.Load<BaseProjectile>("PoliceBullet");

    //start 초기화
    public override void Init()
    {
        //초기화
        _pStats = GetComponent<PlayerStats>();
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        //스텟 호출
        _pType = Define.PlayerType.Police;
        _pStats.PlayerStatSetting(_pType);

        //총알 위치
<<<<<<< HEAD
        //if (_pv.IsMine)
        //_Proj_Parent = this.transform.Find("Barrel_Location").transform;

=======
        _Proj_Parent = this.transform.GetChild(2);
        _bullet = Managers.Resource.Load<GameObject>($"Prefabs/Projectile/{this.gameObject.name}Bullet");
>>>>>>> SinglePlayVersion

        //Range List Setting 
        GetComponentInChildren<SplatManager>().enabled = false;
        GameObject[] loadAttackRange = Resources.LoadAll<GameObject>("Prefabs/AttackRange/");
        foreach (GameObject attackRange in loadAttackRange)
        {
            GameObject Prefab = Instantiate(attackRange);
            Prefab.transform.parent = transform.GetChild(0);
            Prefab.transform.localPosition = Vector3.zero;
            Prefab.SetActive(true);

            _attackRange.Add(Prefab);
        }

        //AttackRange 초기화
        //Projector projector = _attackRange[0].GetComponent<Projector>();
        //projector.orthographicSize = _pStats._attackRange;

        //Prefab off
        GetComponentInChildren<SplatManager>().enabled = true;

        //Object Target 정하는 리스트
        ObjectController._allObjectTransforms.Add(transform);

        //마우스 이벤트 시 무시할 레이어
        ignore = LayerMask.GetMask("Default", "Ignore Raycast");
    }

    //Mouse event
    private void MouseDownAction(Define.MouseEvent evt)
    {
<<<<<<< HEAD
        (Vector3, GameObject) _MousePos = Managers.Input.Get3DMousePosition((1 << 0 | 1 << 2));
        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                MouseClickState(_MousePos.Item1, _MousePos.Item2);

                break;

            case Define.MouseEvent.Press:
                MouseClickState(_MousePos.Item1, _MousePos.Item2);
=======
        (Vector3, GameObject) _mousePos = Managers.Input.Get3DMousePosition(ignore);

        if (_mousePos.Item2 != null)
        {
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
>>>>>>> SinglePlayVersion

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
                                //Effect 좌표 설정
                                _MovingPos = _mousePos.Item1;
                                BaseCard._lockTarget = _mousePos.Item2;

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
                        //Debug.Log("Range Off 입니다.");
                        return;
                    }

                    break;
            }
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

<<<<<<< HEAD
                State = Define.State.Moving;
            }

            //적 or 중앙 obj 클릭 시
            //_pStats.enemyArea가 상수반환이 안되서 if문으로 대체
            if (hitObjectLayer == _enemyLayer || hitObjectLayer == (int)Define.Layer.Neutral)
=======
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
                    if(RangeAttack() != null)
                    {
                        //좌표 설정
                        _MovingPos = RangeAttack().transform.position;
                        BaseCard._lockTarget = RangeAttack();

                        //State Moving 변환
                        State = Define.State.Attack;
                    }

                    break;
            }

        }

        //적 or 중앙 obj 클릭 시
        //_pStats.enemyArea가 상수반환이 안되서 if문으로 대체
        if (lockTarget.layer == 7 || lockTarget.layer == (int)Define.Layer.Neutral)
        {
            //좌표 설정
            _MovingPos = mousePos;
            //타겟 오브젝트 설정
            BaseCard._lockTarget = lockTarget;

            //Attack or Skill
            switch (_proj)
>>>>>>> SinglePlayVersion
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
<<<<<<< HEAD
=======
        //키보드 입력 시 _lockTarget 초기화 -> UI 변환 시간 벌어주기 
        BaseCard._lockTarget = null;
>>>>>>> SinglePlayVersion

        //키보드 입력 시
        switch (_key)
        {
            case Define.KeyboardEvent.Q:
                string Q_key = "Q";
                if (_pStats.UseMana(Q_key).Item1 == true)
                {
                    KeyPushState(Q_key);
                }

                break;

            case Define.KeyboardEvent.W:
                string W_key = "W";
                if (_pStats.UseMana(W_key).Item1 == true)
                {
                    KeyPushState(W_key);
                }

                break;

            case Define.KeyboardEvent.E:
                string E_key = "E";
                if (_pStats.UseMana(E_key).Item1 == true)
                {
                    KeyPushState(E_key);
                }

                break;

            case Define.KeyboardEvent.R:
                string R_key = "R";
                if (_pStats.UseMana(R_key).Item1 == true)
                {
                    KeyPushState(R_key);
                }

                break;

            case Define.KeyboardEvent.A:
                KeyPushState("A");

                break;
        }

    }


    private void KeyPushState(string Keyname)
    {
        //스킬 사거리 표시 On/Off 관리
        //이전 키와 같은 키를 눌렀을 경우 
        if (Keyname != "MouseRightButton" && Keyname == BaseCard._NowKey)
        {
            //사거리 On/Off 관리
            _IsRange = (_IsRange == true ? false : true);
        }
        //이전 키와 다른 키를 눌렀을 때
        if (Keyname != BaseCard._NowKey && Keyname != default)
        {
            //사거리 On/Off 관리
            switch (_IsRange)
            {
                //이전 키 사거리가 켜져있을 경우
                case true:
                    //이전 키 사거리를 Off
                    _attackRange[_SaveRangeNum].SetActive(false);
                    if (Keyname == "MouseRightButton")
                    {
                        _IsRange = false;
                    }

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

<<<<<<< HEAD
                    //카드의 Range 타입에 따른 세팅 변환
                    switch (_cardStats._rangeType)
                    {
                        //활 모양 Range 
                        case "Arrow":
                            //번호 저장
                            _SaveRangeNum = 0;
                            //사거리 On/Off
                            _attackRange[0].SetActive(_IsRange);
=======
                        //사거리가 On일 때
                        if (_IsRange == true)
                        {
                            BaseCard._NowKey = Keyname;
>>>>>>> SinglePlayVersion

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

<<<<<<< HEAD
                        //콘 모양 Range
                        case "Cone":
                            //번호 저장
                            _SaveRangeNum = 1;
                            //사거리 On/ Off
                            _attackRange[1].SetActive(_IsRange);
=======
                        //사거리가 On일 때
                        if (_IsRange == true)
                        {
                            BaseCard._NowKey = Keyname;
>>>>>>> SinglePlayVersion

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

<<<<<<< HEAD
                            //사거리 On/Off
                            _attackRange[2].SetActive(_IsRange);
=======
                        //사거리 On 일 때
                        if (_IsRange == true)
                        {
                            BaseCard._NowKey = Keyname;
>>>>>>> SinglePlayVersion

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

<<<<<<< HEAD
                            //사거리 On/Off
                            _attackRange[3].SetActive(_IsRange);
=======
                        //사거리 On일 때
                        if (_IsRange == true)
                        {
                            BaseCard._NowKey = Keyname;
>>>>>>> SinglePlayVersion

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

<<<<<<< HEAD
                            //사거리 On/Off
                            _attackRange[4].SetActive(_IsRange);
=======
                        //사거리 On일 때
                        if (_IsRange == true)
                        {
                            BaseCard._NowKey = Keyname;
>>>>>>> SinglePlayVersion

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
<<<<<<< HEAD
=======

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
                    BaseCard._NowKey = Keyname;

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

>>>>>>> SinglePlayVersion
    }

    protected override GameObject RangeAttack()
    {
        float dist = 999;
        target = null;

        Collider[] cols = Physics.OverlapSphere(transform.position, _pStats.attackRange, 1 << _pStats.enemyArea);

        foreach(Collider col in cols)
        {
            float Distance = Vector3.Distance(col.transform.position, transform.position);
            if(Distance <= _pStats.attackRange && Distance < dist)
            {
                dist = Distance;
                target = col.gameObject;
            }
        }
        
        return target;
    }

    protected override void UpdateIdle()
    {
        if (_agent.remainingDistance < 0.2f)
        {
            State = Define.State.Idle;
        }

        if (_pStats.nowHealth <= 0) { State = Define.State.Die; }
    }


    protected override void UpdateMoving()
    {
        if (_pv.IsMine)
        {
            transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, _MovingPos) - transform.position);
            _agent.SetDestination(_MovingPos);

<<<<<<< HEAD
            // 조건 만족 시 상태 변환
            if (_lockTarget == null && _agent.remainingDistance < 0.2f) { State = Define.State.Idle; }
            if (_lockTarget != null && _agent.remainingDistance <= _pStats._attackRange && _stopAttack == false) { State = Define.State.Attack; }
=======
        // 조건 만족 시 상태 변환
        //Idle
        if (_agent.remainingDistance < 0.2f) { State = Define.State.Idle; }
        //Attack
        if (_NowState == "Attack" && _agent.remainingDistance <= _pStats.attackRange)
        {
            State = Define.State.Attack;
>>>>>>> SinglePlayVersion
        }
        else
        {
<<<<<<< HEAD
            //Debug.Log("else moving");

            // 수신된 좌표로 보간한 이동처리
            transform.position = Vector3.Lerp(transform.position,
                                              receivePos,
                                              Time.deltaTime * damping);

            // 수신된 회전값으로 보간한 회전처리
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  receiveRot,
                                                  Time.deltaTime * damping);

=======
            Managers.Input.MouseAction = null;
            Managers.Input.KeyAction = null;
            State = Define.State.Skill;
>>>>>>> SinglePlayVersion
        }
        //Die
        if (_pStats.nowHealth <= 0) { State = Define.State.Die; }
    }

    [PunRPC]
    protected override void UpdateAttack()
    {
        //죽었을 때
        if (_pStats.nowHealth <= 0) { State = Define.State.Die; }
        //적이 공격 범위 밖에 있을 때 Moving 전환
        if (Vector3.Distance(this.transform.position, _MovingPos) > _pStats.attackRange)
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
                switch (_pStats.attackType)
                {
<<<<<<< HEAD
                    //원거리
                    case Define.PlayerAttackType.LongRange:
                        //Shoot
                        //Managers.Pool.Projectile_Pool("PoliceBullet", _Proj_Parent.position, _lockTarget.transform, 5.0f, _pStats._basicAttackPower);
                        //GameObject bullet = PhotonNetwork.Instantiate("PoliceBullet", _Proj_Parent.position, Quaternion.identity);
                        //StartCoroutine("ShootTarget", "PoliceBullet");
                        fp_Fire();
                        Debug.Log("Shoot");

=======
                    case "LongRange":
                        if (BaseCard._lockTarget != null)
                        {
                            //Shoot
                            GameObject nowBullet = Instantiate(_bullet, _Proj_Parent.position, _Proj_Parent.rotation);
                            nowBullet.GetComponent<RangedBullet>().BulletSetting(_Proj_Parent.position, BaseCard._lockTarget.transform, _pStats.speed, _pStats.basicAttackPower);
                        }
>>>>>>> SinglePlayVersion
                        break;

                    case "ShortRange":
                        Debug.Log("근접 공격");

                        break;

                    default:
                        Debug.Log("평타 불가");

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
<<<<<<< HEAD
        Debug.Log("UpdateSkill");
=======
        //죽었을 때
        if (_pStats.nowHealth <= 0) { State = Define.State.Die; }
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
                    GameObject ground = new GameObject("Particle");
                    ground.transform.position = _MovingPos;

                    _cardStats.InitCard();
                    GameObject effectObj = _cardStats.cardEffect(ground.transform, this.transform, 1 << 6);

                    Destroy(effectObj, _cardStats._effectTime);
                    Destroy(ground, _cardStats._effectTime);

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
>>>>>>> SinglePlayVersion
    }

    protected override void UpdateDie()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        //스킬 시전 시간동안 키 입력 X
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.KeyAction -= KeyDownAction;

        _IsRange = false;
        _attackRange[_SaveRangeNum].SetActive(_IsRange);

        this.enabled = false;
    }

    protected override void StopAttack()
    {
<<<<<<< HEAD
        if (_SaveAttackSpeed == default) { _SaveAttackSpeed = 0.01f; }
=======
        //초기 세팅
        if (_SaveAttackSpeed == default)
        {
            //attack Delay start
            _SaveAttackSpeed = 0.01f;

            //현재 상태 초기화
            _NowState = null;
            //마우스 좌표, 타겟 초기화
            _MovingPos = default;
            BaseCard._lockTarget = null;
        }

        //attack Delay start
>>>>>>> SinglePlayVersion
        if (_SaveAttackSpeed != default)
        {
            //attack Delay start
            _SaveAttackSpeed += Time.deltaTime;

            //플레이어 평타 딜레이 시간 지나면,
            if (_SaveAttackSpeed >= _pStats.attackDelay)
            {
<<<<<<< HEAD
                _stopAttack = false;
=======
                //attackDelay 초기화
>>>>>>> SinglePlayVersion
                _SaveAttackSpeed = default;
                //StopAttack() update문 stop
                _stopAttack = false;
            }
        }
    }


    //AttackRange On/Off
    private void AttRange_Active()
    {
<<<<<<< HEAD
        if (_IsRange == true || _IsRange == false)
        {
            _IsRange = !_IsRange;
            _attackRange[0].SetActive(_IsRange);
=======
        //초기 세팅
        if (_SaveSkillCool == default)
        {
            //스킬 시전 시간 벌어주는 Delay Start
            _SaveSkillCool = 0.01f;

            //스킬 시전 시간동안 키 입력 X
            Managers.Input.MouseAction -= MouseDownAction;
            Managers.Input.KeyAction -= KeyDownAction;
        }

        //스킬 시전 시간 벌어주는 Delay Start
        if (_SaveSkillCool != default)
        {
            //스킬 시전 시간 벌어주는 Delay Start
            _SaveSkillCool += Time.deltaTime;

            //지정해준 Delay 가 지나면,
            if (_SaveSkillCool >= _cardStats._CastingTime)
            {
                //Skill Delay 초기화
                _SaveSkillCool = default;
                //StopSkill() Update문에서 Stop
                _stopSkill = false;

                //마우스 좌표 초기화
                _MovingPos = default;
                //현재 상태 초기화
                _NowState = null;

                //키 입력 재 시작
                Managers.Input.MouseAction += MouseDownAction;
                Managers.Input.KeyAction += KeyDownAction;

                //Debug.Log("스킬 시전 완료");
            }
>>>>>>> SinglePlayVersion
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
    
    public void fp_Fire()
	{
        GameObject bullet = PhotonNetwork.Instantiate("PoliceBullet", _Proj_Parent.position, this.gameObject.transform.rotation);
        //Debug.Log($"hit Info : {_lockTarget.gameObject.name}");
        _baseProj.GetComponent<PlayerProjectile>().pTarget = _lockTarget;
        _baseProj.GetComponent<PlayerProjectile>().pAttacker = this.gameObject;
        _baseProj.GetComponent<PlayerProjectile>().Fire(_lockTarget.gameObject, this.gameObject);
        //bullet.GetComponent<PlayerProjectile>().Fire(_lockTarget.gameObject, this.gameObject);

    }
}