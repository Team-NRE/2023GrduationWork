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
        _effectTime = 2.0f;
    }

    public override void InitCard()
    {
        Debug.Log($"{this.gameObject.name} is called");
        Debug.Log($"마나 {_cost} 사용 ");
        Debug.Log($"정화됨");
    }

    public override GameObject cardEffect(Transform Ground = null, Transform Player = null, LayerMask layer = default)
    {
        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_Purify", Player);

        return _effectObject;
    }

    public override void DestroyCard(GameObject Particle = null, float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
