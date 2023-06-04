using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Stat;


public class PlayerExample : BaseExample
{
    
    //int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster); //레이어 마스크 숫자로 저장하기
    bool _stopSkill = false;
    PlayerStats _stat;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;

        //_stat = gameObject.GetComponent<PlayerStat>();
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
        //if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            //Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateMoving()
    {
        //몬스터가 내 사정거리보다 가까우면 공격, 단 타겟이 있으면
        if (_lockTarget != null)
        {
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= 1)
            {
                State = Define.State.Skill;
                return;
            }
        }

        //이동부분
        Vector3 dir = _destPos - transform.position;
        dir.y = 0;  //어딘가로 타고 올라가지 못하게함
        if (dir.magnitude < 0.1f) //Vector3에서 Vector3를 빼면 0이 나오는 경우는 거의 없다 -> 자료구조상 오차는 반드시 존재
        {
            State = Define.State.Idle;
        }
        else
        {
            Debug.DrawRay(transform.position, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0) == false)
                    State = Define.State.Idle;
                return;
            }
            //float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            //transform.position += dir.normalized * moveDist;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            //transform.LookAt(_destPos);
        }
    }

    protected override void UpdateSkill()
    {
        if(_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    void OnHitEvent()
    {
        if (_lockTarget != null)
        {
            //Stat targetStat = _lockTarget.GetComponent<Stat>();
            //targetStat.OnAttacked(_stat);
        }

        //TODO
        if (_stopSkill)
        {
            State = Define.State.Idle;
        }

        else
        {
            State = Define.State.Skill;
        }
    }

    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
                {
                    if (evt == Define.MouseEvent.PointerUp)
                        _stopSkill = true;
                }
                break;
        }
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        //if (evt != Define.MouseEvent.Click)
        //    return;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        //layer mask를 사용한다는 것은 결국 비트플래그를 사용한다는 것이다.
        //int mask = (1 << 8) | (1 << 9);   //8번 비트를 켜라 | 9번비트를 켜라

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    /*if (raycastHit)
                    {
                        _destPos = hit.point;
                        State = Define.State.Moving;
                        _stopSkill = false;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            _lockTarget = hit.collider.gameObject;
                        else
                            _lockTarget = null;
                    }*/
                }
                break;
            case Define.MouseEvent.Press:
                {
                    /*if (_lockTarget == null && raycastHit)
                        _destPos = hit.point;*/
                }
                break;
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
        }
    }
}

