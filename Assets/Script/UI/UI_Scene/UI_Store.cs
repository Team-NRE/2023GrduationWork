using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Store : UI_Popup
{
    GameObject store;
    GameObject strengthen;
    GameObject delete;

    public enum StoreUI
    {
        store,
        strengthen,
        delete,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(StoreUI));
        store = Get<GameObject>((int)StoreUI.store);
        strengthen = Get<GameObject>((int)StoreUI.strengthen);
        delete = Get<GameObject>((int)StoreUI.delete);

        store.SetActive(true);
        strengthen.SetActive(false);
        delete.SetActive(false);
    }

    public void storeActive()
    {
        store.SetActive(true);
        strengthen.SetActive(false);
        delete.SetActive(false);
    }

    public void strengthenActive()
    {
        store.SetActive(false);
        strengthen.SetActive(true);
        delete.SetActive(false);
    }

    public void deleteActive()
    {
        store.SetActive(false);
        strengthen.SetActive(false);
        delete.SetActive(true);
    }
}
