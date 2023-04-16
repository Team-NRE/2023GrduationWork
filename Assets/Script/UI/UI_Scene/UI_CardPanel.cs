﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CardPanel : UI_Card
{
	GameObject Q_Btn;
	GameObject W_Btn;
	GameObject E_Btn;
	GameObject R_Btn;

	public enum CardObjects
	{
		Panel,
	}

	public enum Cards
	{
		Q,
		W,
		E,
		R, 
	}

	public override void Init()
	{
		base.Init();

		//Find and Bind UI object
		//Bind<GameObject>(typeof(CardObjects));
		Bind<GameObject>(typeof(Cards));
		Q_Btn = Get<GameObject>((int)Cards.Q);
		W_Btn = Get<GameObject>((int)Cards.W);
		E_Btn = Get<GameObject>((int)Cards.E);
		R_Btn = Get<GameObject>((int)Cards.R);

		//여기서 이제 UI를 자식객체로 넣어준다.
		GameObject Q_Card = Managers.Resource.Instantiate($"Cards/{_initDeck[0]}", Q_Btn.transform);
		GameObject W_Card = Managers.Resource.Instantiate($"Cards/{_initDeck[1]}", W_Btn.transform);
		GameObject E_Card = Managers.Resource.Instantiate($"Cards/{_initDeck[2]}", E_Btn.transform);
		GameObject R_Card = Managers.Resource.Instantiate($"Cards/{_initDeck[3]}", R_Btn.transform);

		BindEvent(Q_Card, (PointerEventData data) => { Q_Btn.GetComponentInChildren<BaseController>().Init(); });
		//Debug.Log(_initDeck[0]);
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	//if click the card btn
	//parameter
	/*public List<string> UseCard(string cardName)
	{
		List<string> updatedList = new List<string>();
		//if use -> call Card init -> delete from deck

		//Add to empty with Random 1 from deck

		return updatedList;
	}*/

	public void UI_UseQ(PointerEventData data)
	{
		Debug.Log("Q");
		Q_Btn.GetComponentInChildren<BaseController>().Init();
	}

	public void UI_UseW(PointerEventData data)
	{
		Debug.Log("W");
		W_Btn.GetComponentInChildren<BaseController>().Init();
	}

	public void UI_UseE(PointerEventData data)
	{
		Debug.Log("E");
		E_Btn.GetComponentInChildren<BaseController>().Init();
	}

	public void UI_UseR(PointerEventData data)
	{
		Debug.Log("R");
		R_Btn.GetComponentInChildren<BaseController>().Init();
	}
}