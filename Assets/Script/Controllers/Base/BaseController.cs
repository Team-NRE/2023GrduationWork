using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
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
    protected bool _stopAttack = false;
    //스킬 발동 여부
    protected bool _stopSkill = false;
    //사거리 유무
    protected bool _IsRange = false;
    
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


    /// <summary>
    /// 외부 namespace Define 참조
    /// </summary>
    public CardType _cdType { get; protected set; }
    public PlayerType _pType { get; protected set; }
    public State _state { get; protected set; } = State.Idle;
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


    public void Update()
    {
        if (_pv.IsMine)
        {
            UpdatePlayer_StateChange();
            UpdatePlayer_AnimationChange();
        }    
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

    protected virtual IEnumerator StopAttack() { yield return null; }
    protected virtual IEnumerator StopSkill() { yield return null; }


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
                _anim.SetFloat("AttackSpeed", _pStats.attackSpeed);
                _anim.SetBool("IsAttack", true);

                _anim.SetBool("IsIdle", false);
                _anim.SetBool("IsMoving", false);
                _anim.SetBool("IsSkill", false);

                if (_stopAttack == false)
                {
                    StartCoroutine(StopAttack());
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

        if (_startDie == false)
        {
            UpdatePlayerStat();
        }

        if (_stopSkill == true)
        {
            StopSkill();

            return;
        }

        //A키를 눌렀을 때 
        if (_IsRange == true && BaseCard._NowKey == KeyboardEvent.A.ToString())
        {
            RangeAttack();
        }
    }

    //퍼센트 계산
    protected double PercentageCount(double percent, double value, int decimalplaces)
    {

        return System.Math.Round(percent / 100 * value, decimalplaces);
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
}
