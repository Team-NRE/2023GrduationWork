using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;


public class Card_Armor : UI_Card
{

    public override void Init()
    {
        _cardBuyCost = 500;
        _cost = 0;
        _defence = 0.5f;
        _rangeType = "None";

        _CastingTime = 0.3f;
        _effectTime = 1.0f;
    }

    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        GameObject _player = GameObject.Find(player);
        PlayerStats _pStat = _player.GetComponent<PlayerStats>();

        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_Armor");
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = new Vector3(0, 0.8f, 0);

        _pStat.defensePower += _defence;

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
