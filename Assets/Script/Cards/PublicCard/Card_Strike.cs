using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��Ÿ
public class Card_Strike : UI_Card
{
    int _targetId;

    public override void Init()
    {
        _cardBuyCost = 1400;
        _cost = 2;
        _rangeType = Define.CardType.Range;
        _rangeScale = 5.0f;

        _CastingTime = 0.3f;
        _effectTime = 5.0f;
    }


    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Strike", ground, Quaternion.Euler(-90, 0, 0));
        _targetId = Managers.game.RemoteTargetIdFinder(BaseCard._lockTarget);

        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId, _targetId);

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
