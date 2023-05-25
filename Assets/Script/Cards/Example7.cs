using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example7 : UI_Card
{
	public override void InitCard()
	{
		Debug.Log($"{this.gameObject.name} is called");
	}

	public void cardEffect()
    {

    }

	public override void DestroyCard()
	{
		Managers.Resource.Destroy(this.gameObject);
	}
}
