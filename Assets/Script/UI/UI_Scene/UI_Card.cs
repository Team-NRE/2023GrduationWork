using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Card : UI_Scene
{
	//public BaseCard _baseCard = new BaseCard();

	public override void Init()
	{
		Debug.Log("UI_Card Init");
	}

	public virtual void InitCard()
	{
		//하위 카드 컴포넌트에서 구현하여 사용 위함
	}

	public virtual void DestroyCard()
	{
		//하위 카드 컴포넌트에서 구현하여 사용 위함
	}
}
