using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : UI_Card
{

	public int _cost;
	public float _damage;
	public float _defence;
	public float _debuff;
	public float _buff;
	public float _range;
	public float _time;

    public override void InitCard()
    {
        Debug.Log($"{this.gameObject.name} is called");
    }

	public void cardEffect()
    {

    }

	public override void DestroyCard()
	{
		Debug.Log(this.gameObject.name + " Destroy Card");
		Managers.Resource.Destroy(this.gameObject);
	}
}
