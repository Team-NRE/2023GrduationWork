using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_HealthKit : UI_Card
{
    int _layer = default;
    public override void Init()
    {
        _cardBuyCost = 1200;
        _cost = 1;
        _rangeType = Define.CardType.None;

        _CastingTime = 0.3f;
        _effectTime = 2.5f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        //GameObject _player = GameObject.Find(player);
        GameObject _player = Managers.game.myCharacter;
        float _healthRegen = 0.5f;

        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_HealthKit");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_HealthKit", ground, Quaternion.Euler(-90, 0, 0));
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = new Vector3(0, 0.3f, 0);

        _layer = layer;

        _effectObject.AddComponent<HealthKitStart>().StartHealthKit(playerId, _healthRegen, _layer);

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
