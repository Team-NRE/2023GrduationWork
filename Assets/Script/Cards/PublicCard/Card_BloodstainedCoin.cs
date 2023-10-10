using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

// �ǹ��� ����
public class Card_BloodstainedCoin : UI_Card
{
    int _layer = default;

    public override void Init()
    {
        _cardBuyCost = 500;
        _cost = 1;
        //_damage = 10f;
        _rangeType = Define.CardType.Range;
        _rangeScale = 5.0f;

        _CastingTime = 0.3f;
        _effectTime = default;

        //_IsResurrection = false;
    }


    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        GameObject _player = Managers.game.RemoteTargetFinder(playerId);

        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_BloodstainedCoin2", _player.transform.position, Quaternion.identity);
        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId, BaseCard._lockTarget.GetComponent<PhotonView>().ViewID);

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
