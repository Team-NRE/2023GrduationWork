using UnityEngine;

public class UI_Store : UI_Popup
{
    [SerializeField] GameObject store;
    [SerializeField] GameObject strengthen;
    [SerializeField] GameObject delete;

    public enum StoreUI
    {
        store,
        // strengthen,
        delete,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(StoreUI));
        store      = Get<GameObject>((int)StoreUI.store);
        // strengthen = Get<GameObject>((int)StoreUI.strengthen);
        delete     = Get<GameObject>((int)StoreUI.delete);

        store      .SetActive(true);
        // strengthen .SetActive(false);
        delete     .SetActive(false);
    }

    public void storeActive()
    {
        if (store.activeSelf) return;
        store      .SetActive(true);
        // strengthen .SetActive(false);
        delete     .SetActive(false);

        Managers.Sound.Play($"UI_ButtonBeep/UI_ButtonBeep_{Random.Range(1, 6)}", Define.Sound.Effect, 1, .5f);
    }

    public void strengthenActive()
    {
        if (strengthen.activeSelf) return;
        store      .SetActive(false);
        // strengthen .SetActive(true);
        delete     .SetActive(false);

        Managers.Sound.Play($"UI_ButtonBeep/UI_ButtonBeep_{Random.Range(1, 6)}", Define.Sound.Effect, 1, .5f);
    }

    public void deleteActive()
    {
        if (delete.activeSelf) return;
        store      .SetActive(false);
        // strengthen .SetActive(false);
        delete     .SetActive(true);

        Managers.Sound.Play($"UI_ButtonBeep/UI_ButtonBeep_{Random.Range(1, 6)}", Define.Sound.Effect, 1, .5f);
    }
}
