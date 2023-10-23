using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;


public class JobCard_WindBlade : UI_Card
{
    public override void Init()
    {
        _cost = 2;

        _rangeType = Define.CardType.Arrow;
        _rangeScale = 3.0f;
        _rangeRange = 5.0f;

        _CastingTime = 0.7f;
        _effectTime = 1.0f;
    }


    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        GameObject player = RemoteTargetFinder(playerId);

        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/EffectJob_WindBlade", player.transform.position, GetDirectionalVector(ground, player.transform));

        //effect ��ġ
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
