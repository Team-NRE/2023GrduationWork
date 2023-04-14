using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using UnityEngine.UI;
using System;
using JetBrains.Annotations;

public abstract class BaseController : MonoBehaviour
{
    public Action<Define.KeyboardEvent> cardEvent = null;
    Define.CardType _cardType = Define.CardType.Undefine;

	private void Start()
    {

    }

    public virtual void Init()
    {
        //this Functions details are writed in Child Scripts
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

    public void CardEventRecv(Define.KeyboardEvent evt)
    {
        //Get Event of Q,W,E,R -> return Component of Card
        switch (evt)
        {
            case Define.KeyboardEvent.Q:
                Debug.Log("Recv Q");
                break;
            case Define.KeyboardEvent.W:
				Debug.Log("Recv W");
				break;
            case Define.KeyboardEvent.E:
				Debug.Log("Recv E");
				break;
            case Define.KeyboardEvent.R:
				Debug.Log("Recv R");
				break;
        }
    }
}
