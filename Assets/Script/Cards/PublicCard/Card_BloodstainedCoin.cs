using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

// �ǹ��� ����
public class Card_BloodstainedCoin : UI_Card
{
    int _layer = default;
    int _enemylayer = default;

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
        //GameObject _player = GameObject.Find(player);
        GameObject _player = Managers.game.RemoteTargetFinder(playerId);

        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_BloodstainedCoin2");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_BloodstainedCoin2", this.gameObject.transform.position, Quaternion.identity);
        //_effectObject.transform.parent = _player.transform;
        _effectObject.transform.SetParent(_player.transform);

        int effectId = _effectObject.GetComponent<PhotonView>().ViewID;
        _effectObject.transform.localPosition = new Vector3(0, 0.8f, 0);

        _layer = layer;

        if (_layer == 6) { _enemylayer = 7; }
        if (_layer == 7) { _enemylayer = 6; }

        //_effectObject.AddComponent<BloodstainedCoinStart>().StartBloodstainedCoin(_player.GetComponent<PhotonView>().ViewID, _damage);
        //_effectObject.GetComponent<BloodstainedCoinStart>().Card(_player.GetComponent<PhotonView>().ViewID, _damage);
        _effectObject.GetComponent<BloodstainedCoinStart>().CardEffectInit(_player.GetComponent<PhotonView>().ViewID);

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
