using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class Card_HackingGrenade : UI_Card
{
    Transform _Ground = null;
    Transform _Player = null;
    LayerMask _layer = default;
    LayerMask _enemylayer = default;

    public override void Init()
    {
        _cost = 1;
        _damage = 25;
        _debuff = 1.02f;

        _rangeType = "Point";
        _rangeScale = 3.0f;
        _rangeRange = 5.0f;

        _CastingTime = 1.0f;
        _effectTime = 1.02f;
    }


    public override GameObject cardEffect(Transform Ground = null, Transform Player = null, LayerMask layer = default)
    {
        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_HackingGrenade", Ground);
        _Ground = Ground;
        _Player = Player;
        _layer = layer;

        if (_layer == 1 << 6) { _enemylayer = 7; }
        if (_layer == 1 << 7) { _enemylayer = 6; }

        _effectObject.AddComponent<GrenadeStart>().StartGrenade(_Player, _damage, _enemylayer, _debuff);
        

        return _effectObject;
    }


    public override void DestroyCard(GameObject Particle = null, float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
