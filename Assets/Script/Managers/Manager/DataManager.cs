using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<string, Data.Stat> StatDict { get; private set; } = new Dictionary<string, Data.Stat>();
    public Dictionary<string, Data.Card> CardDict { get; private set; } = new Dictionary<string, Data.Card>();
    public Dictionary<int, Data.Deck> DeckDict { get; private set; } = new Dictionary<int, Data.Deck>();

    public void Init()
    {
		StatDict = LoadJson<Data.StatData, string, Data.Stat>("StatData").MakeDict();
        CardDict = LoadJson<Data.CardData, string, Data.Card>("CardData").MakeDict();
        DeckDict = LoadJson<Data.DeckData, int, Data.Deck>("DeckData").MakeDict();
	}

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        //Debug.Log(textAsset.text);
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
