using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Resurrection : UI_Card
{

    public override void Init()
    {
        _cardBuyCost = 2300;
        _cost = 3;

        _rangeType = "None";

        _CastingTime = 0.3f;
        _effectTime = default;

        _IsResurrection = true;
    }

    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        GameObject _player = GameObject.Find(player);

        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_Resurrection");
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = new Vector3(0, 0.2f, 0);

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
