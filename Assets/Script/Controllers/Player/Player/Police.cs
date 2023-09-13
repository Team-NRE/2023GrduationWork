using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.StatusIndicators.Components;
using UnityEngine.AI;
using Stat;
using Photon.Pun;
using static UnityEngine.GraphicsBuffer;

public class Police : BaseController
{
    #region Variable
    //총알 위치
    private Transform _Proj_Parent;
    private GameObject _bullet;
    public GameObject target;
    private GameObject _netBullet;

    //UI_Card 접근
    private UI_Card _cardStats;

    //PlayerAttackRange
    public List<GameObject> _attackRange = new List<GameObject>();

    //부활 유무
    private bool _isResurrection = false;

    //평타/스킬/스텟 쿨타임
    public float _SaveAttackSpeed = default;
    public float _SaveSkillCool = default;
    public float _SaveHPSCool = default;
    public float _SaveRespawnTime = default;

    //좌표에 포함안되는 레이어
    private LayerMask ignore;

    //범위 넘버 저장
    private int _SaveRangeNum;

    //리스폰
    public Transform respawn;

    protected BaseProjectile _baseProj;
    public GameObject bullet;
    #endregion


    //리스폰 후 재설정
    public void OnEnable()
    {
        _state = Define.State.Idle;

        //리스폰 지역
        respawn = GameObject.Find("HumanRespawn").transform;

        GetComponent<NavMeshAgent>().enabled = false;
        transform.position = respawn.position;
        GetComponent<NavMeshAgent>().enabled = true;

        //액션 대리자
        Managers.Input.MouseAction += MouseDownAction;
        Managers.Input.KeyAction += KeyDownAction;
    }


    //start 초기화
    public override void Init()
    {
        //초기화
        _pStats = GetComponent<PlayerStats>();
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _pv = GetComponent<PhotonView>();

        //스텟 호출
        _pType = Define.PlayerType.Police;
        if (photonView.IsMine)
            _pv.RPC(
                "PlayerStatSetting",
                RpcTarget.All,
                _pType.ToString(),
                // Managers.game.nickname
                PhotonNetwork.LocalPlayer.NickName
            );

        //총알 위치
        _Proj_Parent = this.transform.GetChild(2);
        //_bullet = Managers.Resource.Load<GameObject>($"Prefabs/Projectile/{this.gameObject.name}Bullet");

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

        //마우스 이벤트 시 무시할 레이어
        ignore = LayerMask.GetMask("Default", "Ignore Raycast");
    }


    //Mouse event
    private void MouseDownAction(Define.MouseEvent evt)
    {
        if (_pv.IsMine)
        {
            (Vector3, GameObject) _mousePos = Managers.Input.Get3DMousePosition(ignore);

            //클릭될 때 잡히는 오브젝트가 없다면
            if (_mousePos.Item2 == null) return;

            if (_mousePos.Item2 != null)
            {
                switch (evt)
                {
                    //마우스 오른쪽 버튼 클릭 시
                    case Define.MouseEvent.PointerDown:
                        //공격 타입
                        _proj = Define.Projectile.Attack_Proj;

                        //좌표, 타겟 설정(도로 클릭 시 공격 타입 -> None 타입으로 변경)
                        TargetSetting(_mousePos.Item1, _mousePos.Item2);

                        //사거리가 켜져있다면 Off
                        if (_IsRange == true)
                        {
                            KeyPushState("MouseRightButton");
                        }

                        //일단 Move
                        State = Define.State.Moving;

                        break;


                    //마우스 오른쪽 버튼 누르고 있을 시
                    case Define.MouseEvent.Press:
                        //공격 타입
                        _proj = Define.Projectile.Attack_Proj;

                        //좌표, 타겟 설정(도로 클릭 시 공격 타입 -> None 타입으로 변경)
                        TargetSetting(_mousePos.Item1, _mousePos.Item2);

                        //사거리가 켜져있다면 Off
                        if (_IsRange == true)
                        {
                            KeyPushState("MouseRightButton");
                        }

                        //일단 Move
                        State = Define.State.Moving;

                        break;


                    //마우스 왼쪽 버튼 클릭 시
                    case Define.MouseEvent.LeftButton:
                        //Range가 On일 때만 좌클릭 시
                        if (_IsRange == true)
                        {
                            //스킬일 때
                            if (_proj == Define.Projectile.Skill_Proj)
                            {
                                //Range 카드 = 타겟 카드
                                if (_SaveRangeNum == (int)Define.CardType.Range)
                                {
                                    //좌표, 타겟 설정
                                    TargetSetting(_mousePos.Item1, _mousePos.Item2);

                                    State = Define.State.Moving;
                                }

                                //Range 카드 = 포인트 카드
                                if (_SaveRangeNum == (int)Define.CardType.Point)
                                {
                                    //Range 좌표 = Effect 위치 
                                    _MovingPos = _attackRange[_SaveRangeNum].transform.position;

                                    //스킬 상태로 전환
                                    State = Define.State.Skill;
                                }

                                //나머지 카드 = 논타겟 카드
                                else
                                {
                                    //Range 좌표 = Effect 위치 
                                    _MovingPos = _mousePos.Item1;

                                    //회전
                                    transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, _MovingPos) - transform.position);

                                    //스킬 상태로 전환
                                    State = Define.State.Skill;
                                }
                            }

                            //평타일 때
                            if (_proj == Define.Projectile.Attack_Proj)
                            {
                                if (RangeAttack() == null) return;
                                if (RangeAttack() != null)
                                {
                                    //타겟 ID 찾기
                                    int targetId = GetRemotePlayerId(RangeAttack());
                                    GameObject remoteTarget = GetRemotePlayer(targetId);

                                    //좌표 설정
                                    _MovingPos = _mousePos.Item1;

                                    //타겟 오브젝트 설정
                                    BaseCard._lockTarget = remoteTarget;

                                    //공격 상태로 전환
                                    State = Define.State.Moving;
                                }
                            }
                        }

                        //Range Off일 때 아무일도 없음.
                        else return;

                        break;
                }
            }
        }
    }


    //마우스 클릭 시 좌표, 타겟 설정
    private void TargetSetting(Vector3 _mousePos, GameObject _lockTarget)
    {
        //도로 클릭 시
        if (_lockTarget.layer == (int)Define.Layer.Road)
        {
            //좌표 설정
            _MovingPos = _mousePos;

            //타겟 오브젝트 설정
            BaseCard._lockTarget = null;

            //공격 타입
            _proj = Define.Projectile.Undefine;
        }

        //적,중앙 오브젝트 클릭 시
        if (_lockTarget.layer == _pStats.enemyArea || _lockTarget.layer == (int)Define.Layer.Neutral)
        {
            //타겟 ID 찾기
            int targetId = GetRemotePlayerId(_lockTarget);
            GameObject remoteTarget = GetRemotePlayer(targetId);

            //좌표 설정
            _MovingPos = _mousePos;

            //타겟 오브젝트 설정
            BaseCard._lockTarget = remoteTarget;
        }
    }


    //Key event
    private void KeyDownAction(Define.KeyboardEvent _key)
    {

        //키보드 입력 시 _lockTarget 초기화 -> UI 변환 시간 벌어주기
        BaseCard._lockTarget = null;

        //키보드 입력 시
        switch (_key)
        {
            case Define.KeyboardEvent.Q:
                if (_pv.IsMine)
                {
                    if (_pStats.UseMana(_key.ToString()).Item1 == true)
                    {
                        KeyPushState(_key.ToString());
                    }
                }

                break;

            case Define.KeyboardEvent.W:
                if (_pv.IsMine)
                {
                    if (_pStats.UseMana(_key.ToString()).Item1 == true)
                    {
                        KeyPushState(_key.ToString());
                    }
                }

                break;

            case Define.KeyboardEvent.E:
                if (_pv.IsMine)
                {
                    if (_pStats.UseMana(_key.ToString()).Item1 == true)
                    {
                        KeyPushState(_key.ToString());
                    }
                }

                break;

            case Define.KeyboardEvent.R:
                if (_pv.IsMine)
                {
                    if (_pStats.UseMana(_key.ToString()).Item1 == true)
                    {
                        KeyPushState(_key.ToString());
                    }
                }

                break;

            case Define.KeyboardEvent.A:
                if (_pv.IsMine)
                {
                    KeyPushState(_key.ToString());
                }
                break;
        }

    }


    //키 누르면 상태 전환
    private void KeyPushState(string Keyname)
    {
        Debug.Log($"현재 누른 Range키: {Keyname} / 이전에 눌렀던 Range키: {BaseCard._NowKey}");

        //이전 키와 다른 키를 눌렀을 때
        if (Keyname != default && Keyname != BaseCard._NowKey)
        {
            //사거리 On/Off
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

        //이전과 같은 키를 눌렀을 경우
        if (Keyname != "MouseRightButton" && Keyname == BaseCard._NowKey)
        {
            //사거리 On/Off
            _IsRange = (_IsRange == true ? false : true);
        }

        //마우스 우클릭이 아니면 사거리 접근
        if (Keyname != "MouseRightButton")
        {
            //누른 키가 A버튼이 아닐때
            if (Keyname != Define.KeyboardEvent.A.ToString())
            {
                //해당 키 밑의 스크립트 찾아주기
                _cardStats = GameObject.Find(Keyname).GetComponentInChildren<UI_Card>();

                //카드의 Range 타입에 따른 세팅 변환
                switch (_cardStats._rangeType)
                {
                    //활 모양 Range
                    case Define.CardType.Arrow:
                        //사거리 On/Off
                        _attackRange[0].SetActive(_IsRange);

                        //사거리가 On일 때
                        if (_IsRange == true)
                        {
                            _SaveRangeNum = (int)Define.CardType.Arrow;

                            //현재 누른 키 정보를 static으로 저장
                            BaseCard._NowKey = Keyname;

                            //논타겟
                            BaseCard._lockTarget = null;

                            //스킬 타입
                            _proj = Define.Projectile.Skill_Proj;

                            //Arrow는 Scale 값 고정
                            _attackRange[0].GetComponent<AngleMissile>().Scale = 10.0f;
                        }

                        break;


                    //콘 모양 Range
                    case Define.CardType.Cone:
                        //사거리 On/ Off
                        _attackRange[1].SetActive(_IsRange);

                        //사거리가 On일 때
                        if (_IsRange == true)
                        {
                            _SaveRangeNum = (int)Define.CardType.Cone;

                            //현재 누른 키 정보를 static으로 저장
                            BaseCard._NowKey = Keyname;

                            //논타겟
                            BaseCard._lockTarget = null;

                            //스킬 타입
                            _proj = Define.Projectile.Skill_Proj;

                            //스킬 범위 크기
                            _attackRange[1].GetComponent<Cone>().Scale = 2 * _cardStats._rangeScale;
                            //스킬 각도
                            _attackRange[1].GetComponent<Cone>().Angle = _cardStats._rangeAngle;
                        }

                        break;


                    //선 모양 Range
                    case Define.CardType.Line:
                        //사거리 On/Off
                        _attackRange[2].SetActive(_IsRange);

                        //사거리 On 일 때
                        if (_IsRange == true)
                        {
                            _SaveRangeNum = (int)Define.CardType.Line;

                            //현재 누른 키 정보를 static으로 저장
                            BaseCard._NowKey = Keyname;

                            //논타겟
                            BaseCard._lockTarget = null;

                            //스킬 타입
                            _proj = Define.Projectile.Skill_Proj;

                            //스킬 범위 크기
                            _attackRange[2].GetComponent<AngleMissile>().Scale = 2 * _cardStats._rangeScale;
                        }

                        break;


                    //포인트 모양 Range
                    case Define.CardType.Point:
                        //사거리 On/Off
                        _attackRange[3].SetActive(_IsRange);

                        //사거리 On일 때
                        if (_IsRange == true)
                        {
                            _SaveRangeNum = (int)Define.CardType.Point;

                            //현재 누른 키 정보를 static으로 저장
                            BaseCard._NowKey = Keyname;

                            //논타겟
                            BaseCard._lockTarget = null;

                            //스킬 타입
                            _proj = Define.Projectile.Skill_Proj;

                            //스킬 범위 크기
                            _attackRange[3].GetComponent<Point>().Scale = 2 * _cardStats._rangeScale;
                            //스킬 거리
                            _attackRange[3].GetComponent<Point>().Range = _cardStats._rangeRange;
                        }

                        break;


                    //원 모양 Range
                    case Define.CardType.Range:
                        //사거리 On/Off
                        _attackRange[4].SetActive(_IsRange);

                        //사거리 On일 때
                        if (_IsRange == true)
                        {
                            _SaveRangeNum = (int)Define.CardType.Range;

                            //현재 누른 키 정보를 static으로 저장
                            BaseCard._NowKey = Keyname;

                            //스킬 범위 크기
                            Projector projector = _attackRange[4].GetComponent<Projector>();

                            //스킬 타입
                            _proj = Define.Projectile.Skill_Proj;
                            projector.orthographicSize = _cardStats._rangeScale;
                        }

                        break;


                    //즉시 사용
                    case Define.CardType.None:
                        //현재 누른 키 정보를 static으로 저장
                        BaseCard._NowKey = Keyname;

                        //자기 자신
                        _MovingPos = this.transform.position;

                        //논타겟
                        BaseCard._lockTarget = null;

                        //스킬 상태
                        State = Define.State.Skill;

                        break;
                }
            }


            //누른 키가 A버튼일때
            if (Keyname == Define.KeyboardEvent.A.ToString())
            {
                //사거리 On/Off
                _attackRange[4].SetActive(_IsRange);

                //사거리 On일 때
                if (_IsRange == true)
                {
                    //현재 누른 키 정보를 static으로 저장
                    BaseCard._NowKey = Keyname;

                    _SaveRangeNum = (int)Define.CardType.Range;

                    //스킬 범위 크기
                    Projector projector = _attackRange[4].GetComponent<Projector>();

                    //평타 타입
                    _proj = Define.Projectile.Attack_Proj;
                    projector.orthographicSize = _pStats.attackRange;
                }
            }
        }
    }


    //A키 공격
    protected override GameObject RangeAttack()
    {
        float dist = 999;
        target = null;

        Collider[] cols = Physics.OverlapSphere(transform.position, _pStats.attackRange, 1 << _pStats.enemyArea);

        foreach (Collider col in cols)
        {
            float Distance = Vector3.Distance(col.transform.position, transform.position);
            if (Distance <= _pStats.attackRange && Distance < dist)
            {
                dist = Distance;
                target = col.gameObject;
            }
        }

        return target;
    }


    //상시로 바뀌는 플레이어 스텟 
    protected override void UpdatePlayerStat()
    {
        //초기 세팅
        if (_SaveHPSCool == default)
        {
            _SaveHPSCool = 0.01f;
        }

        if (_SaveHPSCool != default)
        {
            _SaveHPSCool += Time.deltaTime;

            if (_SaveHPSCool >= 1)
            {
                //피 회복
                _pStats.nowHealth += _pStats.healthRegeneration;

                //attackDelay 초기화
                _SaveHPSCool = default;
            }
        }

    }


    //Idle
    protected override void UpdateIdle()
    {
        //죽었을 때
        if (_pStats.nowHealth <= 0) { State = Define.State.Die; }

        //살았을 때
        if (_pStats.nowHealth > 0 && _agent.remainingDistance < 0.2f)
        {
            State = Define.State.Idle;
        }
    }


    //Moving
    protected override void UpdateMoving()
    {

        //Die
        if (_pStats.nowHealth <= 0) { State = Define.State.Die; }

        if (_pv.IsMine)
        {
            //타겟 - Attack or Skill or Move
            switch (_proj)
            {
                //Attack
                case Define.Projectile.Attack_Proj:
                    if (BaseCard._lockTarget == null)
                    {
                        _agent.ResetPath();

                        break;
                    }

                    if (BaseCard._lockTarget != null)
                    {
                        //이동
                        transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, BaseCard._lockTarget.transform.position) - transform.position);
                        _agent.SetDestination(BaseCard._lockTarget.transform.position);

                        if (_agent.remainingDistance <= _pStats.attackRange)
                        {
                            State = Define.State.Attack;

                            break;
                        }

                        else
                        {
                            State = Define.State.Moving;
                        }
                    }

                    break
;

                //Skill
                case Define.Projectile.Skill_Proj:
                    //논타겟 카드일 때
                    if (BaseCard._lockTarget == null)
                    {
                        _agent.ResetPath();

                        break;
                    }

                    //타겟 카드일 때
                    if (BaseCard._lockTarget != null)
                    {
                        //이동
                        transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, BaseCard._lockTarget.transform.position) - transform.position);
                        _agent.SetDestination(BaseCard._lockTarget.transform.position);

                        if (_agent.remainingDistance <= _cardStats._rangeScale)
                        {
                            State = Define.State.Skill;

                            break;
                        }

                        else
                        {
                            State = Define.State.Moving;
                        }
                    }

                    break;

                //Move
                case Define.Projectile.Undefine:
                    //이동
                    transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, _MovingPos) - transform.position);
                    _agent.SetDestination(_MovingPos);

                    State = Define.State.Moving;

                    break;
            }

            //Idle
            if (_agent.remainingDistance < 0.2f)
            {
                State = Define.State.Idle;
            }
        }

        else
        {
            // 수신된 좌표로 보간한 이동처리
            transform.position = Vector3.Lerp(
                transform.position,
                receivePos,
                Time.deltaTime * damping
            );

            // 수신된 회전값으로 보간한 회전처리
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                receiveRot,
                Time.deltaTime * damping
            );
        }
    }


    //Attack
    protected override void UpdateAttack()
    {
        //죽었을 때
        if (_pStats.nowHealth <= 0) { State = Define.State.Die; }

        //살았을 때
        if (_pStats.nowHealth > 0)
        {
            if (_stopAttack == false)
            {
                //Range Off
                _IsRange = false;
                _attackRange[4].SetActive(_IsRange);

                Debug.Log(BaseCard._lockTarget.name);

                //플레이어 평타 타입에 따른 변환
                //원거리일시
                switch (_pStats.attackType)
                {
                    case "LongRange":
                        if (BaseCard._lockTarget != null)
                        {
                            //Shoot
                            string tempName = "PoliceBullet";
                            _netBullet = PhotonNetwork.Instantiate(tempName, _Proj_Parent.position, _Proj_Parent.rotation);
                            PhotonView localPv = _netBullet.GetComponent<PhotonView>();
                            localPv.RPC("Init", RpcTarget.All, BaseCard._lockTarget.GetComponent<PhotonView>().ViewID);
                            //_pv.RPC("SetProjectile", RpcTarget.All, BaseCard._lockTarget.GetComponent<PhotonView>().ViewID);
                        }
                        break;

                    case "ShortRange":
                        if (BaseCard._lockTarget != null)
                        {
                            //타겟이 미니언, 타워일 시 
                            if (BaseCard._lockTarget.tag != "PLAYER")
                            {
                                ObjStats _Stats = BaseCard._lockTarget.GetComponent<ObjStats>();
                                _Stats.nowHealth -= _pStats.basicAttackPower;
                                _pv.RPC("EnemyHPLog", RpcTarget.All, _Stats.nowHealth.ToString());
                            }

                            //타겟이 적 Player일 시
                            if (BaseCard._lockTarget.tag == "PLAYER")
                            {
                                PlayerStats _Stats = BaseCard._lockTarget.GetComponent<PlayerStats>();
                                _Stats.receviedDamage = _pStats.basicAttackPower;
                                _pv.RPC("EnemyHPLog", RpcTarget.All, _Stats.nowHealth.ToString());
                            }
                        }

                        break;

                    default:
                        Debug.Log("평타 불가");

                        break;
                }

                _agent.ResetPath();

                //평타 쿨타임
                _stopAttack = true;

                //애니메이션 Idle로 변환
                State = Define.State.Idle;

                return;
            }
        }
    }


    //Skill
    protected override void UpdateSkill()
    {
        //죽었을 때
        if (_pStats.nowHealth <= 0) { State = Define.State.Die; }

        //살았을 때
        if (_pStats.nowHealth > 0)
        {
            if (_stopSkill == false)
            {
                //Range Off
                _IsRange = false;
                if (_SaveRangeNum != (int)Define.CardType.None)
                {
                    _attackRange[_SaveRangeNum].SetActive(_IsRange);
                }

                //이펙트 발동
                if (_MovingPos != default)
                {
                    Debug.Log($"UpdateSkill : {_MovingPos} ");
                    //Skill On
                    _cardStats.InitCard();
                    GameObject effectObj = _cardStats.cardEffect(_MovingPos, this.photonView.ViewID, _pStats.playerArea);

                    //이펙트가 특정 시간 후에 사라진다면
                    if (_cardStats._effectTime != default)
                    {
                        Destroy(effectObj, _cardStats._effectTime);
                    }

                    //부활이 켜져있으면
                    if (_cardStats._effectTime == default)
                    {
                        _isResurrection = _cardStats._IsResurrection;
                    }

                }

                _agent.ResetPath();

                //스킬 쿨타임
                _stopSkill = true;

                State = Define.State.Idle;

                return;
            }
        }
    }


    //Die
    protected override void UpdateDie()
    {
        _startDie = true;

        //스킬 시전 시간동안 키 입력 X
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.KeyAction -= KeyDownAction;

        _IsRange = false;
        _attackRange[_SaveRangeNum].SetActive(_IsRange);

        GetComponent<CapsuleCollider>().enabled = false;

        _SaveRespawnTime += Time.deltaTime;

    }


    //리스폰 중
    protected override void StartDie()
    {
        //초기 세팅
        if (_SaveRespawnTime == default)
        {
            //attack Delay start
            _SaveRespawnTime = 0.01f;
        }

        if (_SaveRespawnTime != default)
        {
            _SaveRespawnTime += Time.deltaTime;


            //부활 없을 시 (6 -> 부활 시간 임의로 정함) 
            if (_SaveRespawnTime >= 6.0f && _isResurrection == false)
            {
                //리스폰 지역
                //transform.position = respawn.position;
                //Hp reset
                _pStats.nowHealth = _pStats.maxHealth;

                //Respawn
                _startDie = false;
                _SaveRespawnTime = default;
                _state = Define.State.Idle;
                GetComponent<CapsuleCollider>().enabled = true;
                Managers.Input.MouseAction += MouseDownAction;
                Managers.Input.KeyAction += KeyDownAction;
            }


            //부활 있을 시
            if (_SaveRespawnTime >= 3.0f && _isResurrection == true)
            {
                //HP 70% back
                _pStats.nowHealth = _pStats.maxHealth / 100 * 70;

                //부활권 삭제
                _isResurrection = false;
                GameObject Resurrection = transform.Find("Effect_Resurrection").gameObject;
                Destroy(Resurrection);

                //Respawn
                _startDie = false;
                _SaveRespawnTime = default;
                _state = Define.State.Idle;
                GetComponent<CapsuleCollider>().enabled = true;
                Managers.Input.MouseAction += MouseDownAction;
                Managers.Input.KeyAction += KeyDownAction;
            }

        }
    }


    //평타 후 딜레이
    protected override void StopAttack()
    {
        //초기 세팅
        if (_SaveAttackSpeed == default)
        {
            //attack Delay start
            _SaveAttackSpeed = 0.01f;

            //마우스 좌표, 타겟 초기화
            _MovingPos = default;
            BaseCard._lockTarget = null;
        }

        //attack Delay start
        if (_SaveAttackSpeed != default)
        {
            //attack Delay start
            _SaveAttackSpeed += Time.deltaTime;

            //플레이어 평타 딜레이 시간 지나면,
            if (_SaveAttackSpeed >= _pStats.attackDelay)
            {
                //attackDelay 초기화
                _SaveAttackSpeed = default;
                //StopAttack() update문 stop
                _stopAttack = false;
            }
        }
    }


    //스킬 사용 후 딜레이
    protected override void StopSkill()
    {
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

                //키 입력 재 시작
                Managers.Input.MouseAction += MouseDownAction;
                Managers.Input.KeyAction += KeyDownAction;

                //Debug.Log("스킬 시전 완료");
            }
        }
    }


    //Projectile 설정
    [PunRPC]
    protected void SetProjectile(int id)
    {
        if (_netBullet != null)
            _netBullet.GetComponent<RangedBullet>().Init(id);
    }

    //근접 공격 시 적 HP 관리
    [PunRPC]
    private void EnemyHPLog(string log)
    {
        Debug.Log(log);
    }
}
