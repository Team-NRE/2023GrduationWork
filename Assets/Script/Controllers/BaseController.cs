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
    public Action<Define.KeyboardEvent> cardEvent = null;
    Define.CardType _cardType = Define.CardType.Undefine;

    //InputSystem
    public InputAction inputAction;


    //외부 namespace Stat 참조
    public PlayerStats _pStats { get; set; }
    public CardStats _cStats { get; set; }
    public ObjStats _oStats { get; set; }


    //외부 namespace Define의 Player State 참조
    public State _state { get; set; }
    public Layer _layer { get; set; }
    public KeyboardEvent _keyboard { get; set; }

    private void Start()
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
        inputAction = inputActionAsset.FindAction("Keyfunction");
        inputAction.performed += OnKeyDown;
        inputAction.Enable();
    }

    //키 이벤트를 name으로 받기
    public void OnKeyDown(InputAction.CallbackContext context)
    {

        string name = context.control.name;
        KeyDownAction(name);
    }


    //Ray로 마우스 좌표 받기
    public Vector3 Get3DMousePosition(params Layer[] _layers)
    {
        int layerMask = 0;
        foreach (var layer in _layers)
        {
            //만약 3과 5번의 레이어를 받았다면, OR 비트 연산자를 통해 000101000이 전달 됨.
            layerMask |= 1 << (int)layer;
        }

        RaycastHit hit;

        //해당 레이어만 탐지하도록 설정.
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
        {
            switch (hit.collider.gameObject.layer)
            {
                case (int)Define.Layer.Cyborg:
                    _layer = Define.Layer.Cyborg;

                    break;

                case (int)Define.Layer.Road:
                    _layer = Define.Layer.Road;

                    break;
            
            }

            return hit.point;
        }

        else
            return Vector3.zero;
    }

    //키 입력
    public virtual void KeyDownAction(string name)
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


    public virtual void TypeVerify(BaseController card)
    {
        //if card has Projectile
        switch (card._cardType)
        {
            case Define.CardType.Projective:
                break;
            case Define.CardType.NonProjective:
                card.LoadEffect();
                break;
            default:
                Debug.LogError($"{card.name}'s CardType is not Defined!");
                break;
        }
    }

}


