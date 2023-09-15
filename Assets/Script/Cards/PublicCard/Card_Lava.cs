using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ¿ë¾Ï
public class Card_Lava : UI_Card
{
    int _layer = default;
    int _enemylayer = default;
    protected PhotonView _pv;

    public override void Init()
    {
        _cardBuyCost = 1400;
        _cost = 2;
        _damage = 0.1f;
        _rangeType = Define.CardType.Point;
        _rangeScale = 2.0f;
        _rangeRange = 4.0f;

        _CastingTime = 0.3f;
        _effectTime = 3.5f;
    }


    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_Lava");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Lava", ground, Quaternion.Euler(-90, 0, 0));
        _effectObject.transform.position = ground;

        _layer = layer;

        if (_layer == 6) { _enemylayer = 7; }
        if (_layer == 7) { _enemylayer = 6; }

        //_effectObject.AddComponent<LavaStart>().StartLava(playerId, _damage, _enemylayer);
        _effectObject.GetComponent<LavaStart>().StartLava(playerId, _damage, _enemylayer);

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
