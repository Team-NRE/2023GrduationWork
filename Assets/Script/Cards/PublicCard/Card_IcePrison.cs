using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

// ���� ����
public class Card_IcePrison : UI_Card
{
    public override void Init()
    {
        _cardBuyCost = 750;
        _cost = 1;

        _rangeType = Define.CardType.None;

        _CastingTime = 0.3f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_IcePrison", ground, Quaternion.Euler(-90, 0, 0));
        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId, 3.0f);

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
