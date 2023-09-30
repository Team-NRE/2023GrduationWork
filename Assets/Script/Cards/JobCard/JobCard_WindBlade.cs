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
        //_effectObject = Managers.Resource.Instantiate($"Particle/EffectJob_Grenade");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/EffectJob_Grenade", ground, Quaternion.identity);
        _effectObject.transform.position = ground;

        //_effectObject.AddComponent<GrenadeStart>().StartGrenade(playerId, _damage, _enemylayer);
        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
