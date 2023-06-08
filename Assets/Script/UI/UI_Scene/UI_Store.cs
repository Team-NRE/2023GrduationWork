using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Store : UI_Popup
{
    GameObject store;
    GameObject strengthen;
    GameObject deck;

    public enum StoreUI
    {
        store,
        strengthen,
        deck,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(StoreUI));
        store = Get<GameObject>((int)StoreUI.store);
        strengthen = Get<GameObject>((int)StoreUI.strengthen);
        deck = Get<GameObject>((int)StoreUI.deck);

        store.SetActive(true);
        strengthen.SetActive(false);
        deck.SetActive(false);
    }

    public void storeActive()
    {
        store.SetActive(true);
        strengthen.SetActive(false);
        deck.SetActive(false);
    }

    public void strengthenActive()
    {
        store.SetActive(false);
        strengthen.SetActive(true);
        deck.SetActive(false);
    }

    public void deckActive()
    {
        store.SetActive(false);
        strengthen.SetActive(false);
        deck.SetActive(true);
    }
}
