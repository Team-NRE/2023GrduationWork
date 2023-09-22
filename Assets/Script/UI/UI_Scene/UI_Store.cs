using UnityEngine;

public class UI_Store : UI_Popup
{
    [SerializeField] GameObject store;
    [SerializeField] GameObject strengthen;
    [SerializeField] GameObject delete;

    public enum StoreUI
    {
        store,
        strengthen,
        delete,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(StoreUI));
        store      = Get<GameObject>((int)StoreUI.store);
        strengthen = Get<GameObject>((int)StoreUI.strengthen);
        delete     = Get<GameObject>((int)StoreUI.delete);

        store      .SetActive(true);
        strengthen .SetActive(false);
        delete     .SetActive(false);
    }

    public void storeActive()
    {
        if (store.activeSelf) return;
        store      .SetActive(true);
        strengthen .SetActive(false);
        delete     .SetActive(false);
    }

    public void strengthenActive()
    {
        if (strengthen.activeSelf) return;
        store      .SetActive(false);
        strengthen .SetActive(true);
        delete     .SetActive(false);
    }

    public void deleteActive()
    {
        if (delete.activeSelf) return;
        store      .SetActive(false);
        strengthen .SetActive(false);
        delete     .SetActive(true);
    }
}
