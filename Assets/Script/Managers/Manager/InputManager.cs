using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Define;

public class InputManager
{
    public Action<KeyboardEvent> KeyAction = null;
    public Action<MouseEvent> MouseAction = null;


    bool _MousePressed = false;
    float _pressedTime = 0.0f;



    //마우스 좌표에 따른 PlayerRotate
    public Vector3 FlattenVector(GameObject player, Vector3 mousepositon)
    {
        return new Vector3(mousepositon.x, player.transform.position.y, mousepositon.z);
    }


    //Ray로 마우스 좌표 받기
    public (Vector3, GameObject) Get3DMousePosition(int layerMask = default)
    {
        RaycastHit hit;

        //마우스 좌표에 위치하고 있는 오브젝트 레이어 구별.
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, ~layerMask))
        {
            return (hit.point, hit.collider.gameObject);
        }

        else { return (Vector3.zero, null); }
    }


    public void OnUpdate()
    {
        HitMouseEvent();
        HitKeyEvent();
    }

    //마우스 이벤트
    public void HitMouseEvent()
    {
        if (MouseAction != null)
        {
            //현재 포인터가 UI 객체 위에 있는지 여부를 확인하는 데 사용
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if(Input.GetMouseButton(0)) 
            {
                MouseAction.Invoke(MouseEvent.LeftButton);
                
                return;
            }

            //마우스 오른쪽 버튼 누를 시 
            if (Input.GetMouseButton(1))
            {
                //눌렀을 때
                if (!_MousePressed)
                {
                    //눌렀을 때 PointerDown 액션 전달
                    MouseAction.Invoke(MouseEvent.PointerDown);
                    //누른 시간 저장
                    _pressedTime = Time.time;
                }

                //누르는 동안 Press 액션 전달
                MouseAction.Invoke(MouseEvent.Press);
                //PointerDown 액션 전달 하지 않게 true로 변경
                _MousePressed = true;
            }

            //마우스 오른쪽 버튼 땠을 시
            else
            {
                //클릭 여부 판별
                if (_MousePressed)
                {
                    //버튼을 땠을 때의 시간이 저장한 시간+0.2f 보다 작을 때
                    if (Time.time < _pressedTime + 0.2f)
                        //클릭 형태가 됨.
                        MouseAction.Invoke(MouseEvent.PointerUp);
                }
                _MousePressed = false;
                _pressedTime = 0;
            }
        }
    }

    //키보드 이벤트
    public void HitKeyEvent()
    {
        //키보드만 입력 시
        if (KeyAction != null)
        {
            //스킬
            if (Input.GetKeyDown(KeyCode.Q))
            {
                KeyAction.Invoke(KeyboardEvent.Q);
            }

            //스킬
            else if (Input.GetKeyDown(KeyCode.W))
            {
                KeyAction.Invoke(KeyboardEvent.W);
            }

            //스킬
            else if (Input.GetKeyDown(KeyCode.E))
            {
                KeyAction.Invoke(KeyboardEvent.E);
            }

            //스킬
            else if (Input.GetKeyDown(KeyCode.R))
            {
                KeyAction.Invoke(KeyboardEvent.R);
            }

            //공격 사거리
            else if (Input.GetKeyDown(KeyCode.A))
            {
                KeyAction.Invoke(KeyboardEvent.A);
            }

            //카메라 고정
            else if (Input.GetKeyDown(KeyCode.U))
            {
                KeyAction.Invoke(KeyboardEvent.U);
            }

            //상점
            else if (Input.GetKeyDown(KeyCode.P))
            {
                KeyAction.Invoke(KeyboardEvent.P);
            }

            //설정
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                KeyAction.Invoke(KeyboardEvent.Escape);
            }

            //누르고 있을 때 상태창 On
            else if (Input.GetKey(KeyCode.Tab))
            {
                KeyAction.Invoke(KeyboardEvent.Tab);
            }

            //땠을 때 상태창 Off
            else if (Input.GetKeyUp(KeyCode.Tab))
            {
                KeyAction.Invoke(KeyboardEvent.TabUp);
            }     

            //누르고 있을 때 카메라 따라가기
            else if (Input.GetKey(KeyCode.Space))
            {
                KeyAction.Invoke(KeyboardEvent.Space);
            }

            //땠을 때 카메라 이동
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                KeyAction.Invoke(KeyboardEvent.SpaceUp);
            }
        }
    }


    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}

