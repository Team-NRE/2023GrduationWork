using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class Card_Shield : UI_Card
{
    Transform _Player = null;
    bool isShield = false;
    float defenceTime;

    public override void Init()
    {
        _cardBuyCost = 300;    
        _cost = 0;
        _defence = 50;
        _rangeType = "None";

        _CastingTime = 0.3f;
        _effectTime = 2.0f;
        defenceTime = 0.01f;
    }


    public override void InitCard()
    {
        Debug.Log($"방어력 증가");
    }


    public override GameObject cardEffect(Transform Ground = null, Transform Player = null, LayerMask layer = default)
    {
        //따라다녀야함
        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_Shield", Player);
        _Player = Player;

        _effectObject.AddComponent<ShieldStart>().StartShield(_Player, _defence);
        _Player.gameObject.GetComponent<PlayerStats>().defensePower += _defence;

        return _effectObject;
    }

    public override void DestroyCard(GameObject Particle = null, float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
