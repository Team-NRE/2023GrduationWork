using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class Card_Cannon : UI_Card
{
    int _layer = default;
    int _enemylayer = default;

    public override void Init()
    {
        _cardBuyCost = 1400;
        _cost = 2;
        _damage = 50;
        _rangeType = Define.CardType.Point;
        _rangeScale = 1.5f;
        _rangeRange = 4.0f;

        _CastingTime = 0.7f;
        _effectTime = 1.0f;
    }

    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_Cannon");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Cannon", ground, Quaternion.identity);
        _effectObject.transform.position = ground;

        _layer = layer;

        if (_layer == 6) { _enemylayer = 7; }
        if (_layer == 7) { _enemylayer = 6; }

        _effectObject.AddComponent<CannonStart>().StartCannon(player, _damage, _enemylayer);

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}