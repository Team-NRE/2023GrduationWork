using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class JobCard_Grenade : UI_Card
{
    int _layer = default;
    int _enemylayer = default;

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

    
    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        _effectObject = Managers.Resource.Instantiate($"Particle/EffectJob_Grenade");
        _effectObject.transform.position = ground;

        _layer = layer;

        if (_layer == 6) { _enemylayer = 7; }
        if (_layer == 7) { _enemylayer = 6; }

        _effectObject.AddComponent<GrenadeStart>().StartGrenade(player, _damage, _enemylayer);
        
        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}