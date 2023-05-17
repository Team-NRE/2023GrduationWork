using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BaseCard
{
	public static List<string> _initDeck = new List<string>();
	public static List<string> _inHand = new List<string>();

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

	//초기 4장 선정
	//1. 선정된 카드를 리스트에서 지운다
	//2. 초기 4장을 인스턴스 한다. 한번 사용하고 그 뒤로는 사용되지 않는다.
	public static int StartDeck()
	{
		int rand = UnityEngine.Random.Range(0, _initDeck.Count);
		//Managers.Resource.Instantiate()
		_initDeck.RemoveAt(rand);
		//for(int i = 0; i < _initDeck.Count-1; i++)
		//{
		//	Debug.Log(_initDeck[i]);
		//}
		Debug.Log("남은 카드 덱 : " + _initDeck.Count + ", 초기호출 인덱스 : " + rand);
		return rand;
	}
}
