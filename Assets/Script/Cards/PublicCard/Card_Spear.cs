using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

// ǥâ
public class Card_Spear : UI_Card
{
    LayerMask _layer = default;
    LayerMask _enemylayer = default;

    public override void Init()
    {
        _cardBuyCost = 100;
        _cost = 1;
        //_damage = 15;

        _rangeType = Define.CardType.Line;
        _rangeScale = 5.0f;

        _CastingTime = 0.77f;
        _effectTime = 0.77f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {

        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Spear", ground, Quaternion.Euler(-90, 0, 0));
        _layer = layer;

        if (_layer == 6) { _enemylayer = 7; }
        if (_layer == 7) { _enemylayer = 6; }

        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
