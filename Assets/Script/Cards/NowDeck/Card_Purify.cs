using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
public class Card_Purify : UI_Card
{
    public override void Init()
    {
        _cost = 0;
        
        _rangeType = "None";



        _CastingTime = 0.3f;
        _effectTime = 2.0f;
    }

    public override GameObject cardEffect(Transform Ground = null, Transform Player = null, LayerMask layer = default)
    {
        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_Purify", Player);
        PlayerStats pStat = Player.gameObject.GetComponent<PlayerStats>();
        pStat.nowState = "Health"; 

        return _effectObject;
    }

    public override void DestroyCard(GameObject Particle = null, float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
