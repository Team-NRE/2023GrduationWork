using System;
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

	GameObject Q_Card;
	GameObject W_Card;
	GameObject E_Card;
	GameObject R_Card;

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
		//나중에 덱이 늘어나면 여기에 파라미터로 덱 아이디를 전달
		BaseCard.ExportDeck();
		
		//Find and Bind UI object
		Bind<GameObject>(typeof(Cards));
		Q_Btn = Get<GameObject>((int)Cards.Q);
		W_Btn = Get<GameObject>((int)Cards.W);
		E_Btn = Get<GameObject>((int)Cards.E);
		R_Btn = Get<GameObject>((int)Cards.R);

		DeckStart();

		BindEvent(Q_Card, (PointerEventData data) => { UI_UseQ(data); });
		BindEvent(W_Card, (PointerEventData data) => { UI_UseW(data); });
		BindEvent(E_Card, (PointerEventData data) => { UI_UseE(data); });
		BindEvent(R_Card, (PointerEventData data) => { UI_UseR(data); });
	}

	public void DeckStart()
	{
		//여기서 이제 UI를 자식객체로 넣어준다.
		Q_Card = Managers.Resource.Instantiate($"Cards/{BaseCard._initDeck[BaseCard.StartDeck()]}", Q_Btn.transform);
		W_Card = Managers.Resource.Instantiate($"Cards/{BaseCard._initDeck[BaseCard.StartDeck()]}", W_Btn.transform);
		E_Card = Managers.Resource.Instantiate($"Cards/{BaseCard._initDeck[BaseCard.StartDeck()]}", E_Btn.transform);
		R_Card = Managers.Resource.Instantiate($"Cards/{BaseCard._initDeck[BaseCard.StartDeck()]}", R_Btn.transform);

		BaseCard._inHand = 
	}

	//0번 인덱스의 리스트를 반드시 사용한다.
	public void UI_UseQ(PointerEventData data)
	{
		Q_Btn.GetComponentInChildren<UI_Card>().InitCard();
		int useId = BaseCard.UseCard(Q_Btn.transform.GetChild(0).name);
		Q_Btn.GetComponentInChildren<UI_Card>().DestroyCard();
		Debug.Log(Q_Btn.transform.GetChild(0).name);
		if (useId != 0)
		{
			Managers.Resource.Instantiate($"Cards/{BaseCard._initDeck[useId]}", Q_Btn.transform);
			Debug.Log(useId);
		}
		BindEvent(Q_Card, (PointerEventData data) => { UI_UseQ(data); });
	}

	//1번 인덱스의 리스트를 반드시 사용한다.
	public void UI_UseW(PointerEventData data)
	{
		W_Btn.GetComponentInChildren<UI_Card>().InitCard();
		int useId = BaseCard.UseCard(Q_Btn.transform.GetChild(0).name);
		W_Btn.GetComponentInChildren<UI_Card>().DestroyCard();
		Debug.Log(W_Btn.transform.GetChild(0).name);
		if (useId != 0)
		{
			Managers.Resource.Instantiate($"Cards/{BaseCard._initDeck[useId]}", W_Btn.transform);
			Debug.Log(useId);
		}
		BindEvent(W_Card, (PointerEventData data) => { UI_UseW(data); });
	}

	//2번 인덱스의 리스트를 반드시 사용한다.
	public void UI_UseE(PointerEventData data)
	{
		E_Btn.GetComponentInChildren<UI_Card>().InitCard();
		int useId = BaseCard.UseCard(E_Btn.transform.GetChild(0).name);
		E_Btn.GetComponentInChildren<UI_Card>().DestroyCard();
		Debug.Log(E_Btn.transform.GetChild(0).name);
		if (useId != 0)
		{
			Managers.Resource.Instantiate($"Cards/{BaseCard._initDeck[useId]}", E_Btn.transform);
			Debug.Log(useId);
		}

		BindEvent(E_Card, (PointerEventData data) => { UI_UseE(data); });
	}

	public void UI_UseR(PointerEventData data)
	{
		R_Btn.GetComponentInChildren<UI_Card>().InitCard();
		int useId = BaseCard.UseCard(R_Btn.transform.GetChild(0).name);
		R_Btn.GetComponentInChildren<UI_Card>().DestroyCard();
		Debug.Log(R_Btn.transform.GetChild(0).name);
		if (useId != 0)
		{
			Managers.Resource.Instantiate($"Cards/{BaseCard._initDeck[useId]}", R_Btn.transform);
			Debug.Log(useId);
		}
		BindEvent(R_Card, (PointerEventData data) => { UI_UseR(data); });
	}
}
