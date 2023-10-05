using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.StatusIndicators.Components;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using Stat;
using Photon.Pun;

public class Players : BaseController
{
    /// <summary> 사거리 표시 List </summary>
    public List<GameObject> _attackRange = new List<GameObject>();

    /// <summary>  카드 정보 참조 </summary>
    protected UI_Card _cardStats;

    /// <summary> 좌표에 포함안되는 레이어 </summary>
    public LayerMask ignore;

    /// <summary> 범위 넘버 저장 </summary>
    public int _SaveRangeNum;

    /// <summary>
    /// 스텟 쿨타임
    /// </summary>
    public float _SaveRegenCool = default;

    /// <summary>
    /// 평타 좌클릭 타겟 지정시
    /// </summary>
    protected GameObject LeftMouseButtontarget;
    protected string enemyName;
    //한발만 쏘기 체크
    protected bool oneShot = false;

    public override void Init()
    {
        base.Init();

        //초기화
        _pStats = GetComponent<PlayerStats>();
        _anim = GetComponent<Animator>();
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _pv = GetComponent<PhotonView>();
        _grid = FindObjectOfType<GridLayout>();
        _tilemap = FindObjectOfType<Tilemap>();

        //Range List Setting
        GetComponentInChildren<SplatManager>().enabled = false;
        GameObject[] loadAttackRange = Resources.LoadAll<GameObject>("Prefabs/AttackRange");
        foreach (GameObject attackRange in loadAttackRange)
        {
            GameObject Prefab = Instantiate(attackRange);
            Prefab.transform.parent = transform.Find("Range");
            Prefab.transform.localPosition = Vector3.zero;
            Prefab.SetActive(true);

            _attackRange.Add(Prefab);
        }
        //Prefab off
        GetComponentInChildren<SplatManager>().enabled = true;


        //Object Target 정하는 리스트
        ObjectController._allObjectTransforms.Add(gameObject);


        //마우스 이벤트 시 무시할 레이어
        ignore = LayerMask.GetMask("Default", "Ignore Raycast");

        //부활 effect setting
        transform.Find("SpawnSimplePink").gameObject.SetActive(false);
    }

    public override void InitOnEnable()
    {
        //enemyName
        enemyName = (_pStats.playerArea == 6) ? "Cyborg" : "Human";

        //부활권 유무
        if (_pStats.isResurrection == true)
        {
            _pStats.isResurrection = false;
            GameObject Resurrection = transform.Find("Effect_Resurrection(Clone)").gameObject;
            PhotonNetwork.Destroy(Resurrection);
            //체력 회복
            _pv.RPC("photonStatSet", RpcTarget.All, "nowHealth", PercentageCount(70, _pStats.maxHealth, 1));
        }
        else if (_pStats.isResurrection == false)
        {
            //체력 회복
            _pv.RPC("photonStatSet", RpcTarget.All, "nowHealth", _pStats.maxHealth);
        }

        StartCoroutine(RespawnResetting());
    }

    IEnumerator RespawnResetting()
    {
        //2.5초 뒤에 Input 받기
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.KeyAction -= KeyDownAction;
        Managers.Input.UIKeyboardAction -= UIKeyDownAction;

        yield return new WaitForSeconds(2.5f);
        //부활 effect On
        transform.Find("SpawnSimplePink").gameObject.SetActive(true);

        //collider On
        _pv.RPC("RemoteRespawnEnable", RpcTarget.All, _pv.ViewID, true, 2);

        //액션 대리자 재설정
        //Attack
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.MouseAction += MouseDownAction;
        Managers.Input.KeyAction -= KeyDownAction;
        Managers.Input.KeyAction += KeyDownAction;

        //Card
        Managers.Input.UIKeyboardAction -= UIKeyDownAction;
        Managers.Input.UIKeyboardAction += UIKeyDownAction;

        //액션 대리자 재설정 후 UpdatePlayerStat
        _startDie = false;

        yield return new WaitForSeconds(0.5f);
        //부활 effect 재설정
        transform.Find("SpawnSimplePink").gameObject.SetActive(false);
    }


    //Mouse event
    public void MouseDownAction(Define.MouseEvent evt)
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
                        if (_pStats.nowHealth <= 0) return;

                        //좌표, 타겟 설정(도로 클릭 시 공격 타입 -> None 타입으로 변경)
                        TargetSetting(_mousePos.Item1, _mousePos.Item2, evt);

                        //사거리가 켜져있다면 Off
                        if (_IsRange == true)
                        {
                            KeyPushState("MouseRightButton");
                        }

                        //우클릭 평타 금지
                        BaseCard._lockTarget = null;
                        _proj = Define.Projectile.Undefine;

                        //Move
                        _state = Define.State.Moving;

                        break;


                    //마우스 오른쪽 버튼 누르고 있을 시
                    case Define.MouseEvent.Press:
                        if (_pStats.nowHealth <= 0) return;

                        //좌표, 타겟 설정(도로 클릭 시 공격 타입 -> None 타입으로 변경)
                        TargetSetting(_mousePos.Item1, _mousePos.Item2, evt);

                        //사거리가 켜져있다면 Off
                        if (_IsRange == true)
                        {
                            KeyPushState("MouseRightButton");
                        }

                        //우클릭 평타 금지
                        BaseCard._lockTarget = null;
                        _proj = Define.Projectile.Undefine;

                        //Move
                        _state = Define.State.Moving;

                        break;


                    //마우스 왼쪽 버튼 클릭 시
                    case Define.MouseEvent.LeftButton:
                        //Range Off일 때 아무일도 없음.
                        if (_IsRange == false) return;

                        //Range가 On일 때만 좌클릭 시
                        if (_IsRange == true)
                        {
                            //스킬일 때
                            if (_proj == Define.Projectile.Skill_Proj)
                            {
                                //Range 카드 = 타겟 카드
                                if (_SaveRangeNum == (int)Define.CardType.Range)
                                {
                                    TargetSetting(_mousePos.Item1, _mousePos.Item2, evt);

                                    _state = Define.State.Moving;
                                }

                                //Range 카드 = 포인트 카드
                                if (_SaveRangeNum == (int)Define.CardType.Point)
                                {
                                    //Range 좌표 = Effect 위치 
                                    _MovingPos = _attackRange[_SaveRangeNum].transform.position;

                                    //스킬 상태로 전환
                                    _state = Define.State.Skill;
                                }

                                //나머지 카드 = 논타겟 카드
                                if (_SaveRangeNum == (int)Define.CardType.Arrow || _SaveRangeNum == (int)Define.CardType.Cone ||
                                        _SaveRangeNum == (int)Define.CardType.Line || _SaveRangeNum == (int)Define.CardType.None)
                                {
                                    //Range 좌표 = Effect 위치 
                                    _MovingPos = _mousePos.Item1;

                                    //회전
                                    transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, _MovingPos) - transform.position);

                                    //스킬 상태로 전환
                                    _state = Define.State.Skill;
                                }
                            }

                            //평타일 때
                            if (_proj == Define.Projectile.Attack_Proj)
                            {
                                if (RangeAttack() == null) return;
                                if (RangeAttack() != null)
                                {
                                    //좌클릭이 적 타워, 미니언일때 _lockTarget 변경 
                                    if(_mousePos.Item2.tag == "OBJECT")
                                    {
                                        //같은편 플레이어 공격 시 return
                                        if (_mousePos.Item2.layer == _pStats.playerArea) return;
                                        LeftMouseButtontarget = _mousePos.Item2;
                                    }

                                    //좌클릭이 땅, Player라면 RangeAttack
                                    if(_mousePos.Item2.tag != "OBJECT")
                                    {
                                        //같은편 플레이어 공격 시 return
                                        if (_mousePos.Item2.layer == _pStats.playerArea) return;
                                        //적 플레이어를 공격햇다면
                                        if (_mousePos.Item2.tag == "PLAYER")
                                        {
                                            LeftMouseButtontarget = _mousePos.Item2;
                                        }
                                        //땅을 클릭햇다면
                                        if (_mousePos.Item2.tag != "PLAYER")
                                        {
                                            LeftMouseButtontarget = RangeAttack();
                                        }

                                    }

                                    int targetId = GetRemotePlayerId(LeftMouseButtontarget);
                                    //타겟 ID 찾기
                                    GameObject remoteTarget = GetRemotePlayer(targetId);

                                    //타겟 오브젝트 설정
                                    BaseCard._lockTarget = remoteTarget;

                                    //공격 상태로 전환
                                    _state = Define.State.Moving;
                                }
                            }
                        }

                        break;
                }
            }
        }
    }


    //A키 공격
    protected override GameObject RangeAttack()
    {
        float dist = 999;
        GameObject target = null;

        int layerMask = 1 << _pStats.enemyArea; //적
        layerMask |= 1 << (int)Define.ObjectType.Neutral; //중앙 오브젝트  

        Collider[] cols = Physics.OverlapSphere(transform.position, _pStats.attackRange, layerMask);

        foreach (Collider col in cols)
        {
            //플레이어 우선 감지
            if(col.gameObject.tag == "PLAYER")
            {
                return col.gameObject;
            }
            
            //Nexus, 중앙 object 감지
            if(col.gameObject.name == $"{enemyName}Nexus" || col.gameObject.name == "NeutralMob")
            {
                return col.gameObject;
            }

            //적 타워 감지
            for(int i = 1; i <= 5; i++)
            {
                if (col.gameObject.name == $"{enemyName}Tower" || col.gameObject.name == $"{enemyName}Tower_{i}")
                {
                    return col.gameObject;
                }
            }
            
            //가까운 미니언 감지
            float Distance = Vector3.Distance(col.transform.position, transform.position);
            if (Distance <= _pStats.attackRange && Distance < dist)
            {
                dist = Distance;
                target = col.gameObject;
            }
        }

        return target;
    }


    //마우스 클릭 시 좌표, 타겟 설정
    public void TargetSetting(Vector3 _mousePos, GameObject _lockTarget = null, Define.MouseEvent _evt = default)
    {
        //도로 클릭 시
        if (_lockTarget == null || _lockTarget.layer == (int)Define.Layer.Road)
        {
            //좌표 설정
            _MovingPos = _mousePos;

            //타겟 오브젝트 설정
            BaseCard._lockTarget = null;

            //마우스 오른쪽 클릭 & 누르기
            if (_evt != Define.MouseEvent.LeftButton)
            {
                _proj = Define.Projectile.Undefine;
            }
            
            return;
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

            //마우스 오른쪽 클릭 & 누르기
            if (_evt != Define.MouseEvent.LeftButton)
                _proj = Define.Projectile.Attack_Proj;
            
        }
    }


    //Card Key event
    public void UIKeyDownAction(Define.UIKeyboard _key)
    {
        //키보드 입력 시 _lockTarget 초기화 -> Card UI 변환 시간 벌어주기
        BaseCard._lockTarget = null;

        if (_pv.IsMine)
        {
            //키보드 입력 시
            switch (_key)
            {
                case Define.UIKeyboard.Q:
                    if (_pStats.UseMana(_key.ToString()).Item1 == true)
                    {
                        KeyPushState(_key.ToString());
                    }

                    break;

                case Define.UIKeyboard.W:
                    if (_pStats.UseMana(_key.ToString()).Item1 == true)
                    {
                        KeyPushState(_key.ToString());
                    }

                    break;

                case Define.UIKeyboard.E:
                    if (_pStats.UseMana(_key.ToString()).Item1 == true)
                    {
                        KeyPushState(_key.ToString());
                    }

                    break;

                case Define.UIKeyboard.R:
                    if (_pStats.UseMana(_key.ToString()).Item1 == true)
                    {
                        KeyPushState(_key.ToString());
                    }

                    break;
            }
        }

    }

    //Attack key event
    public void KeyDownAction(Define.KeyboardEvent _key)
    {
        if (_pv.IsMine)
        {
            if (_key == Define.KeyboardEvent.A)
            {
                KeyPushState(_key.ToString());
            }
        }
    }

    //키 누르면 상태 전환
    public void KeyPushState(string Keyname)
    {
        //Debug.Log($"현재 누른 Range키: {Keyname} / 이전에 눌렀던 Range키: {BaseCard._NowKey}");

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
                        _SaveRangeNum = (int)Define.CardType.None;

                        //현재 누른 키 정보를 static으로 저장
                        BaseCard._NowKey = Keyname;

                        //자기 자신
                        _MovingPos = this.transform.position;

                        //논타겟
                        BaseCard._lockTarget = null;

                        //스킬 상태
                        _state = Define.State.Skill;

                        break;
                }
            }
        }
    }


    //상시로 바뀌는 플레이어 스텟 
    protected override void UpdatePlayerStat()
    {
        //초기 세팅
        if (_SaveRegenCool == default)
        {
            _SaveRegenCool = 0.01f;
        }

        if (_SaveRegenCool != default)
        {
            _SaveRegenCool += Time.deltaTime;

            if (_SaveRegenCool >= 1)
            {
                //피 회복
                _pv.RPC("photonStatSet", RpcTarget.All, "nowHealth", _pStats.healthRegeneration);
                _pStats.nowMana += _pStats.manaRegen;

                //attackDelay 초기화
                _SaveRegenCool = default;
            }
        }
    }


    //Moving
    protected override void UpdateMoving()
    {
        //타겟 - Attack or Skill or Move
        switch (_proj)
        {
            //Attack
            case Define.Projectile.Attack_Proj:
                if (BaseCard._lockTarget == null)
                {
                    _agent.ResetPath();
                    _state = Define.State.Idle;

                    break;
                }

                if (BaseCard._lockTarget != null)
                {
                    //이동
                    transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, BaseCard._lockTarget.transform.position) - transform.position);
                    _agent.SetDestination(BaseCard._lockTarget.transform.position);

                    float distance = Vector3.Distance(BaseCard._lockTarget.transform.position, transform.position);
                    if (distance <= _pStats.attackRange || BaseCard._lockTarget.name == $"{enemyName}Nexus" || BaseCard._lockTarget.name == "NeutralMob")
                    {
                        _state = Define.State.Attack;

                        return;
                    }

                    if (distance > _pStats.attackRange)
                    {
                        _state = Define.State.Moving;
                        _proj = Define.Projectile.Attack_Proj;
                    }
                }

                break;
;

            //Skill
            case Define.Projectile.Skill_Proj:
                //논타겟 카드일 때
                if (BaseCard._lockTarget == null)
                {
                    _agent.ResetPath();
                    _state = Define.State.Idle;
                }

                //타겟 카드일 때
                if (BaseCard._lockTarget != null)
                {
                    float targetDis = Vector3.Distance(BaseCard._lockTarget.transform.position, transform.position);
                    if (targetDis <= _cardStats._rangeScale)
                    {
                        _state = Define.State.Skill;

                        return;
                    }
                }

                break;

            //Move
            case Define.Projectile.Undefine:
                //이동
                // transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, _MovingPos) - transform.position);
                var pos = Managers.Input.FlattenVector(this.gameObject, _agent.steeringTarget) - transform.position;
                if (pos != Vector3.zero)
                {
                    var targetRotation = Quaternion.LookRotation(pos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7.5f * Time.deltaTime);
                }
                _agent.SetDestination(_MovingPos);
                //Idle
                if (Vector3.Distance(transform.position, _MovingPos) < 0.2f)
                {
                    _state = Define.State.Idle;
                }

                break;
        }
    }

    //Attack 초기화
    protected override void StartAttack()
    {
        //Input 재설정
        //마우스 오른쪽
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.MouseAction += MouseDownAction;
        //Card
        Managers.Input.UIKeyboardAction -= UIKeyDownAction;
        Managers.Input.UIKeyboardAction += UIKeyDownAction;
    }

    protected override void StartUIAttack() 
    {
        //적이 죽었을 때
        if (BaseCard._lockTarget == null)
        {
            _proj = Define.Projectile.Undefine;
        }

        //적이 안죽었다면
        if (BaseCard._lockTarget != null)
        {
            _proj = Define.Projectile.Attack_Proj;
        }

        //애니메이션 Idle로 변환
        _state = Define.State.Idle;

        ///Attack 초기화
        oneShot = false;
        _stopAttack = false;

        //attack
        Managers.Input.KeyAction -= KeyDownAction;
        Managers.Input.KeyAction += KeyDownAction;
    }


    //평타 후 딜레이
    protected override void StopAttack()
    {
        //// 평타 딜레이 중
        _stopAttack = true;

        //움직임 초기화
        _agent.ResetPath();

        //평타 중 Key Input 안받기 
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.KeyAction -= KeyDownAction;
        //스킬 잠깐 못쓰기
        Managers.Input.UIKeyboardAction -= UIKeyDownAction;

        if (_pv.IsMine)
        {
            ////Range Off
            _IsRange = false;
            _attackRange[4].SetActive(_IsRange);
        }
    }

    //Skill 초기화
    protected override void StartSkill()
    {
        _IsRange = false;
        _attackRange[_SaveRangeNum].SetActive(_IsRange);

        //애니메이션 Idle로 변환
        _state = Define.State.Idle;

        //마우스 좌표, 타겟 초기화
        _MovingPos = default;
        BaseCard._lockTarget = null;

        //Attack 재설정
        //Managers.Input.KeyAction -= KeyDownAction;
        //Managers.Input.KeyAction += KeyDownAction;
        //Managers.Input.MouseAction -= MouseDownAction;
        //Managers.Input.MouseAction += MouseDownAction;
    }


    //스킬 사용 후 딜레이
    protected override IEnumerator StopSkill()
    {
        //// 스킬 딜레이 중
        _stopSkill = true;

        //움직임 초기화
        _agent.ResetPath();

        //평타 중 Key Input 안받기 
        //Managers.Input.KeyAction -= KeyDownAction;
        //Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.UIKeyboardAction -= UIKeyDownAction;

        UpdateSkill();

        yield return new WaitForSeconds(2.0f);
        _stopSkill = false;

        //card 재설정
        Managers.Input.UIKeyboardAction -= UIKeyDownAction;
        Managers.Input.UIKeyboardAction += UIKeyDownAction;
    }

    //Skill
    protected override void UpdateSkill()
    {
        //이펙트 발동
        if (_MovingPos != default)
        {
            //Debug.Log($"UpdateSkill : {_MovingPos} ");
            //Skill On
            _cardStats.InitCard();
            GameObject effectObj = _cardStats.cardEffect(_MovingPos, this._pv.ViewID, _pStats.playerArea);
            //Debug.Log(effectObj);
            //_pv.RPC("RemoteSkillStarter", RpcTarget.All, this.GetComponent<PhotonView>().ViewID, effectObj.GetComponent<PhotonView>().ViewID);

            //이펙트가 특정 시간 후에 사라진다면
            if (_cardStats._effectTime != default)
            {
                //Destroy(effectObj, _cardStats._effectTime);
                StartCoroutine(DelayDestroy(effectObj, _cardStats._effectTime));
                Debug.Log("Delete EffectPaticle");
            }
        }

        return;
    }


    //Die
    protected override void UpdateDie()
    {
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.KeyAction -= KeyDownAction;
        Managers.Input.UIKeyboardAction -= UIKeyDownAction;
        Managers.game.DieEvent(_pv.ViewID);

        //죽었을 때 재설정
        BaseCard._lockTarget = null;
        _MovingPos = default;
        _stopAttack = false;
        _stopSkill = false;
        oneShot = false;
        _attackRange[_SaveRangeNum].SetActive(false);
        _startDie = true;
    }
}
