using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Infection : UI_Card
{
    int _layer = default;
    int _enemylayer = default;

    public override void Init()
    {
        _cardBuyCost = 300;
        _cost = 1;
        _damage = 0.1f;
        _rangeType = Define.CardType.Point;
        _rangeScale = 1.75f;
        _rangeRange = 3.0f;

        _CastingTime = 0.3f;
        _effectTime = 3.0f;
    }


    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_Infection");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Infection", ground, Quaternion.Euler(-90, 0, 0));
        _effectObject.transform.position = new Vector3(ground.x, 0.5f, ground.z);

        _layer = layer;

        if (_layer == 6) { _enemylayer = 7; }
        if (_layer == 7) { _enemylayer = 6; }

        _effectObject.AddComponent<InfectionStart>().StartInfection(player, _damage, _enemylayer);

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
