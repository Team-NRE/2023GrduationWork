using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_NextCard : UI_Scene
{
	public enum Buttons
	{
		NextCard,
	}

	public override void Init()
	{
		base.Init();
		Bind<Button>(typeof(Buttons));

		GetButton((int)Buttons.NextCard).gameObject.BindEvent(NextCardCall);
	}

	public void NextCardCall(PointerEventData data)
	{
		Debug.Log("Event has binded : nextCard");
	}
}
