using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

// ����
public class Card_Cannon : UI_Card
{
    int _layer = default;

    public override void Init()
    {
        _cardBuyCost = 1400;
        _cost = 2;
        //_damage = 50;
        _rangeType = Define.CardType.Point;
        _rangeScale = 1.5f;
        _rangeRange = 4.0f;

        _CastingTime = 0.7f;
        _effectTime = 1.0f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_Cannon");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Cannon", ground, Quaternion.identity);
        _effectObject.transform.position = ground;

        _layer = layer;

        //_effectObject.AddComponent<CannonStart>().StartCannon(playerId, _damage, _enemylayer);
        //_effectObject.GetComponent<CannonStart>().StartCannon(playerId, _damage, _enemylayer);
        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit",RpcTarget.All, playerId);

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}