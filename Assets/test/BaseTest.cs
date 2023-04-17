using System.Collections;
using Data;
using System.Collections.Generic;
using UnityEngine.UI;
using JetBrains.Annotations;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public abstract class BaseTest : MonoBehaviour
{
    public Action<Define.KeyboardEvent> cardEvent = null;
    Define.CardType _cardType = Define.CardType.Undefine;

    private void Start()
    {
        BaseSetting();
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


    public virtual void BaseSetting()
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


    public virtual void TypeVerify(BaseTest card)
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


