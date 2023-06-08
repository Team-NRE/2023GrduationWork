using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Cannon : UI_Card
{
    public override void Init()
    {
        _cost = 2;
        _rangeType = "Point";
        _rangeScale = 1.5f;
        _rangeRange = 4.0f;

        _CastingTime = 0.7f;
    }

    public override void InitCard()
    {
        Debug.Log($"{this.gameObject.name} is called");
        Debug.Log($"마나 {_cost} 사용 ");
        Debug.Log($"{_rangeScale}내에 Cone 모양 대포 발사");
    }

    public override void cardEffect(Transform trans)
    {
        Managers.Resource.Instantiate($"Particle/Card_Cannon", trans);
    }

    public override void DestroyCard(float delay)
    {
        Destroy(this.gameObject, delay);
    }
}