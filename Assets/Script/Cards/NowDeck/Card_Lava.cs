using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Lava : UI_Card
{
    Transform _Player;
    LayerMask _layer = default;
    int _enemylayer = default;



    public override void Init()
    {
        _cost = 2;
        _damage = 0.1f;
        _rangeType = "Point";
        _rangeScale = 2.0f;
        _rangeRange = 4.0f;

        _CastingTime = 0.3f;
        _effectTime = 3.5f;
    }


    public override GameObject cardEffect(Transform Ground = null, Transform Player = null, LayerMask layer = default)
    {
        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_Lava", Ground);
        _Player = Player;
        _layer = layer;

        if (_layer == 1 << 6) { _enemylayer = 7; }
        if (_layer == 1 << 7) { _enemylayer = 6; }

        _effectObject.AddComponent<LavaStart>().StartLava(_Player, _damage, _enemylayer);


        return _effectObject;
    }


    public override void DestroyCard(GameObject Particle = null, float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
