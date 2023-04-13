using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using UnityEngine.UI;
using System;
using JetBrains.Annotations;

public abstract class BaseController : MonoBehaviour
{
    Define.CardType _cardType = Define.CardType.Undefine;

    private void Start()
    {
        Debug.Log("Start");
        //CardDictionary();
    }

    public virtual void Init()
    {
        //this Functions details are writed in Child Scripts
    }

    public virtual void LoadEffect()
    {

    }

    public virtual void LoadProjectile()
    {
        //Projectile Component must has Effect LoadEffect Function Defaultly
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
