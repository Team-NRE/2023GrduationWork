using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using Stat;
using Define;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public abstract class BaseController : MonoBehaviourPun, IPunObservable
{
    
    protected Vector3 _MovingPos;

    /// <summary>
    /// IPunObservable
    /// </summary>
    protected Vector3 receivePos;
    protected Quaternion receiveRot;
    protected float damping = 10.0f;

    /// <summary>
    /// 초기화
    /// </summary>
    protected PhotonView _pv;
    protected Animator _anim;
    protected NavMeshAgent _agent;

    //총알 발사 여부
    public bool _stopAttack = false;
    //스킬 발동 여부
    public bool _stopSkill = false;
    
    /// <summary>
    /// 즉음 유무
    /// </summary>
    public bool _startDie = false;

    /// <summary>
    /// Manager 참조
    /// </summary>
    protected RespawnManager respawnManager;

    /// <summary>
    /// 플레이어 스텟
    /// </summary>
    public PlayerStats _pStats { get; set; }

    /// <summary>타일 그리드</summary>
    protected GridLayout _grid;
    /// <summary>타일 맵</summary>
    protected Tilemap _tilemap;
    /// <summary>현재 서 있는 지역</summary>
    public ObjectPosArea _area;


    /// <summary>
    /// 외부 namespace Define 참조
    /// </summary>
    public CardType _cdType { get; protected set; }
    public PlayerType _pType { get; protected set; }
    public State _state { get; set; } = State.Idle;
    public CameraMode _cameraMode { get; protected set; } = CameraMode.QuaterView;
    public Projectile _proj { get; protected set; } = Projectile.Undefine;

    public void Awake() 
    {
        // 팀 분배
        Init();
    }

    public void OnEnable()
    {
        InitOnEnable();
    }


    public virtual void Update()
    {
        if (_pv.IsMine)
        {
            UpdatePlayer_StateChange();
            UpdatePlayer_AnimationChange();
        }

        GetTransformArea();
    }


    //abstract = 하위 클래스에서 꼭 선언해야함.
    public virtual void Init() { }
    public virtual void InitOnEnable() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMoving() { if (!PhotonNetwork.IsMasterClient) return; }
    protected virtual void UpdateAttack() { }
    protected virtual void UpdateSkill() { }
    protected virtual void UpdateDie() { }
    protected virtual void UpdatePlayerStat() { }

    protected virtual GameObject RangeAttack() { return null; }

    protected virtual void StopAttack() { }
    protected virtual IEnumerator StopSkill() { yield return null; }

    protected virtual void StartAttack() { }
    protected virtual void StartUIAttack() { }
    protected virtual void StartSkill() { }

    protected void UpdatePlayer_AnimationChange() 
    {
        //키, 마우스 이벤트 받으면 state가 변환
        switch (_state)
        {
            case Define.State.Idle:
                _anim.SetBool("IsIdle", true);

                _anim.SetBool("IsMoving", false);
                _anim.SetBool("IsAttack", false);
                _anim.SetBool("IsSkill", false);
                _anim.SetBool("IsDie", false);

                UpdateIdle();

                break;

            case Define.State.Die:
                _anim.SetBool("IsDie", true);

                _anim.SetBool("IsIdle", false);
                _anim.SetBool("IsMoving", false);
                _anim.SetBool("IsAttack", false);
                _anim.SetBool("IsSkill", false);

                UpdateDie();

                break;

            case Define.State.Moving:
                _anim.SetBool("IsMoving", true);

                _anim.SetBool("IsIdle", false);
                _anim.SetBool("IsAttack", false);
                _anim.SetBool("IsSkill", false);

                UpdateMoving();

                break;

            case Define.State.Attack:
                _anim.SetBool("IsAttack", true);

                _anim.SetBool("IsIdle", false);
                _anim.SetBool("IsMoving", false);
                _anim.SetBool("IsSkill", false);
                _anim.SetFloat("AttackSpeed", _pStats.attackSpeed);

                if (_stopAttack == false)
                {
                    StopAttack();
                }

                break;

            case Define.State.Skill:
                _anim.SetBool("IsSkill", true);

                _anim.SetBool("IsIdle", false);
                _anim.SetBool("IsMoving", false);
                _anim.SetBool("IsAttack", false);

                if (_stopSkill == false)
                {
                    StartCoroutine(StopSkill());
                }

                break;
        }
    }


    protected void UpdatePlayer_StateChange()
    {
        //Die
        if (_pStats.nowHealth <= 0)
        {
            _state = Define.State.Die;

            return;
        }

        //자동 공격
        if (BaseCard._lockTarget != null && _proj == Define.Projectile.Attack_Proj && _state == Define.State.Idle)
        {
            _state = Define.State.Moving;
          
            return;
        }

        if (_startDie == false) { UpdatePlayerStat(); }
    }

    //퍼센트 계산
    protected float PercentageCount(double percent, double value, int decimalplaces)
    {

        return (float)System.Math.Round(percent / 100 * value, decimalplaces);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 자신의 로컬 캐릭터인 경우 자신의 데이터를 다른 네트워크 유저에게 송신
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }

    protected int GetRemotePlayerId(GameObject target)
	{
        int remoteId = target.GetComponent<PhotonView>().ViewID;
        return remoteId;
	}

    protected GameObject GetRemotePlayer(int remoteId)
	{
        GameObject target = PhotonView.Find(remoteId)?.gameObject;
        return target;
	}

    protected Vector3 GetRemoteVector(int remoteId)
	{
        Vector3 targetVector = GetRemotePlayer(remoteId).transform.position;
        return targetVector;
	}
    protected IEnumerator DelayDestroy(GameObject target, float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log(time);
        if (target == null)
            yield return null;
        PhotonNetwork.Destroy(target);
        Debug.Log("Destroy");
    }
    
    [PunRPC]
    public void RemoteRespawnEnable(int viewId, bool state, int enableTime)
    {
        if(enableTime == 1)
        {
            GetRemotePlayer(viewId).GetComponent<Players>().enabled = state;
            GetRemotePlayer(viewId).GetComponent<PlayerStats>().enabled = state;
        }
        if(enableTime == 2)
        {
            GetRemotePlayer(viewId).GetComponent<Collider>().enabled = state;
        }
    }

    [PunRPC]
    protected void RemoteSkillStarter(int playerId, int particleId)
    {
        GameObject childObject = GetRemotePlayer(particleId);
        GameObject parentObject = GetRemotePlayer(playerId);
        childObject.transform.parent = parentObject.transform;
    }

    private void GetTransformArea()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        _area = Managers.game.GetPosAreaInMap(transform.position);
    }
}
