using System.Collections;
using Data;
using System.Collections.Generic;
using UnityEngine.UI;
using JetBrains.Annotations;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using Stat;
using Define;


[System.Serializable]
public abstract class BaseController : MonoBehaviour
{
    //InputSystem
    public InputActionMap inputAction;


    //외부 namespace Stat 참조
    public PlayerStats _pStats { get; set; }
    public CardStats _cStats { get; set; }
    public ObjStats _oStats { get; set; }


    //외부 namespace Define의 Player State 참조
    public State _state { get; set; }
    public Layer _layer { get; set; }
    public KeyboardEvent _keyboardEvent { get; set; }
    public CardType _cardType { get; set; }
    public CameraMode _cameraMode { get; set; }
    public Projectile _projectile { get; set; }

    public void Start()
    {
        _pStats = GetComponent<PlayerStats>();
        _cStats = GetComponent<CardStats>();
        _oStats = GetComponent<ObjStats>();

        Setting();
    }

    //키 입력
    public virtual void OnEnable()
    {
        // Input System의 "action"을 정의
        var inputActionAsset = Resources.Load<InputActionAsset>("Input");
        inputAction = inputActionAsset.FindActionMap("KeyMap");
        inputAction.FindAction("Keyfunction").performed += OnKeyDown;
        inputAction.FindAction("Mousefunction").performed += OnMouseDown;
        inputAction.Enable();
    }

    //키 이벤트
    public void OnKeyDown(InputAction.CallbackContext context)
    {
        string keyName = context.control.name;
        int keyValue = (int)Enum.Parse(typeof(KeyCode), keyName, true);
        _keyboardEvent = (KeyboardEvent)keyValue;
        KeyDownAction(_keyboardEvent);
    }

    //마우스 이벤트
    public void OnMouseDown(InputAction.CallbackContext context)
    {
        string _button = context.control.name;

        MouseDownAction(_button);
    }


    //마우스 좌표에 따른 PlayerRotate
    protected Vector3 FlattenVector(Vector3 mousepositon)
    {
        return new Vector3(mousepositon.x, transform.position.y, mousepositon.z);
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


    //마우스 입력
    public virtual void MouseDownAction(string _button)
    {

    }

    //키 입력
    public virtual void KeyDownAction(KeyboardEvent _key)
    {

    }


    public virtual void Init()
    {

    }

    //Player/Camera 초기 세팅 -> start
    public virtual void Setting()
    {

    }

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


