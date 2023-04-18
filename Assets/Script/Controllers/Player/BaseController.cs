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


    //namespace Stat
    public PlayerStats _pStats { get; set; }
    public CardStats _cStats { get; set; }
    public ObjStats _oStats { get; set; }

    //namespace Define
    public State _state { get; set; }

    private void Start()
    {
        _pStats = GetComponent<PlayerStats>();
        _cStats = GetComponent<CardStats>();
        _oStats = GetComponent<ObjStats>();
        
        Player_Setting();
    }

    //키 입력
    private void OnEnable()
    {
        // Input System의 "action"을 정의
        var inputActionAsset = Resources.Load<InputActionAsset>("Input");
        InputAction inputAction = inputActionAsset.FindAction("Keyfunction");
        inputAction.performed += OnKeyDown;
        inputAction.Enable();
    }


    public void OnKeyDown(InputAction.CallbackContext context)
    {
        string name = context.control.name;
        KeyDownAction(name);
    }


    //Ray로 마우스 좌표 받기
    public static Vector3 Get3DMousePosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
        {
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

    //Player 초기 세팅 -> start
    public virtual void Player_Setting()
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


