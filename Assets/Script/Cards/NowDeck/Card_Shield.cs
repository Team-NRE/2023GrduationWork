using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Shield : UI_Card
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
        Debug.Log($"방어력 증가");
    }

    public override void cardEffect(Transform trans)
    {
        Managers.Resource.Instantiate($"Particle/Card_Shield", trans);
        //따라다녀야함
    }
    
    public override void DestroyCard(float delay)
    {
        Destroy(this.gameObject, delay);
    }
}
