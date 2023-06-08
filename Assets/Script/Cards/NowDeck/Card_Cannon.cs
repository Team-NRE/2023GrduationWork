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
        _effectTime = 1.0f;
    }

    public override void InitCard()
    {
        Debug.Log($"{this.gameObject.name} is called");
        Debug.Log($"마나 {_cost} 사용 ");
        Debug.Log($"{_rangeScale}내에 Cone 모양 대포 발사");
    }

    public override GameObject cardEffect(Transform Ground = null, Transform Player = null, LayerMask layer = default)
    {
        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_Cannon", Ground);

        return _effectObject;
    }

    public override void DestroyCard(GameObject Particle = null, float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}