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

            if (Input.GetMouseButton(0))
            {
                //눌렀을 때
                if (!_pressed)
                {
                    MouseAction.Invoke(MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                MouseAction.Invoke(MouseEvent.Press);
                _pressed = true;
            }
            
            if(Input.GetMouseButton(1))
            {
                //땟을 때
                if (_pressed)
                {
                    if (Time.time < _pressedTime + 0.2f)
                        MouseAction.Invoke(MouseEvent.PointerUp);
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