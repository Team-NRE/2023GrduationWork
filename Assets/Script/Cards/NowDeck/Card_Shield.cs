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
        _effectTime = 2.0f;
    }


    public override void InitCard()
    {
        Debug.Log($"방어력 증가");
    }

    public override GameObject cardEffect(Transform Ground = null, Transform Player = null, LayerMask layer = default)
    {
        //따라다녀야함
        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_Shield", Player);

        return _effectObject;
    }

    public override void DestroyCard(GameObject Particle = null, float delay = default)
    {
        Destroy(this.gameObject, delay); 
    }
}
