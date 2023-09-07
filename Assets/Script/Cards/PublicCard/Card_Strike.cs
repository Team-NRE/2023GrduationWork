using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Strike : UI_Card
{
    int _layer = default;
    int _enemylayer = default;

    public override void Init()
    {
        _cardBuyCost = 1400;
        _cost = 2;
        _damage = 40f;
        _rangeType = Define.CardType.Range;
        _rangeScale = 5.0f;

        _CastingTime = 0.3f;
        _effectTime = 2.0f;
    }


    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_Strike");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Strike", ground, Quaternion.Euler(-90, 0, 0));
        _effectObject.transform.parent = BaseCard._lockTarget.transform;
        _effectObject.transform.localPosition = new Vector3(0, 1.8f, 0);

        _layer = layer;

        if (_layer == 6) { _enemylayer = 7; }
        if (_layer == 7) { _enemylayer = 6; }

        _effectObject.AddComponent<StrikeStart>().StartStrike(player, _damage, _effectTime);

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
