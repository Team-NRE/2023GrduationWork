using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���
public class Card_Lava : UI_Card
{

    public override void Init()
    {
        _cardBuyCost = 1400;
        _cost = 2;
        //_damage = 0.1f;
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

        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
