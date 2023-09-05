using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class Card_Spear : UI_Card
{
    LayerMask _layer = default;
    LayerMask _enemylayer = default;

    public override void Init()
    {
        _cardBuyCost = 100;
        _cost = 1;
        _damage = 15;

        _rangeType = "Line";
        _rangeScale = 5.0f;

        _CastingTime = 0.77f;
        _effectTime = 0.77f;
    }

    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        //GameObject _player = GameObject.Find(player);
        GameObject _player = Managers.game.myCharacter;

        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_Spear");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Spear", ground, Quaternion.identity);
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = new Vector3(-0.1f, 1.12f, 0.9f);

        _layer = layer;

        if (_layer == 6) { _enemylayer = 7; }
        if (_layer == 7) { _enemylayer = 6; }

        _effectObject.AddComponent<SpearStart>().StartSpear(player, _enemylayer, _damage);

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
