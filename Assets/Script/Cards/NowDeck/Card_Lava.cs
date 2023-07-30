using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Lava : UI_Card
{
    LayerMask _layer = default;
    int _enemylayer = default;

    public override void Init()
    {
        _cardBuyCost = 1400;
        _cost = 2;
        _damage = 0.1f;
        _rangeType = "Point";
        _rangeScale = 2.0f;
        _rangeRange = 4.0f;

        _CastingTime = 0.3f;
        _effectTime = 3.5f;
    }


    public override GameObject cardEffect(Vector3 ground, string player, LayerMask layer = default)
    {
        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_Lava");
        _effectObject.transform.position = ground;

        _layer = layer;

        if (_layer == 6) { _enemylayer = 7; }
        if (_layer == 7) { _enemylayer = 6; }

        _effectObject.AddComponent<LavaStart>().StartLava(player, _damage, _enemylayer);

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
