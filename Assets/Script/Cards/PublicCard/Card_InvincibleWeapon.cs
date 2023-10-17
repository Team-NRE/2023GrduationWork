using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ����
public class Card_InvincibleWeapon : UI_Card
{

    public override void Init()
    {
        _cardBuyCost = 3000;
        _cost = 3;
        _rangeType = Define.CardType.Arrow;

        _CastingTime = 2.0f;
        _effectTime = 3.0f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        GameObject player = RemoteTargetFinder(playerId);
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_InvincibleWeapon", player.transform.position, Quaternion.identity);

        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}