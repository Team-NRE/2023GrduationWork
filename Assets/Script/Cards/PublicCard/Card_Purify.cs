using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
public class Card_Purify : UI_Card
{
    public override void Init()
    {
        _cardBuyCost = 1200;
        _cost = 0;

        _rangeType = "None";

        _CastingTime = 0.3f;
        _effectTime = 2.0f;
    }

    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        GameObject _player = GameObject.Find(player);
        PlayerStats _pStat = _player.GetComponent<PlayerStats>();

        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_Purify");
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = new Vector3(0, 1.12f, 0);

        _pStat.nowState = "Health";

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
