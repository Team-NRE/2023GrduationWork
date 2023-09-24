using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 부활
public class Card_Resurrection : UI_Card
{
# warning 이건 클라이언트 코드가 아직 완성되지 않음
    public override void Init()
    {
        _cardBuyCost = 2300;
        _cost = 3;

        _rangeType = Define.CardType.None;

        _CastingTime = 0.3f;
        _effectTime = default;

        _IsResurrection = true;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        //GameObject _player = GameObject.Find(player);
        GameObject _player = Managers.game.RemoteTargetFinder(playerId);

        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_Resurrection");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Resurrection", ground, Quaternion.identity);
        _effectObject.transform.SetParent(_player.transform);

        _effectObject.transform.localPosition = new Vector3(0, 0.2f, 0);

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
