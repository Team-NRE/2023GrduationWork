using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example7 : UI_Card
{
	private GameObject Range;

    public override void Init()
    {
        _rangeType = "Point";
        _rangeScale = 6.0f;
        _rangeRange = 6.0f;

        Range = GameObject.Find("AttackRange");
    }

    public override void InitCard()
    {
        Debug.Log($"{this.gameObject.name} is called");
        Managers.Resource.Instantiate($"Particle/Boom", Range.transform.GetChild(3));
    }

    public override void cardEffect(Transform trans)
    {
        Managers.Resource.Instantiate($"Particle/Boom", trans);
    }
    
    public override void DestroyCard()
    {
        Managers.Resource.Destroy(this.gameObject);
    }
}
