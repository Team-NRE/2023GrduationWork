using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_CrisisAversion : UI_Card
{
    int _layer = default;


    public override void Init()
    {
        _cardBuyCost = 2800;
        _cost = 3;

        _rangeType = "None";
        _rangeScale = 3.6f;

        _CastingTime = 0.3f;
        _effectTime = 7.0f;
    }

    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        GameObject _player = GameObject.Find(player);
        _layer = layer;

        //¶ì·Î¸µ
        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_CrisisAversion");
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = new Vector3(0, 0.3f, 0);

        return _effectObject;
    }



    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
