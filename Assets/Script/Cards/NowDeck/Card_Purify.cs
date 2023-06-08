using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Purify : UI_Card
{
    public override void Init()
    {
        _cost = 0;
        _rangeType = "None";

        _CastingTime = 0.3f;
    }

    public override void InitCard()
    {
        Debug.Log($"{this.gameObject.name} is called");
        Debug.Log($"마나 {_cost} 사용 ");
        Debug.Log($"정화됨");
    }

    public override void cardEffect(Transform trans)
    {
        Managers.Resource.Instantiate($"Particle/Card_Purify", trans);
    }

    public override void DestroyCard(float delay)
    {
        Destroy(this.gameObject, delay);
    }
}
