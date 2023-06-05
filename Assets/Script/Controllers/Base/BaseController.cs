using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using Define;


[System.Serializable]
public abstract class BaseController : MonoBehaviour
{
    //SerializeField = private 변수를 인스펙터에서 설정
    //protected = 상속 관계에 있는 클래스 내부에서만 접근
    protected Animator _anim;
    protected NavMeshAgent _agent;

    protected GameObject _lockTarget;
    protected Vector3 _MovingPos;

    //총알 발사 여부
    protected bool _stopAttack = false;
    //스킬 발동 여부
    protected bool _stopSkill = false;


    //외부 namespace Define의 Player State 참조
    //public = 변수나 멤버의 접근 범위를 가장 넓게 설정
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
                    //inputAction.Disable();
                    //GetComponent<CapsuleCollider>().enabled = false;
                    //this.enabled = false;
                    //StopAllCoroutines();

                    break;
            }
        }
    }


    private void Start() { Init(); }

    private void Update()
    {
        Debug.Log(State);

        if (_stopAttack == true) { Debug.Log("평타 쿨타임"); StopAttack(); }
        if (_stopSkill == true) { Debug.Log("스킬 쿨타임"); StopSkill(); }

        //키, 마우스 이벤트 받으면 state가 변환
        switch (State)
        {
            case Define.State.Idle:
                UpdateIdle();
                break;

            case Define.State.Die:
                UpdateDie();
                break;

            case Define.State.Moving:
                UpdateMoving();
                break;

            case Define.State.Attack:
                if(_stopAttack == false)
                    UpdateAttack();
                break;

            case Define.State.Skill:
                if(_stopSkill == false)
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
    protected virtual void StopSkill() { }
}

