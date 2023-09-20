using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

// 순간이동
public class Card_Teleport : UI_Card
{
    public override void Init()
    {
        _cardBuyCost = 2200;
        _cost = 0;

        _rangeType = Define.CardType.Point;
        _rangeScale = 0.95f;
        _rangeRange = 2.0f;

        _CastingTime = 1.0f;
        _effectTime = 1.02f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        //GameObject _player = GameObject.Find(player);
        GameObject _player = Managers.game.RemoteTargetFinder(playerId);

        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_Teleport");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Teleport", ground, Quaternion.Euler(-90, 0, 0));

        _effectObject.transform.position = new Vector3(ground.x, 0.4f, ground.z);
        _player.transform.position = new Vector3(_effectObject.transform.position.x, _player.transform.position.y, _effectObject.transform.position.z);

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
