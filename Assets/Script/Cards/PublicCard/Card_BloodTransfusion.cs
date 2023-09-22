using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����
public class Card_BloodTransfusion : UI_Card
{
    int _layer = default;
    int _enemylayer = default;

    public override void Init()
    {
        _cardBuyCost = 300;
        _cost = 1;
        _damage = 30f;
        _rangeType = Define.CardType.Range;
        _rangeScale = 5.0f;

        _CastingTime = 0.3f;
        _effectTime = 1.3f;
    }


    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_BloodTransfusion");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_BloodTransfusion", BaseCard._lockTarget.transform.position, Quaternion.Euler(-90, 0, 0));
        _effectObject.transform.parent = BaseCard._lockTarget.transform;
        _effectObject.transform.localPosition = new Vector3(0, 0.8f, 0);

        _layer = layer;

        if (_layer == 6) { _enemylayer = 7; }
        if (_layer == 7) { _enemylayer = 6; }

        //_effectObject.AddComponent<BloodTransfusionStart>().StartBloodTransfusion(playerId, _damage);
        _effectObject.GetComponent<BloodTransfusionStart>().StartBloodTransfusion(playerId, _damage);

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
