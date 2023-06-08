using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Lava : UI_Card
{
    public override void Init()
    {
        _cost = 2;
        _rangeType = "Point";
        _rangeScale = 4.0f;
        _rangeRange = 6.0f;
        
        _CastingTime = 0.3f;
    }

    public override void InitCard()
    {
        Debug.Log($"{this.gameObject.name} is called");
        Debug.Log($"마나 {_cost} 사용 ");
        Debug.Log($"{_rangeScale} 크기의 독 장판 On");
    }

    public override void cardEffect(Transform trans)
    {
        Managers.Resource.Instantiate($"Particle/Card_Lava", trans);
    }

    public override void DestroyCard(float delay)
    {
        Destroy(this.gameObject, delay);
    }
}
