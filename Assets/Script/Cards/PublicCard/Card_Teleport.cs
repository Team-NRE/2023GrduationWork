using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

// �����̵�
public class Card_Teleport : UI_Card
{
    public override void Init()
    {
        _cardBuyCost = 3800;
        _cost = 3;

        _rangeType = Define.CardType.Point;
        _rangeScale = 0.95f;
        _rangeRange = 9.2f;

        _CastingTime = 1.0f;
        _effectTime = 1.02f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        GameObject _player = Managers.game.RemoteTargetFinder(playerId);

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
