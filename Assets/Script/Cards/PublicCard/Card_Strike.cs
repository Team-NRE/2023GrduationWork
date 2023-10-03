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
        //_damage = 40f;
        _rangeType = Define.CardType.Range;
        _rangeScale = 5.0f;

        _CastingTime = 0.3f;
        _effectTime = 2.0f;
    }


    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_Strike");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Strike", ground, Quaternion.Euler(-90, 0, 0));
        //_effectObject.transform.parent = BaseCard._lockTarget.transform;
        //_effectObject.transform.SetParent(BaseCard._lockTarget.transform);

        //_effectObject.transform.localPosition = new Vector3(0, 1.8f, 0);
        _targetId = Managers.game.RemoteTargetIdFinder(BaseCard._lockTarget);

        //_effectObject.AddComponent<StrikeStart>().StartStrike(playerId, _damage, _effectTime);
        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId, _targetId);

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
