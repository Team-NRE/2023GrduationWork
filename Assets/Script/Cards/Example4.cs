using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example4 : UI_Card
{
	public override void InitCard()
	{
		Debug.Log($"{this.gameObject.name} is called");
		DestroyCard();
	}

	public void cardEffect()
    {

    }

	public override void DestroyCard()
	{
		Managers.Resource.Destroy(this.gameObject);
	}
}
