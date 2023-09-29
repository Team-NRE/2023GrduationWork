using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.StatusIndicators.Components;
using UnityEngine.AI;
using Stat;
using Photon.Pun;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.SocialPlatforms;

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
    /// 평타/스킬/스텟 쿨타임
    /// </summary>
    public float _SaveAttackSpeed = default;
    public float _SaveSkillCool = default;
    public float _SaveRegenCool = default;

    public override void Init()
    {
        base.Init();

        //초기화
        _pStats = GetComponent<PlayerStats>();
        _anim = GetComponent<Animator>();
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _pv = GetComponent<PhotonView>();

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
        ObjectController._allObjectTransforms.Add(transform);


        //마우스 이벤트 시 무시할 레이어
        ignore = LayerMask.GetMask("Default", "Ignore Raycast");

        //부활 effect setting
        transform.Find("SpawnSimplePink").gameObject.SetActive(false);
    }

    public override void InitOnEnable()
    {
        //부활권 유무
        if (_pStats.isResurrection == true)
        {
            _pStats.isResurrection = false;
            GameObject Resurrection = transform.Find("Effect_Resurrection(Clone)").gameObject;
            PhotonNetwork.Destroy(Resurrection);
            //체력 회복
            _pv.RPC("photonStatSet", RpcTarget.All, "nowHealth", (float)PercentageCount(70, _pStats.maxHealth, 1));
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
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.KeyAction -= KeyDownAction;

        yield return new WaitForSeconds(2.5f);
        //부활 effect On
        transform.Find("SpawnSimplePink").gameObject.SetActive(true);

        //collider On
        _pv.RPC("RemoteRespawnEnable", RpcTarget.All, _pv.ViewID, true, 2);

        //액션 대리자 재설정
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.MouseAction += MouseDownAction;
        Managers.Input.KeyAction -= KeyDownAction;
        Managers.Input.KeyAction += KeyDownAction;

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
                                    //타겟 ID 찾기
                                    int targetId = GetRemotePlayerId(RangeAttack());
                                    GameObject remoteTarget = GetRemotePlayer(targetId);

                                    //좌표 설정
                                    _MovingPos = _mousePos.Item1;

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
            {
                _proj = Define.Projectile.Attack_Proj;
            }
        }
    }


    //Key event
    public void KeyDownAction(Define.KeyboardEvent _key)
    {
        //키보드 입력 시 _lockTarget 초기화 -> Card UI 변환 시간 벌어주기
        BaseCard._lockTarget = null;

        if (_pv.IsMine)
        {
            //키보드 입력 시
            switch (_key)
            {
                case Define.KeyboardEvent.Q:
                    if (_pStats.UseMana(_key.ToString()).Item1 == true)
                    {
                        KeyPushState(_key.ToString());
                    }

                    break;

                case Define.KeyboardEvent.W:
                    if (_pStats.UseMana(_key.ToString()).Item1 == true)
                    {
                        KeyPushState(_key.ToString());
                    }

                    break;

                case Define.KeyboardEvent.E:
                    if (_pStats.UseMana(_key.ToString()).Item1 == true)
                    {
                        KeyPushState(_key.ToString());
                    }

                    break;

                case Define.KeyboardEvent.R:
                    if (_pStats.UseMana(_key.ToString()).Item1 == true)
                    {
                        KeyPushState(_key.ToString());
                    }

                    break;

                case Define.KeyboardEvent.A:
                    KeyPushState(_key.ToString());

                    break;
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


    //A키 공격
    protected override GameObject RangeAttack()
    {
        float dist = 999;
        GameObject target = null;

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

                    if (_agent.remainingDistance <= _pStats.attackRange)
                    {
                        _state = Define.State.Attack;

                        return;
                    }

                    else
                    {
                        _state = Define.State.Moving;
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
                if (_agent.remainingDistance < 0.2f)
                {
                    _state = Define.State.Idle;
                }

                break;
                _agent.SetDestination(_MovingPos);
                //Idle
                if (_agent.remainingDistance < 0.2f)
                {
                    _state = Define.State.Idle;
                }

                break;
        }
    }

    //Skill
    protected override void UpdateSkill()
    {
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
                    //Debug.Log($"UpdateSkill : {_MovingPos} ");
                    //Skill On
                    _cardStats.InitCard();
                    GameObject effectObj = _cardStats.cardEffect(_MovingPos, this._pv.ViewID, _pStats.playerArea);
                    //_pv.RPC("RemoteSkillStarter", RpcTarget.All, this.GetComponent<PhotonView>().ViewID, effectObj.GetComponent<PhotonView>().ViewID);

                    //이펙트가 특정 시간 후에 사라진다면
                    if (_cardStats._effectTime != default)
                    {
                        //Destroy(effectObj, _cardStats._effectTime);
                        StartCoroutine(DelayDestroy(effectObj, _cardStats._effectTime));
                        Debug.Log("Delete EffectPaticle");
                    }
                }

                _agent.ResetPath();

                //스킬 쿨타임
                _stopSkill = true;

                _state = Define.State.Idle;

                return;
            }
        }

        if (_pStats.nowHealth <= 0)
        {
            _state = Define.State.Die;
        }
    }

    //Die
    protected override void UpdateDie()
    {
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.KeyAction -= KeyDownAction;
        Managers.game.DieEvent(_pv.ViewID);

        _attackRange[_SaveRangeNum].SetActive(false);
        _startDie = true;
    }


    //평타 후 딜레이
    protected override IEnumerator StopAttack()
    {
        //// 평타 딜레이 중
        _stopAttack = true;

        //움직임 초기화
        _agent.ResetPath();

        //평타 중 Key Input 안받기 
        Managers.Input.KeyAction -= KeyDownAction;
        Managers.Input.MouseAction -= MouseDownAction;


        ////attack animation에서 특정 동작에서  데미지 작용
        yield return new WaitForSeconds((float)PercentageCount(_pStats.attackAnimPercent, _pStats.attackDelay, 2));
        UpdateAttack();
        //평타 중 Key Input 안받기 
        Managers.Input.KeyAction -= KeyDownAction;
        Managers.Input.MouseAction -= MouseDownAction;


        ////attackDelay가 다 지나간 후
        yield return new WaitForSeconds((float)PercentageCount((100 - _pStats.attackAnimPercent), _pStats.attackDelay, 2));
        //애니메이션 Idle로 변환
        _state = Define.State.Idle;

        //마우스 좌표, 타겟 초기화, StopAttack() update문 stop
        _MovingPos = default;
        BaseCard._lockTarget = null;
        _stopAttack = false;

        //Input 재설정
        Managers.Input.KeyAction -= KeyDownAction;
        Managers.Input.KeyAction += KeyDownAction;
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.MouseAction += MouseDownAction;
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

}
