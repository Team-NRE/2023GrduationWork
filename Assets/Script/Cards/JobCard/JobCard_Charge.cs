using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobCard_Charge : UI_Card
{
    public override void Init()
    {
        _cost = 1;
        _rangeType = "Point";
        _rangeScale = 3.0f;
        _rangeRange = 5.0f;

        _CastingTime = 0.7f;
        _effectTime = 0.7f;
    }

    public override void InitCard()
    {
        Debug.Log($"{this.gameObject.name} is called");
        Debug.Log($"마나 {_cost} 사용 ");
        Debug.Log($"{_rangeScale}내 적 카드 사용 불가");
    }

    /*
    public override GameObject cardEffect(Transform Ground = null, Transform Player = null, LayerMask layer = default)
    {
        _effectObject = Managers.Resource.Instantiate($"Particle/EffectJob_Grenade", Ground);

        return _effectObject;
    }*/

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
