using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ����
public class Card_InvincibleWeapon : UI_Card
{
    int _layer = default;
    int _enemylayer = default;

    public override void Init()
    {
        _cardBuyCost = 3000;
        _cost = 3;
        //_damage = 1;
        _rangeType = Define.CardType.Arrow;

        _CastingTime = 2.0f;
        _effectTime = 2.0f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        GameObject player = RemoteTargetFinder(playerId);
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_InvincibleWeapon", player.transform.position, GetDirectionalVector(ground, player.transform));

        _layer = layer;

        if (_layer == 6) { _enemylayer = 7; }
        if (_layer == 7) { _enemylayer = 6; }

        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}