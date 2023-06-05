using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_InvincibleWeapon : UI_Card
{
    public override void Init()
    {
        _cost = 3;
        _rangeType = "Arrow";

        _CastingTime = 0.7f;
    }

    public override void InitCard()
    {
        Debug.Log($"{this.gameObject.name} is called");
        Debug.Log($"마나 {_cost} 사용 ");
        Debug.Log($"큰 화살 발사");
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
