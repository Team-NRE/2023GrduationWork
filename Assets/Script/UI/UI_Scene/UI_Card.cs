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

	public List<string> UISetter()
	{
		return _initDeck;
	}

	//Deck Initializer, return List<string> names, called only Deck is refreshed or starting game, ?꾩꽦
	public List<string> GetDeckBase(List<BaseController> originals)
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
			Debug.Log(cardNames[i].ToString());
		}

		return cardNames;
	}

	//parameter
	public List<string> UseCard(string cardName)
	{
		List<string> updatedList = new List<string>();
		//if use -> call Card init -> delete from deck

		//Add to empty with Random 1 from deck

		return updatedList;
	}

	//Base return Components List for basement of search
	//string 由ъ뒪??諛섑솚???꾪븳 ?댁슜
	public List<BaseController> CardDictionary()
	{
		Dictionary<string, Data.Card> cards = Managers.Data.CardDict;
		List<BaseController> bcs = new List<BaseController>();
		List<string> cardNames = new List<string>();

		foreach (Data.Card card in cards.Values)
		{
			//Debug.Log(card.name);
			BaseController go = Managers.Resource.Load<BaseController>($"Prefabs/Cards/{card.name}").GetComponent<BaseController>();

			cardNames.Add(card.name);
			if (go.name == card.name)
			{
				bcs.Add(Managers.Resource.Load<BaseController>($"Prefabs/Cards/{card.name}"));

				//Debug.Log(Managers.Resource.Load<GameObject>($"Prefabs/Cards/{card.name}"));
			}
		}

		return bcs;
	}

}
