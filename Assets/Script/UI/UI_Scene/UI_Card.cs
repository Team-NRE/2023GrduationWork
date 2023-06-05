using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Card : UI_Scene
{
    //카드 비용
    public int _cost;
    //카드 시전 시간
    public float _CastingTime;
    //이펙트 발동 시간
    public float _effectTime; 

    public float _damage;
    public float _defence;
    public float _debuff;
    public float _buff;

    //스킬 범위 타입
    //Arrow = _rangeScale 고정 / Cone = _rangeScale, _rangeAngle / Line = _rangeScale 
    //Point = _rangeScale, _rangeRange / Range = _rangeScale
    public string _rangeType;
    //스킬 범위 크기
    public float _rangeScale;
    //스킬 거리
    public float _rangeRange;
    //스킬 각도
    public float _rangeAngle;

    public override void Init()
    {
        Debug.Log("UI_Card Init");
    }


    public virtual void InitCard()
    {
        //하위 카드 컴포넌트에서 구현하여 사용 위함
    }
    
    public virtual void cardEffect(Transform trans)
    {
        
    }

    public virtual void DestroyCard(float delay)
    {
        //하위 카드 컴포넌트에서 구현하여 사용 위함
    }
}
