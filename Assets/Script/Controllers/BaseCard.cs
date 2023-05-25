using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class BaseCard
{
	public static Action<Define.KeyboardEvent> cardAction = null;
	public static List<string> _initDeck = new List<string>();
	public static List<string> _inHand = new List<string>();
	public static List<string> _AddOns = new List<string>();

	//Json으로 덱을 가져온다. 나중에 덱 숫자가 늘어나면 파라미터로 입력
	public static List<string> LoadDeck(int deckNum = 0)
	{
		Dictionary<int, Data.Deck> deck = Managers.Data.DeckDict;
		List<string> cardNames = new List<string>();

		for(int i = 0; i < deck[deckNum].cards.Count; i++)
		{
			//cardNames[i] = deck[deckNum].cards[i];
			cardNames.Add(deck[deckNum].cards[i]);
			//Debug.Log(deck[deckNum].cards[i]);
		}
		return cardNames;
	}

	//가져온 이름으로 List를 채운다
	public static List<string> ExportDeck()
	{
		return _initDeck = LoadDeck();
	}

	//카드 사용
	public static int UseCard(string card)
	{
		//외부에서 이름을 받는다.
		_initDeck.Remove(card);
		Debug.Log("_initDeck : " + _initDeck.Count);
		//덱에 남은 카드의 숫자를 찾는다.
		if (_initDeck.Count == 0)
		{
			Debug.Log("_initDeck is Empty");
			return 0;
		}
		//카드가 남아 있다면 랜덤으로 뽑아서 처리
		int rand = UnityEngine.Random.Range(0, _initDeck.Count);
		return rand;
	}

	//키보드 이벤트를 받아서 카드를 사용
	public static int UseCard(Define.KeyboardEvent evt)
	{
		//이벤트를 받으면 -> 

		int rand = UnityEngine.Random.Range(0, _initDeck.Count);
		return rand;
	}

	//초기 4장 선정
	//1. 선정된 카드를 리스트에서 지운다
	//2. 초기 4장을 인스턴스 한다. 한번 사용하고 그 뒤로는 사용되지 않는다.
	public static int StartDeck()
	{
		int rand = UnityEngine.Random.Range(0, _initDeck.Count - 1);
		//Managers.Resource.Instantiate()
		_initDeck.RemoveAt(rand);

		Debug.Log("남은 카드 덱 : " + _initDeck.Count + ", 초기호출 인덱스 : " + rand);
		return rand;
	}

	//다쓰면 다시 채운다, 비었는지 여부는 UI 이벤트 단에서 바꾼다.
	public static List<string> ReloadDeck()
	{
		Debug.Log("Reload call");
		//비었으니 기본 덱으로 채운다
		ExportDeck();
		foreach(string name in _initDeck)
		{
			Debug.Log("reload : " + name);
		}

		//만약 AddCard로 추가된 카드가 있다면
		if (_AddOns == null)
			Debug.Log("추가카드 없음");
		else
		{
			foreach (string name in _AddOns)
			{
				_initDeck.Add(name);
				Debug.Log($"Adds are : {name}");
			}
		}

		return _initDeck;
	}

	//새로운 카드를 추가해서 덱을 새로 만드는 카드
	public static List<string> AddCard(string name)
	{
		List<string> newDeck = new List<string>();

		_initDeck.Add(name);

		return newDeck;
	}
}
