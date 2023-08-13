using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class Card_HealthPotion : UI_Card
{
    
    public override void Init()
    {
        _cardBuyCost = 100;
        _cost = 0;
        _defence = 50;
        _rangeType = "None";

        _CastingTime = 0.3f;
        _effectTime = 1.0f;
    }

    public override GameObject cardEffect(Vector3 ground, string player, LayerMask layer = default)
    {
        GameObject _player = GameObject.Find(player);
        PlayerStats _pStat = _player.GetComponent<PlayerStats>();

        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_HealthPotion");
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = new Vector3(0, 0, 0);

        _pStat.nowHealth += 130;

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
