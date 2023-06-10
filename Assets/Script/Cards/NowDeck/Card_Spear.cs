using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class Card_Spear : UI_Card
{
    LayerMask _layer = default;
    LayerMask _enemylayer = default;

    public override void Init()
    {
        _cost = 1;
        _damage = 15;

        _rangeType = "Line";
        _rangeScale = 5.0f;

        _CastingTime = 0.77f;
        _effectTime = 0.77f;
    }

    public override GameObject cardEffect(Transform Ground = null, Transform Player = null, LayerMask layer = default)
    {
        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_Spear", Player);


        _layer = layer;
        if (_layer == 1 << 6) { _enemylayer = 7; }
        if (_layer == 1 << 7) { _enemylayer = 6; }


        PlayerStats pStat = Player.gameObject.GetComponent<PlayerStats>();
        float speed = pStat._speed;
        _effectObject.transform.localPosition = new Vector3(-0.1f, 1.12f, 0.9f);
        _effectObject.AddComponent<SpearStart>().Setting(Player, _enemylayer, speed, _damage);

        return _effectObject;
    }


    public override void DestroyCard(GameObject Particle = null, float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
