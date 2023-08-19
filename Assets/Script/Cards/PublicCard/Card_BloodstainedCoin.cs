using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class Card_BloodstainedCoin : UI_Card
{
    int _layer = default;
    int _enemylayer = default;

    public override void Init()
    {
        _cardBuyCost = 500;
        _cost = 1;
        _damage = 10f;
        _rangeType = "Range";
        _rangeScale = 5.0f;

        _CastingTime = 0.3f;
        _effectTime = default;

        _IsResurrection = false;
    }


    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        GameObject _player = GameObject.Find(player);

        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_BloodstainedCoin2");
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = new Vector3(0, 0.8f, 0);

        _layer = layer;

        if (_layer == 6) { _enemylayer = 7; }
        if (_layer == 7) { _enemylayer = 6; }

        _effectObject.AddComponent<BloodstainedCoinStart>().StartBloodstainedCoin(player, _damage);

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
