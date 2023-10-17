using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

// �ӵ�
public class Card_Speed : UI_Card
{
    public override void Init()
    {
        _cardBuyCost = 300;
        _cost = 0;
        _rangeType = Define.CardType.None;

        _CastingTime = 0.3f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Speed", ground, Quaternion.Euler(-90, 0, 0));
        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
