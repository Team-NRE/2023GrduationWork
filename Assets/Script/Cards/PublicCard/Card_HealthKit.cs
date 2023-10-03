using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ｺ ŰƮ
public class Card_HealthKit : UI_Card
{
    int _layer = default;
    public override void Init()
    {
        _cardBuyCost = 1200;
        _cost = 1;
        _rangeType = Define.CardType.None;

        _CastingTime = 0.3f;
        _effectTime = 2.5f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_HealthKit", ground, Quaternion.Euler(-90, 0, 0));
        _layer = layer;
        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);
        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
