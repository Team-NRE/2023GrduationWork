using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_InvincibleWeapon : UI_Card
{
    public override void Init()
    {
        _cost = 3;
        _rangeType = "Arrow";

        _CastingTime = 2.0f;
        _effectTime = 2.0f;
    }

    public override void InitCard()
    {
        Debug.Log($"{this.gameObject.name} is called");
        Debug.Log($"마나 {_cost} 사용 ");
        Debug.Log($"큰 화살 발사");
    }

    public override GameObject cardEffect(Transform Ground = null, Transform Player = null, LayerMask layer = default)
    {
        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_InvincibleWeapon", Player);

        return _effectObject;
    }

    public override void DestroyCard(GameObject Particle = null, float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
