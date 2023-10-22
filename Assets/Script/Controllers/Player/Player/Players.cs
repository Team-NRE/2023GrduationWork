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
    public bool _IsRange = false;

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

    protected GameObject attackSound;
    GameObject resen;

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

        resen = transform.Find("SpawnSimplePink").gameObject;
        attackSound = transform.Find("AttackSound").gameObject;


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
        resen.SetActive(false);
    }

    public override void InitOnEnable()
    {
        //enemyName
        enemyName = (_pStats.playerArea == 6) ? "Cyborg" : "Human";

        //0으로 초기화
        _pStats.nowHealth = 0;

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
        resen.SetActive(true);
        resen.GetComponent<AudioSource>().enabled = true;

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

        yield return new WaitForSeconds(2.5f);
        //부활 effect 재설정
        resen.SetActive(false);
        resen.GetComponent<AudioSource>().enabled = false;
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
                        KeyPushState();
                        RightButtonTargetSetting(_mousePos.Item1);

                        break;


                    //마우스 오른쪽 버튼 누르고 있을 시
                    case Define.MouseEvent.Press:
                        KeyPushState();
                        RightButtonTargetSetting(_mousePos.Item1);

                        break;


                    //마우스 왼쪽 버튼 클릭 시
                    case Define.MouseEvent.LeftButton:
                        //Range Off일 때 아무일도 없음.
                        if (_IsRange == false) return;

                        //스킬일 때
                        if (_proj == Define.Projectile.Skill_Proj)
                        {
                            LeftButton_SkillTargetSetting(_SaveRangeNum, _mousePos.Item1, _mousePos.Item2);
                        }

                        //평타일 때
                        if (_proj == Define.Projectile.Attack_Proj)
                        {
                            LeftButton_AttackTargetSetting(_mousePos.Item2);
                        }

                        break;
                }
            }
        }
    }


    //우클릭 타겟, 좌표 설정
    public void RightButtonTargetSetting(Vector3 _mousePos)
    {
        //좌표 설정
        _MovingPos = _mousePos;
        BaseCard._lockTarget = null;

        //enum 설정
        _proj = Define.Projectile.Undefine;
        _state = Define.State.Moving;
    }

    //좌클릭 스킬 타겟, 좌표 설정
    public void LeftButton_SkillTargetSetting(int _SaveRangeNum, Vector3 _mousePos, GameObject _lockTarget)
    {
        switch (_SaveRangeNum) 
        {
            //None 카드 즉시 break;
            case (int)Define.CardType.None: break;

            //Range 카드 (타겟팅 카드)
            case (int)Define.CardType.Range:
                //좌표 설정
                _MovingPos = _mousePos;

                //타겟 오브젝트 설정
                if (_lockTarget.layer == (int)Define.Layer.Road) { BaseCard._lockTarget = null; }
                if (_lockTarget.layer == _pStats.enemyArea || _lockTarget.layer == (int)Define.Layer.Neutral) 
                {
                    //타겟 ID 찾기 및 타겟 지정
                    int targetId = GetRemotePlayerId(_lockTarget);
                    GameObject remoteTarget = GetRemotePlayer(targetId);
                    BaseCard._lockTarget = remoteTarget;
                }

                //상태 변환
                _state = Define.State.Moving;

                break;

            //Point 카드 (논타겟팅 카드)
            case (int)Define.CardType.Point:
                //좌표 설정
                _MovingPos = _attackRange[_SaveRangeNum].transform.position;

                //타겟 오브젝트 설정
                BaseCard._lockTarget = null;

                //회전
                transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, _MovingPos) - transform.position);

                //상태 변환
                _state = Define.State.Skill;

                break;

            //나머지 카드 (논타겟팅 카드)
            default:
                //좌표 설정 
                _MovingPos = _mousePos;

                //타겟 오브젝트 설정
                BaseCard._lockTarget = null;

                //회전
                transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, _MovingPos) - transform.position);

                //상태 변환
                _state = Define.State.Skill;

                break;
        }
    }

    //좌클릭 평타 타겟, 좌표 설정
    public void LeftButton_AttackTargetSetting(GameObject _lockTarget, GameObject target = null)
    {
        ////좌클릭 : 우리 팀 오브젝트 
        if (_lockTarget.layer == _pStats.playerArea) { return; }

        //좌클릭 : 적 오브젝트 / 땅
        if (_lockTarget.CompareTag("OBJECT") || _lockTarget.CompareTag("PLAYER")) { target = _lockTarget; }
        if (_lockTarget.layer == (int)Define.Layer.Road) { target = RangeAttack(); }

        //타겟 null 이면 return
        if (target == null) 
        { 
            _state = Define.State.Idle;
            _IsRange = false;
            _attackRange[_SaveRangeNum].SetActive(_IsRange);
            _SaveRangeNum = (int)Define.CardType.None;

            return; 
        }

        //타겟 ID 찾기 및 타겟 지정
        int targetId = GetRemotePlayerId(target);
        GameObject remoteTarget = GetRemotePlayer(targetId);
        BaseCard._lockTarget = remoteTarget;
        _MovingPos = default;

        //공격 상태로 전환
        _state = Define.State.Moving;
    }

    //A키 공격
    protected override GameObject RangeAttack()
    {
        float dist = 9999;
        GameObject target = null;

        int layerMask = 1 << _pStats.enemyArea; //적
        layerMask |= 1 << (int)Define.ObjectType.Neutral; //중앙 오브젝트  

        //탐지 거리 늘림
        Collider[] cols = Physics.OverlapSphere(transform.position, (_pStats.attackRange + 1.5f), layerMask);
        foreach (Collider col in cols)
        {
            //플레이어 우선 감지
            if(col.gameObject.CompareTag("PLAYER"))
            {
                return col.gameObject;
            }
            
            //Nexus, 중앙 object 감지
            if(col.gameObject.name == $"{enemyName}Nexus" || col.gameObject.name == "NeutralMob")
            {
                return col.gameObject;
            }

            //가까운 미니언 감지
            float Distance = Vector3.Distance(col.transform.position, transform.position);
            if (Distance < dist)
            {
                dist = Distance;
                target = col.gameObject;
            }

            ////적 타워 감지
            //for(int i = 1; i <= 5; i++)
            //{
            //    if (col.gameObject.name == $"{enemyName}Tower" || col.gameObject.name == $"{enemyName}Tower_{i}")
            //    {
            //        return col.gameObject;
            //    }
            //}

        }

        return target;
    }


    //Card Key event
    public void UIKeyDownAction(Define.UIKeyboard _key)
    {
        if (_pv.IsMine)
        {
            //키보드 입력 시
            switch (_key)
            {
                case Define.UIKeyboard.Q:
                    if (_pStats.UseMana(_key.ToString()).Item1 == true)
                    {
                        _cardStats = FindObjectOfType<UI_CardPanel>().Q_UI;
                        KeyPushState(_key.ToString());
                    }

                    break;

                case Define.UIKeyboard.W:
                    if (_pStats.UseMana(_key.ToString()).Item1 == true)
                    {
                        _cardStats = FindObjectOfType<UI_CardPanel>().W_UI;
                        KeyPushState(_key.ToString());
                    }

                    break;

                case Define.UIKeyboard.E:
                    if (_pStats.UseMana(_key.ToString()).Item1 == true)
                    {
                        _cardStats = FindObjectOfType<UI_CardPanel>().E_UI;
                        KeyPushState(_key.ToString());
                    }

                    break;

                case Define.UIKeyboard.R:
                    if (_pStats.UseMana(_key.ToString()).Item1 == true)
                    {
                        _cardStats = FindObjectOfType<UI_CardPanel>().R_UI;
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
    public void KeyPushState(string nowKey = null)
    {
        // nowkey 없거나 우클릭 시 return
        if (nowKey == null)
        {
            _IsRange = false;
            _attackRange[_SaveRangeNum].SetActive(_IsRange);
            _SaveRangeNum = (int)Define.CardType.None;
            
            return;
        }

        //On/Off
        _IsRange = (_IsRange == true ? false : true);

        if (_IsRange == false) 
        {
            //이전 키 사거리를 Off
            _attackRange[_SaveRangeNum].SetActive(_IsRange);
            _SaveRangeNum = (int)Define.CardType.None;
            
            //현재 키 != 이전 키
            if(nowKey != BaseCard._NowKey) { _IsRange = true; }

            BaseCard._lockTarget = null;
            BaseCard._NowKey = null; 
        }
        if (_IsRange == true)
        {
            //현재 카드 세팅
            CardSettingChange(nowKey);

            //Range On
            _attackRange[_SaveRangeNum].SetActive(_IsRange);
        }

    }

    //카드 Range 세팅 변환
    public void CardSettingChange(string nowKey)
    {
        //평타 공격 시
        if (nowKey == Define.KeyboardEvent.A.ToString())
        {
            //현재 누른 키의 정보 저장
            BaseCard._NowKey = nowKey;

            //스킬 타입
            _proj = Define.Projectile.Attack_Proj;

            //attackRange 설정 - 범위 크기
            _SaveRangeNum = (int)Define.CardType.Range;
            _attackRange[_SaveRangeNum].GetComponent<Projector>().orthographicSize = _pStats.attackRange;

            return;
        }

        //카드의 Range 타입에 따른 세팅 변환
        switch (_cardStats._rangeType)
        {
            case Define.CardType.Arrow:
                //현재 누른 키의 정보 저장
                BaseCard._NowKey = nowKey;

                //스킬 타입
                _proj = Define.Projectile.Skill_Proj;

                //attackRange 설정 - Arrow는 Scale 값 고정
                _SaveRangeNum = (int)Define.CardType.Arrow;
                _attackRange[_SaveRangeNum].GetComponent<AngleMissile>().Scale = 10.0f;

                break;


            case Define.CardType.Cone:
                //현재 누른 키의 정보 저장
                BaseCard._NowKey = nowKey;

                //스킬 타입
                _proj = Define.Projectile.Skill_Proj;

                //attackRange 설정 - 범위 크기 / 각도
                _SaveRangeNum = (int)Define.CardType.Cone;
                _attackRange[_SaveRangeNum].GetComponent<Cone>().Scale = 2 * _cardStats._rangeScale;
                _attackRange[_SaveRangeNum].GetComponent<Cone>().Angle = _cardStats._rangeAngle;

                break;


            case Define.CardType.Line:
                //현재 누른 키의 정보 저장
                BaseCard._NowKey = nowKey;

                //스킬 타입
                _proj = Define.Projectile.Skill_Proj;

                //attackRange 설정 - 범위 크기
                _SaveRangeNum = (int)Define.CardType.Line;
                _attackRange[_SaveRangeNum].GetComponent<AngleMissile>().Scale = 2 * _cardStats._rangeScale;

                break;


            case Define.CardType.Point:
                //현재 누른 키의 정보 저장
                BaseCard._NowKey = nowKey;

                //스킬 타입
                _proj = Define.Projectile.Skill_Proj;

                //attackRange 설정 - 범위 크기 / Point 크기
                _SaveRangeNum = (int)Define.CardType.Point;
                _attackRange[_SaveRangeNum].GetComponent<Point>().Scale = 2 * _cardStats._rangeScale;
                _attackRange[_SaveRangeNum].GetComponent<Point>().Range = _cardStats._rangeRange;

                break;


            case Define.CardType.Range:
                //현재 누른 키의 정보 저장
                BaseCard._NowKey = nowKey;

                //스킬 타입
                _proj = Define.Projectile.Skill_Proj;

                //attackRange 설정 - 범위 크기
                _SaveRangeNum = (int)Define.CardType.Range;
                _attackRange[_SaveRangeNum].GetComponent<Projector>().orthographicSize = _cardStats._rangeScale;
                
                break;


            case Define.CardType.None:
                //effect 발동 위치, 타겟 정보, 현재 키 정보 저장
                _MovingPos = this.transform.position;
                BaseCard._lockTarget = null;
                BaseCard._NowKey = null;

                //Range 정보 저장
                _SaveRangeNum = (int)Define.CardType.None;
                
                //플레이어 상태 즉시 변환
                _state = Define.State.Skill;

                break;
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

        //1초에 HealthRegen만큼 회복
        if (_SaveRegenCool != default)
        {
            _SaveRegenCool += Time.deltaTime;

            if (_SaveRegenCool >= 1)
            {
                //피 회복
                _pv.RPC("photonStatSet", RpcTarget.All, "nowHealth", _pStats.healthRegeneration);
                _pStats.nowMana += _pStats.manaRegen;

                //Regen 초기화
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
                //자동 공격 중 타겟 사망 시
                if (BaseCard._lockTarget == null)
                {
                    _agent.ResetPath();
                    _state = Define.State.Idle;

                    break;
                }
                //타겟 On
                if (BaseCard._lockTarget != null)
                {
                    //타겟을 향해 회전
                    transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, BaseCard._lockTarget.transform.position) - transform.position);
                    
                    //타겟을 향해 경로 설정
                    _agent.SetDestination(BaseCard._lockTarget.transform.position);

                    //타겟과의 거리 <= 평타 거리
                    float distance = Vector3.Distance(BaseCard._lockTarget.transform.position, transform.position);
                    //타겟이 Nexus, 중립 몹이면 거리와 상관없이 공격
                    if (distance <= _pStats.attackRange || BaseCard._lockTarget.name == $"{enemyName}Nexus" || BaseCard._lockTarget.name == "NeutralMob")
                    {
                        _state = Define.State.Attack;

                        return;
                    }
                    //타겟과의 거리 > 평타 거리
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
                //타겟 사망 시
                if (BaseCard._lockTarget == null)
                {
                    _agent.ResetPath();
                    _state = Define.State.Idle;
                }

                //Range 카드 시
                if (BaseCard._lockTarget != null)
                {
                    //타겟을 향해 회전
                    transform.rotation = Quaternion.LookRotation(Managers.Input.FlattenVector(this.gameObject, BaseCard._lockTarget.transform.position) - transform.position);

                    //타겟을 향해 경로 설정
                    _agent.SetDestination(BaseCard._lockTarget.transform.position);

                    //타겟과의 거리 <= 카드와의 거리
                    float distance = Vector3.Distance(BaseCard._lockTarget.transform.position, transform.position);
                    if (distance <= _cardStats._rangeScale)
                    {
                        _state = Define.State.Skill;

                        return;
                    }
                    //타겟과의 거리 > 카드와의 거리
                    if (distance > _cardStats._rangeScale)
                    {
                        _state = Define.State.Moving;
                        _proj = Define.Projectile.Skill_Proj;
                    }
                }

                break;

            //Move
            case Define.Projectile.Undefine:
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


    /// <summary>
    /// Attack - StopAttack -> UpdateAttack -> StartAttack -> StartUIAttack 순으로 진행
    /// </summary>
    protected override void StopAttack()
    {
        //// 평타 딜레이 중
        _stopAttack = true;

        //움직임 초기화
        _agent.ResetPath();

        //평타 중 공격, 마우스 키 못받기 
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.KeyAction -= KeyDownAction;

        if (_pv.IsMine)
        {
            ////Range Off
            _IsRange = false;
            _attackRange[4].SetActive(_IsRange);
        }
    }
    protected override void StartAttack()
    {
        //Input 재설정
        //마우스 오른쪽
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.MouseAction += MouseDownAction;
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

        //평타 소리
        attackSound.GetComponent<AudioSource>().enabled = false;

        ///bool 초기화
        oneShot = false;
        _stopAttack = false;

        //attack 재설정
        Managers.Input.KeyAction -= KeyDownAction;
        Managers.Input.KeyAction += KeyDownAction;
    }


    /// <summary>
    /// Skill - StopSkill -> UpdateSkill -> StartSkill -> StopSkill 순으로 진행
    /// </summary>
    protected override IEnumerator StopSkill()
    {
        // 스킬 딜레이 중
        _stopSkill = true;

        //움직임 초기화
        _agent.ResetPath();

        //Card
        Managers.Input.UIKeyboardAction -= UIKeyDownAction;

        //Skill
        UpdateSkill();

        yield return new WaitForSeconds(2.0f);
        _stopSkill = false;

        //Attack 재설정
        Managers.Input.KeyAction -= KeyDownAction;
        Managers.Input.KeyAction += KeyDownAction;
        //마우스 재설정
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.MouseAction += MouseDownAction;

        //Card
        Managers.Input.UIKeyboardAction -= UIKeyDownAction;
        Managers.Input.UIKeyboardAction += UIKeyDownAction;
    }
    protected override void UpdateSkill()
    {
        if (_MovingPos == default) return;
        if (_MovingPos != default)
        {
            //스킬 발동
            _cardStats.InitCard();
            GameObject effectObj = _cardStats.cardEffect(_MovingPos, this._pv.ViewID, _pStats.playerArea);

            //이펙트가 특정 시간 후에 사라진다면
            if (_cardStats._effectTime != default)
            {
                StartCoroutine(DelayDestroy(effectObj, _cardStats._effectTime));
            }
        }
    }
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
        Managers.Input.KeyAction -= KeyDownAction;
        Managers.Input.KeyAction += KeyDownAction;
        //마우스 재설정
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.MouseAction += MouseDownAction;
    }


   
    //Die
    protected override void UpdateDie()
    {
        //Manager
        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.KeyAction -= KeyDownAction;
        Managers.Input.UIKeyboardAction -= UIKeyDownAction;
        Managers.game.DieEvent(_pv.ViewID);

        //죽었을 때 재설정
        BaseCard._lockTarget = null;
        _MovingPos = default;

        //attack
        _stopAttack = false;
        oneShot = false;

        //skill
        _stopSkill = false;

        //Range
        _IsRange = false;
        _attackRange[_SaveRangeNum].SetActive(_IsRange);
        _SaveRangeNum = (int)Define.CardType.None;

        //die
        _startDie = true;
    }
}
