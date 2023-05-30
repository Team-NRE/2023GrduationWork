using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public abstract class BaseExample : MonoBehaviour
{
    [SerializeField]
    protected Vector3 _destPos;

    //위치 벡터
    //방향 벡터 : 크기와 방향을 동시에 알 수 있다.

    [SerializeField]
    protected State _state = State.Idle;

    [SerializeField]
    protected GameObject _lockTarget;

    public WorldObject WorldObjectType { get; protected set; } = WorldObject.Unknown;

    public virtual State State
    {
        get { return _state; }
        set
        {
            _state = value;

            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case Define.State.Die:

                    break;
                case Define.State.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case Define.State.Moving:
                    anim.CrossFade("RUN", 0.1f);
                    break;
                case Define.State.Skill:
                    anim.CrossFade("ATTACK", 0.1f);
                    break;
            }
        }
    }

    private void Start()
    {
    
    }

    void Update()
    {
        switch (State)
        {
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;
        }
    }

    public virtual void Init(){
        
    }

    protected virtual void UpdateDie()
    {

    }
    protected virtual void UpdateMoving()
    {

    }
    protected virtual void UpdateIdle()
    {

    }
    protected virtual void UpdateSkill()
    {

    }
}
