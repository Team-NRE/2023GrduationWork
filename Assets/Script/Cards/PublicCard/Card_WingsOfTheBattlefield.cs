using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ����
public class Card_WingsOfTheBattlefield : UI_Card
{  
    int _layer = default;

    public override void Init()
    {
        _cardBuyCost = 2000;
        _cost = 2;

        _rangeScale = 3.6f;
        _rangeType = Define.CardType.None;

        _effectTime = 0.9f;
        _CastingTime = 0.3f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_WingsoftheBattlefield2", ground, Quaternion.Euler(-90, 0, 0));
        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
