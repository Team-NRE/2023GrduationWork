using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Card : UI_Scene
{
	public List<string> _initDeck = new List<string>();
	public override void Init()
	{
		Debug.Log("UI_Card Init");
		_initDeck = GetDeckBase(CardDictionary());
	}

	public virtual void InitCard()
	{

	}

	//Deck Initializer, return List<string> names, called only Deck is refreshed or starting game, ?꾩꽦
	public List<string> GetDeckBase(List<UI_Card> originals)
	{
		Dictionary<int, Data.Deck> dict = Managers.Data.DeckDict;
		//Return to InhandDeck
		List<string> cardNames = new List<string>();

		//Init deck all -> make List, that have all of deck
		for (int i = 0; i < originals.Count; i++)
		{
			cardNames.Add(originals[i].name); 
			//Debug.Log(cardNames[i]);        
		}

		//Pick random 4
		for (int i = 0; i < originals.Count; i++)
		{
			cardNames[i] = originals[i].name;
			//Debug.Log(cardNames[i].ToString());
		}

		return cardNames;
	}

	//parameter
	public List<string> UseCard(string cardName)
	{
		List<string> updatedList = new List<string>();
		

		return updatedList;
	}

	//Base return Components List for basement of search
	//string 由ъ뒪??諛섑솚???꾪븳 ?댁슜
	public List<UI_Card> CardDictionary()
	{
		Dictionary<string, Data.Card> cards = Managers.Data.CardDict;
		List<UI_Card> bcs = new List<UI_Card>();
		List<string> cardNames = new List<string>();

		foreach (Data.Card card in cards.Values)
		{
			//Debug.Log(card.name);
			UI_Card go = Managers.Resource.Load<UI_Card>($"Prefabs/Cards/{card.name}").GetComponent<UI_Card>();

			cardNames.Add(card.name);
			if (go.name == card.name)
			{
				bcs.Add(Managers.Resource.Load<UI_Card>($"Prefabs/Cards/{card.name}"));

				//Debug.Log(Managers.Resource.Load<GameObject>($"Prefabs/Cards/{card.name}"));
			}
		}

		return bcs;
	}

}
