using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Lava : UI_Card
{
    public override void Init()
    {
        _cost = 2;
        _rangeType = "Point";
        _rangeScale = 3.0f;
        _rangeRange = 4.0f;

        _CastingTime = 0.3f;
        _effectTime = 4.0f;
    }

    public override void InitCard()
    {
        Debug.Log($"{this.gameObject.name} is called");
        Debug.Log($"마나 {_cost} 사용 ");
        Debug.Log($"{_rangeScale} 크기의 독 장판 On");
    }

    public override GameObject cardEffect(Transform Ground = null, Transform Player = null, LayerMask layer = default)
    {
        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_Lava", Ground);
        
        return _effectObject;
    }

    public override void DestroyCard(GameObject Particle = null, float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
