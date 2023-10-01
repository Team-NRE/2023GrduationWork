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

        _rangeType = Define.CardType.Point;
        _rangeScale = 3.0f;
        _rangeRange = 5.0f;

        _CastingTime = 0.7f;
        _effectTime = 0.7f;
    }


    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        GameObject player = RemoteTargetFinder(playerId);
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/EffectJob_WindBlade", player.transform.position, Quaternion.identity);
        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
