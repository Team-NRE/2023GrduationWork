/// ksPark
///
/// In Game 하위 체력바

using UnityEngine;
using TMPro;
using Stat;
using UnityEngine.UI;
using System.Collections;

public class UI_StatusBar : MonoBehaviour
{
    // myCharacter PlayerStat
    PlayerStats myStat;
    
    // cam
    Camera iconCamera;
    string camName = "IconCamera";

    // levelText
    TextMeshProUGUI levelText;
    string levelTextName = "Level";

    // HealthBar
    Image hpBarImage;
    Image hpBarYellowImage;
    string hpBarImageName = "Hp_Fill_Red";
    string hpBarYellowImageName = "Hp_Fill_Yellow";

    float lastHealth;
    public float lastDealTime = 0.0f;
    public float yellowDelay = .01f;

    // ManaBar
    Image[] manaBarImages = new Image[0];
    string manaBarImagesName = "Mana_Fill";

    float lastMana = 0f;

    // ExpBar
    Image expBarImage;
    string expBarImageName = "Exp_Fill";
    
    float lastExp = 0f;

    private void Awake()
    {
        GetLevelText();
        GetHpBarImage();
        GetExpBarImage();
    }

    void Update()
    {
        Init();
        Renew();
    }

    // 초기화
    void Init()
    {
        if (Managers.game.myCharacter == null) return;

        GetStat();
        GetCamera();
        GetManaBarImages();
    }

    // 갱신
    void Renew()
    {
        if (Managers.game.myCharacter == null) return;

        UpdateLevelText();
        UpdateHpBarImage();
        UpdateManaBarImages();
        UpdateExpBarImage();
    }

    void GetStat()
    {
        if (myStat != null) return;
        myStat = Managers.game.myCharacter.GetComponent<PlayerStats>();
        lastHealth = myStat.nowHealth;
    }

    void GetCamera()
    {
        if (iconCamera != null) return;
        iconCamera = GetObject<Camera>(Managers.game.myCharacter, camName);
        GetObject<SkinnedMeshRenderer>(Managers.game.myCharacter, "CharacterMesh").gameObject.layer = LayerMask.NameToLayer("CharacterMesh");
        if (iconCamera != null) iconCamera.gameObject.SetActive(true);
    }

    void GetLevelText()
    {
        if (levelText != null) return;
        levelText = GetObject<TextMeshProUGUI>(gameObject, levelTextName);
    }

    void GetHpBarImage()
    {
        if (hpBarImage != null) return;
        hpBarImage = GetObject<Image>(gameObject, hpBarImageName);

        if (hpBarYellowImage != null) return;
        hpBarYellowImage = GetObject<Image>(gameObject, hpBarYellowImageName);

        lastDealTime = 0.0f;
    }

    void GetManaBarImages()
    {
        if (manaBarImages.Length > 0) return;
        manaBarImages = new Image[(int)myStat.maxMana];
        for (int i=0; i<myStat.maxMana; i++)
            manaBarImages[i] = GetObject<Image>(gameObject, manaBarImagesName + i.ToString());

        lastMana = 0.1f;
    }

    void GetExpBarImage()
    {
        if (expBarImage != null) return;
        expBarImage = GetObject<Image>(gameObject, expBarImageName);

        lastExp = 0.1f;
    }

    void UpdateLevelText()
    {
        if (levelText == null) return;
        levelText.text = myStat.level.ToString();
    }

    void UpdateHpBarImage()
    {
        if (myStat.nowHealth == lastHealth) return;

        if (hpBarImage == null) return;
        hpBarImage.fillAmount = myStat.nowHealth / myStat.maxHealth;

        if (myStat.nowHealth < lastHealth)
        {
            if (lastDealTime > 0)
            {
                hpBarYellowImage.fillAmount = lastHealth / myStat.maxHealth;
            }

            lastDealTime = yellowDelay;
        }
        lastHealth = myStat.nowHealth;

        if (lastDealTime > 0) 
        {
            lastDealTime -= Time.deltaTime;
            return;
        }

        if (hpBarYellowImage == null) return;
        hpBarYellowImage.fillAmount = myStat.nowHealth / myStat.maxHealth;
    }

    void UpdateManaBarImages()
    {
        if (myStat.nowMana == lastMana) return;

        if (manaBarImages == null) return;
        if (manaBarImages.Length == 0) return;
        lastMana = Mathf.Lerp(lastMana, myStat.nowMana, Time.deltaTime * 10.0f);

        for (int i=0; i<myStat.maxMana; i++)
        {
            if (manaBarImages[i] == null) continue;
            manaBarImages[i].fillAmount = (lastMana-i);
        }
    }

    void UpdateExpBarImage()
    {
        if (myStat.experience == lastExp) return;

        if (expBarImage == null) return;
        lastExp = Mathf.Lerp(lastExp, myStat.experience, Time.deltaTime * 7.5f);
        expBarImage.fillAmount = lastExp / myStat.levelUpEx;
    }

     T GetObject<T>(GameObject parent, string name)
    {
        if (parent == null) return default(T);
        if (name == null || name == "") return default(T);

        T[] objs = parent.GetComponentsInChildren<T>(true);

        if (objs == null) return default(T);

        for (int i=0; i<objs.Length; i++)
        {
            if (objs[i].ToString().Substring(0, name.Length).Equals(name))
            {
                return objs[i];
            }
        }

        return default(T);
    }
}
