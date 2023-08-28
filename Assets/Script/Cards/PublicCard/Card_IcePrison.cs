using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class Card_IcePrison : UI_Card
{
    public override void Init()
    {
        _cardBuyCost = 750;
        _cost = 1;

        _rangeType = "None";

        _CastingTime = 0.3f;
        _effectTime = 3.0f;
    }

    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        GameObject _player = GameObject.Find(player);
        PlayerStats _pStat = _player.GetComponent<PlayerStats>();

        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_IcePrison");
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = new Vector3(0, 0.3f, 0);
        _effectObject.AddComponent<IcePrisonStart>().StartIcePrison(player, _effectTime);

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
