using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_InvincibleShield : UI_Card
{
    public override void Init()
    {
        _cost = 3;
        _rangeType = "Range";
        _rangeScale = 5.0f;

        _CastingTime = 0.3f;
    }

    public override void InitCard()
    {
        Debug.Log($"{this.gameObject.name} is called");
        Debug.Log($"마나 {_cost} 사용 ");
        Debug.Log($"{_rangeScale}내에 팀원들 버프");
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
