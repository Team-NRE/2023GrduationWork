using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

// ��ȭ
public class Card_Purify : UI_Card
{
    public override void Init()
    {
        _cardBuyCost = 1200;
        _cost = 0;

        _rangeType = Define.CardType.None;

        _CastingTime = 0.3f;
        _effectTime = 2.0f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Purify", ground, Quaternion.identity);

        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);
        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}