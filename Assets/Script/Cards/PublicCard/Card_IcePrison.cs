using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

// ���� ����
public class Card_IcePrison : UI_Card
{
    public override void Init()
    {
        _cardBuyCost = 750;
        _cost = 1;

        _rangeType = Define.CardType.None;

        _CastingTime = 0.3f;
        _effectTime = 3.0f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        //GameObject _player = GameObject.Find(player);
        GameObject _player = Managers.game.RemoteTargetFinder(playerId);

        PlayerStats _pStat = _player.GetComponent<PlayerStats>();

        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_IcePrison");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_IcePrison", ground, Quaternion.Euler(-90, 0, 0));
        //_effectObject.transform.parent = _player.transform;
        _effectObject.transform.SetParent(_player.transform);

        _effectObject.transform.localPosition = new Vector3(0, 0.3f, 0);

        //_effectObject.AddComponent<IcePrisonStart>().StartIcePrison(playerId, _effectTime);
        //_effectObject.GetComponent<IcePrisonStart>().StartIcePrison(playerId, _effectTime);
        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
