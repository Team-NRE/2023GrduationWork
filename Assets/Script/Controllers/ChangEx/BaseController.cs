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
    protected InputActionMap inputAction;
    protected Animator animator;
    protected NavMeshAgent agent;


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
                    animator.SetBool("IsIdle", true);
                    animator.SetBool("IsWalk", false);
                    animator.SetBool("IsThrow1", false);
                    animator.SetBool("IsFire", false);

                    break;

                case Define.State.Moving:
                    //Invoke("idle", 0.2f);
                    animator.SetBool("IsWalk", true);
                    animator.SetBool("IsIdle", false);
                    animator.SetBool("IsThrow1", false);
                    animator.SetBool("IsFire", false);

                    break;

                case Define.State.Attack:
                    //Invoke("idle", 0.2f);
                    //agent.ResetPath();
                    animator.SetBool("IsFire", true);
                    animator.SetBool("IsIdle", false);
                    animator.SetBool("IsThrow1", false);

                    break;

                case Define.State.Skill:
                    //Invoke("idle", 0.2f);
                    //agent.ResetPath();
                    animator.SetBool("IsThrow1", true);
                    animator.SetBool("IsIdle", false);
                    animator.SetBool("IsFire", false);

                    break;

                case Define.State.Die:
                    animator.SetTrigger("Die");
                    animator.SetBool("IsIdle", false);
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
        switch (State)
        {
            //상시로 판별하면서 state가 변환
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



    //Effects for normal attack
    public virtual void LoadEffect()
    {

    }

    public virtual void LoadProjectile()
    {
        //Projectile Component must has Effect LoadEffect Function Defaultly
    }

    public virtual void SetStat()
    {

    }

}


