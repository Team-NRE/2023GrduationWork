using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Define;

public class InputManager
{
    public Action<KeyboardEvent> KeyAction = null;
    public Action<MouseEvent> MouseAction = null;


    bool _pressed = false;
    float _pressedTime = 0.0f;


    //키 이벤트
    public void OnKeyDown(InputAction.CallbackContext context)
    {
        string keyName = context.control.name;
        int keyValue = (int)Enum.Parse(typeof(KeyCode), keyName, true);
        //키보드  Define.KeyboardEvent 로 받기
        KeyboardEvent keyboardEvent = (KeyboardEvent)keyValue;
        KeyAction.Invoke(keyboardEvent);
    }


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
        if (KeyAction != null && Input.anyKey != false)
            KeyAction.Invoke(KeyboardEvent.NoInput);

        if (MouseAction != null)
        {
            //현재 포인터가 UI 객체 위에 있는지 여부를 확인하는 데 사용
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            //마우스 왼쪽 / 오른쪽 버튼 누를 시 
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                //눌렀을 때
                if (!_pressed)
                {
                    //눌렀을 때 PointerDown 액션 전달
                    MouseAction.Invoke(MouseEvent.PointerDown);
                    //누른 시간 저장
                    _pressedTime = Time.time;
                }

                //누르는 동안 Press 액션 전달
                MouseAction.Invoke(MouseEvent.Press);
                //PointerDown 액션 전달 하지 않게 true로 변경
                _pressed = true;
            }
            
            //마우스 왼쪽 / 오른쪽 버튼 땠을 시
            else
            {
                //클릭 여부 판별
                if (_pressed)
                {
                    //버튼을 땠을 때의 시간이 저장한 시간+0.2f 보다 작을 때
                    if (Time.time < _pressedTime + 0.2f)
                        //클릭 형태가 됨.
                        MouseAction.Invoke(MouseEvent.Click);
                }
                _pressed = false;
                _pressedTime = 0;
            }
        }
    }


    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}