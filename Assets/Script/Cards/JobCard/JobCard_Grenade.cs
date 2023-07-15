using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class JobCard_Grenade : UI_Card
{
    Transform _Ground = null;
    Transform _Player = null;
    LayerMask _layer = default;
    LayerMask _enemylayer = default;
    bool isGrenade = false;

    public override void Init()
    {
        _cost = 2;
        _damage = 50;

        _rangeType = "Point";
        _rangeScale = 3.0f;
        _rangeRange = 5.0f;

        _CastingTime = 0.7f;
        _effectTime = 0.7f;
    }


    public override GameObject cardEffect(Transform Ground = null, Transform Player = null, LayerMask layer = default)
    {
        _effectObject = Managers.Resource.Instantiate($"Particle/EffectJob_Grenade", Ground);
        _Ground = Ground;   
        _Player = Player;
        _layer = layer;

        if (_layer == 1 << 6) { _enemylayer = 7; }
        if (_layer == 1 << 7) { _enemylayer = 6; }

        _effectObject.AddComponent<GrenadeStart>().StartGrenade(_Player, _damage, _enemylayer);
        
        return _effectObject;
    }


    public override void DestroyCard(GameObject Particle = null, float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
