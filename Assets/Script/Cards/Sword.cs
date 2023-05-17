using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : UI_Card
{
    public int CardCost = 1;

	public override void InitCard()
	{
		Debug.Log($"{this.gameObject.name} is called");
	}
    
    public void cardEffect()
    {
        Debug.Log(CardCost);
    }

	public override void DestroyCard()
	{
		Debug.Log(this.gameObject.name + " Destroy card");
		Managers.Resource.Destroy(this.gameObject);
	}
}
