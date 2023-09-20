using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

// ��ŷ ����ź
public class Card_HackingGrenade : UI_Card
{
    int _layer = default;
    int _enemylayer = default;

    public override void Init()
    {
        _cardBuyCost = 1200;
        _cost = 1;
        _damage = 25;
        _debuff = 1.02f;

        _rangeType = Define.CardType.Point;
        _rangeScale = 3.0f;
        _rangeRange = 5.0f;

        _CastingTime = 1.0f;
        _effectTime = 1.02f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_HackingGrenade");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_HackingGrenade", ground, Quaternion.identity);
        _effectObject.transform.position = ground;

        _layer = layer;

        if (_layer == 6) { _enemylayer = 7; }
        if (_layer == 7) { _enemylayer = 6; }

        //_effectObject.AddComponent<GrenadeStart>().StartGrenade(playerId, _damage, _enemylayer, _debuff);
        _effectObject.GetComponent<GrenadeStart>().StartGrenade(playerId, _damage, _enemylayer, _debuff);
        
        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
