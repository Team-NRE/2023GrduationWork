using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����
public class Card_BloodTransfusion : UI_Card
{
    int _targetId;

    public override void Init()
    {
        _cardBuyCost = 500;
        _cost = 1;
        //_damage = 30f;
        _rangeType = Define.CardType.Range;
        _rangeScale = 5.0f;

        _CastingTime = 0.3f;
        _effectTime = 1.3f;
    }

    // 이건 타겟을 어떻게 가져오는지 문의
    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_BloodTransfusion", BaseCard._lockTarget.transform.position, Quaternion.Euler(-90, 0, 0));
        _targetId = Managers.game.RemoteTargetIdFinder(BaseCard._lockTarget);

        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId, _targetId);
        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
