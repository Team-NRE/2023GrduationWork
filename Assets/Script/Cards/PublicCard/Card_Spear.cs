using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

// ǥâ
public class Card_Spear : UI_Card
{
    public override void Init()
    {
        _cardBuyCost = 500;
        _cost = 1;

        _rangeType = Define.CardType.Line;
        _rangeScale = 5.0f;

        _CastingTime = 0.77f;
        _effectTime = 0.77f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        GameObject player = RemoteTargetFinder(playerId);

        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Spear", ground, Quaternion.Euler(-90, 0, 0));

        //effect 위치
        _effectObject.transform.parent = player.transform;
        _effectObject.transform.localPosition = new Vector3(-0.1f, 1.12f, 0.9f);
        _effectObject.transform.parent = null;

        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId, _effectObject.transform.rotation);

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
