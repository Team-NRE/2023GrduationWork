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
    protected PhotonView _pv;
    protected Vector3 receivePos;
    protected Quaternion receiveRot;
    protected float damping = 10.0f;

    private GameObject _player;

    //SerializeField = private 변수를 인스펙터에서 설정
    //protected = 상속 관계에 있는 클래스 내부에서만 접근
    protected Animator _anim;
    protected NavMeshAgent _agent;
    protected Vector3 _MovingPos;

    //총알 발사 여부
    protected bool _stopAttack = false;
    //스킬 발동 여부
    protected bool _stopSkill = false;
    protected bool _startDie = false;
    //사거리 유무
    protected bool _IsRange = false;


    protected RespawnManager respawnManager;
    public PlayerStats _pStats { get; set; }


    //외부 namespace Define의 Player State 참조
    //public = 변수나 멤버의 접근 범위를 가장 넓게 설정
    public CardType _cdType { get; protected set; }
    public PlayerType _pType { get; protected set; }
    public State _state { get; protected set; } = State.Idle;
    public CameraMode _cameraMode { get; protected set; } = CameraMode.FloatCamera;
    public Projectile _proj { get; protected set; } = Projectile.Undefine;


    //State
    public virtual State State
    {
        get { return _state; }
        set
        {
            _state = value;

            switch (_state)
            {
                case Define.State.Idle:
                    _anim.SetBool("IsIdle", true);
                    _anim.SetBool("IsWalk", false);
                    _anim.SetBool("IsThrow1", false);
                    _anim.SetBool("IsFire", false);

                    break;

                case Define.State.Moving:
                    _anim.SetBool("IsWalk", true);
                    _anim.SetBool("IsIdle", false);
                    _anim.SetBool("IsThrow1", false);
                    _anim.SetBool("IsFire", false);

                    break;

                case Define.State.Attack:
                    _anim.SetBool("IsFire", true);
                    _anim.SetBool("IsIdle", false);
                    _anim.SetBool("IsThrow1", false);

                    break;

                case Define.State.Skill:
                    _anim.SetBool("IsThrow1", true);
                    _anim.SetBool("IsIdle", false);
                    _anim.SetBool("IsFire", false);

                    break;

                case Define.State.Die:
                    _anim.SetTrigger("Die");
                    _anim.SetBool("IsIdle", false);

                    break;
            }
        }
    }

    private void Awake() 
    {
        // 팀 분배
        Init(); 
    }

    private void Update()
    {
        if (_startDie == false)
        {
            UpdatePlayerStat();
            Debug.Log("Alive");
        }

        if (_stopSkill == true)
        {
            StopSkill();
        }
        if (_stopAttack == true)
        {
            StopAttack();
        }

        //A키를 눌렀을 때 
        if (_IsRange == true && BaseCard._NowKey == KeyboardEvent.A.ToString())
        {
            RangeAttack();
        }
        //Debug.Log(State);

        //키, 마우스 이벤트 받으면 state가 변환
        switch (State)
        {
            case Define.State.Idle:
                if (_pv.IsMine)
                    UpdateIdle();
                break;

            case Define.State.Die:
                if (_pv.IsMine)
                {
                    UpdateDie();
                }

                break;

            case Define.State.Moving:
                UpdateMoving();
                break;

            case Define.State.Attack:
                if (_pv.IsMine)
                {
                    if (_stopAttack == false)
                        UpdateAttack();
                }
                break;

            case Define.State.Skill:
                if (_pv.IsMine)
                {
                    if (_stopSkill == false)
                        UpdateSkill();
                }
                break;
        }
    }


    //abstract = 하위 클래스에서 꼭 선언해야함.
    public abstract void Init();

    public virtual void awakeInit() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMoving() { if (!PhotonNetwork.IsMasterClient) return; }
    protected virtual void UpdateAttack() { }
    protected virtual void UpdateSkill() { }
    protected virtual void UpdateDie() { }
    protected virtual void UpdatePlayerStat() { }
    protected virtual GameObject RangeAttack() { return null; }
    protected virtual void StopAttack() { }
    protected virtual void StopSkill() { }
    protected virtual void StartDie() { }

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
}
