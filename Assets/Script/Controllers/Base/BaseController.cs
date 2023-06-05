using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using Define;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public abstract class BaseController : MonoBehaviourPunCallbacks, IPunObservable
{
    protected PhotonView _pv;
    protected Vector3 receivePos;
    protected Quaternion receiveRot;
    protected float damping = 10.0f;

    //SerializeField = private 변수를 인스펙터에서 설정
    //protected = 상속 관계에 있는 클래스 내부에서만 접근
    protected Animator _anim;
    protected NavMeshAgent _agent;

    protected GameObject _lockTarget;
    protected Vector3 _MovingPos;
    //총알 발사 여부
    protected bool _stopAttack = false;

    //외부 namespace Define의 Player State 참조
    //public = 변수나 멤버의 접근 범위를 가장 넓게 설정
    public State _state { get; protected set; } = State.Idle;
    public CameraMode _cameraMode { get; protected set; } = CameraMode.FloatCamera;

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
                    //agent.ResetPath();
                    _anim.SetBool("IsThrow1", true);
                    _anim.SetBool("IsIdle", false);
                    _anim.SetBool("IsFire", false);

                    break;

                case Define.State.Die:
                    _anim.SetTrigger("Die");
                    _anim.SetBool("IsIdle", false);
                    //inputAction.Disable();
                    //GetComponent<CapsuleCollider>().enabled = false;
                    //this.enabled = false;
                    //StopAllCoroutines();

                    break;
            }
        }
    }


    private void Start()
    {
        Init();

    }

    private void Update()
    {
        Debug.Log(State);
        if (_stopAttack == true) { StopAttack(); }
        switch (State)
        {
            case Define.State.Idle:
                UpdateIdle();
                break;

            case Define.State.Die:
                UpdateDie();
                break;

            //키, 마우스 이벤트 받으면 state가 변환
            case Define.State.Moving:
                UpdateMoving();
                break;

            case Define.State.Attack:
                UpdateAttack();

                break;

            case Define.State.Skill:
                UpdateSkill();
                break;
        }
    }


    //abstract = 하위 클래스에서 꼭 선언해야함.
    public abstract void Init();


    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateAttack() { }
    protected virtual void UpdateSkill() { }
    protected virtual void UpdateDie() { }

    protected virtual void StopAttack() { }

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
}

