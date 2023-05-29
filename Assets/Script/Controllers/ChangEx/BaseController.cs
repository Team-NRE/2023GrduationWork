using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using Define;


[System.Serializable]
public abstract class BaseController : MonoBehaviour
{
    //InputSystem
    //SerializeField = private 변수를 인스펙터에서 설정
    //protected = 상속 관계에 있는 클래스 내부에서만 접근
    protected InputActionMap _inputAction;
    protected Animator _anim;
    protected NavMeshAgent _agent;

    [SerializeField]
    protected GameObject _lockTarget;
    protected Vector3 _MovingPos;

    //외부 namespace Define의 Player State 참조
    //public = 변수나 멤버의 접근 범위를 가장 넓게 설정
    public State _state { get; protected set; } = State.Idle;
    public Layer _layer { get; protected set; } = Layer.Default;
    public KeyboardEvent _keyboardEvent { get; protected set; } = KeyboardEvent.NoInput;
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


    private void Start() { Init(); }
    private void Update()
    {
        StartCoroutine(UpdateState());
    }

    //abstract = 하위 클래스에서 꼭 선언해야함.
    public abstract void Init();

    public IEnumerator UpdateState() 
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log(State);
        
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


    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateAttack() { }
    protected virtual void UpdateSkill() { }
    protected virtual void UpdateDie() { }
}


