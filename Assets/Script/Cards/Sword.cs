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
}
