using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_InvincibleWeapon : UI_Card
{
    int _layer = default;
    int _enemylayer = default;

    public override void Init()
    {
        _cardBuyCost = 3000;
        _cost = 3;
        _damage = 1;
        _rangeType = "Arrow";

        _CastingTime = 2.0f;
        _effectTime = 2.0f;
    }

    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        GameObject _player = GameObject.Find(player);

        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_InvincibleWeapon");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_InvincibleWeapon", ground, Quaternion.identity);
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = Vector3.zero;
        _effectObject.transform.localRotation = Quaternion.Euler(-90, 180, 76);


        _layer = layer;

        if (_layer == 6) { _enemylayer = 7; }
        if (_layer == 7) { _enemylayer = 6; }

        _effectObject.AddComponent<InvincibleWeaponStart>().StartWeapon(player, _damage, _enemylayer);

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}