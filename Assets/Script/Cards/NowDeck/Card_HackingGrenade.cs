using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_HackingGrenade : UI_Card
{
    public override void Init()
    {
        _cost = 1;
        _rangeType = "Point";
        _rangeScale = 5.0f;
        _rangeRange = 6.0f;

        _CastingTime = 0.3f;
    }

    public override void InitCard()
    {
        Debug.Log($"{this.gameObject.name} is called");
        Debug.Log($"마나 {_cost} 사용 ");
        Debug.Log($"{_rangeScale}내 적 카드 사용 불가");
    }

    public override void cardEffect(Transform trans)
    {
        Managers.Resource.Instantiate($"Particle/Card_HackingGrenade", trans);
    }

    public override void DestroyCard(float delay)
    {
        Destroy(this.gameObject, delay);
    }
}
