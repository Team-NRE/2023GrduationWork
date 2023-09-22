using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���
public class Card_CrisisAversion : UI_Card
{
    int _layer = default;
    int _enemylayer = default;

    public override void Init()
    {
        _cardBuyCost = 2800;
        _cost = 3;

        _rangeType = Define.CardType.None;
        _rangeScale = 3.6f;

        _CastingTime = 0.3f;
        _effectTime = default;

        _IsResurrection = false;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default) 
    {
        _layer = layer;
        if (_layer == 6) { _enemylayer = 7; }
        if (_layer == 7) { _enemylayer = 6; }
        
        //GameObject _player = GameObject.Find(player);
        GameObject _player = Managers.game.RemoteTargetFinder(playerId);

        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_CrisisAversion");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_CrisisAversion", ground, Quaternion.Euler(-90, 0, 0));
        //_effectObject.transform.parent = _player.transform;
        _effectObject.transform.SetParent(_player.transform);

        int effectId = _effectObject.GetComponent<PhotonView>().ViewID;

        _effectObject.transform.localPosition = new Vector3(0, 0.3f, 0);
        //_effectObject.AddComponent<CrisisAversionStart>().StartCrisisAversion(playerId, _enemylayer);
        _effectObject.GetComponent<CrisisAversionStart>().StartCrisisAversion(playerId, _enemylayer);

        return _effectObject;
    }



    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
