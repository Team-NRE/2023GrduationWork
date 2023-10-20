using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���
public class Card_CrisisAversion : UI_Card
{

    public override void Init()
    {
        _cardBuyCost = 2200;
        _cost = 3;

        _rangeType = Define.CardType.None;
        _rangeScale = 3.6f;

        _CastingTime = 0.3f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default) 
    {
        
        GameObject _player = Managers.game.RemoteTargetFinder(playerId);

        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_CrisisAversion", ground, Quaternion.Euler(-90, 0, 0));
        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);

        return _effectObject;
    }



    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
