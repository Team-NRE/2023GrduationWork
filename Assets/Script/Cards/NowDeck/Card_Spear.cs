using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Spear : UI_Card
{
    public override void Init()
    {
        _cost = 1;
        _rangeType = "Line";
        _rangeScale = 3.0f;

        _CastingTime = 1.0f;
    }

    public override void InitCard()
    {
        Debug.Log($"{this.gameObject.name} is called");
        Debug.Log($"마나 {_cost} 사용 ");
        Debug.Log($"표창을 던짐");
    }

    public override void cardEffect(Transform trans)
    {
        Managers.Resource.Instantiate($"Particle/Boom", trans);
    }

    public override void DestroyCard(float delay)
    {
        Destroy(this.gameObject, delay);
    }
}
