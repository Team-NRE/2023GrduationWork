using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Spear : UI_Card
{
    public override void Init()
    {
        _cost = 1;
        _rangeType = "Line";
        _rangeScale = 5.0f;

        _CastingTime = 1.0f;
        _effectTime = 1.0f;
    }

    public override void InitCard()
    {
        Debug.Log($"{this.gameObject.name} is called");
        Debug.Log($"마나 {_cost} 사용 ");
        Debug.Log($"표창을 던짐");
    }

    public override GameObject cardEffect(Transform Ground = null, Transform Player = null, LayerMask layer = default)
    {
        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_Spear", Player);

        _effectObject.transform.localPosition = new Vector3(-0.1f, 1.2f, 3.8f);

        return _effectObject;
    }

    public override void DestroyCard(GameObject Particle = null, float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
