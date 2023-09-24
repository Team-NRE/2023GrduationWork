using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Stat;
using Define;
using UnityEngine.UI;

public class store : UI_Store
{
    enum GameObjects
    {
        Big_card,
        toggleBasic,
    }

    enum ToggleGroups
    {
        StoreContent,
    }

    enum Texts
    {
        Coin_Text,
        Price_Text,
        Card_Text,
    }

    enum Buttons
    {
        Store_Buy,
    }

    Dictionary<string, GameObject> _makeAllBigCardList = new Dictionary<string, GameObject>();

    int _BuyCost;

    PlayerStats _pStat;

    float lastCoin;

    public override void Init()
    {
        BaseCard.ExportPublicCard();
        BaseCard.ExportJobCard();
        
        _pStat = Managers.game.myCharacter.GetComponent<PlayerStats>();

        Bind<GameObject>      (typeof(GameObjects));
        Bind<ToggleGroup>     (typeof(ToggleGroups));
        Bind<TextMeshProUGUI> (typeof(Texts));
        Bind<Button>          (typeof(Buttons));

        Get<GameObject>((int)GameObjects.toggleBasic).SetActive(false);

        MakeUI_AllBigcard();

        lastCoin = _pStat.gold;

        Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "";
        Get<TextMeshProUGUI>((int)Texts.Price_Text).text = "0";
        Get<TextMeshProUGUI>((int)Texts.Coin_Text).text = $"{((int)lastCoin).ToString()}";
    }

    void MakeUI_AllBigcard()
    {
        // 폴더에 카드가 없다면 return
        if (BaseCard._AllPublicCard.Count == 0) { return; }

        for (int i = 0; i < BaseCard._AllPublicCard.Count; i++)
        {
            //상점의 큰 카드 세팅
            _makeAllBigCardList.Add(BaseCard._AllPublicCard[i], Managers.Resource.Instantiate($"Cards/{BaseCard._AllPublicCard[i]}", Get<GameObject>((int)GameObjects.Big_card).transform));
            _makeAllBigCardList[BaseCard._AllPublicCard[i]].GetComponent<RectTransform>().localScale = new Vector3(2f, 2f, 0);
            _makeAllBigCardList[BaseCard._AllPublicCard[i]].SetActive(false);

            //상점의 스크롤 카드 세팅
            GameObject newToggleCard = Instantiate(Get<GameObject>((int)GameObjects.toggleBasic));
            newToggleCard.transform.SetParent(Get<ToggleGroup>((int)ToggleGroups.StoreContent).transform);
            newToggleCard.GetComponent<RectTransform>().localScale = Vector3.one;
            newToggleCard.GetComponent<Toggle>().onValueChanged.AddListener(delegate { CardInfoChange(newToggleCard.GetComponent<Toggle>()); });
            newToggleCard.name = BaseCard._AllPublicCard[i];
            newToggleCard.SetActive(true);

            GameObject makeScrollCard = Managers.Resource.Instantiate($"Cards/{BaseCard._AllPublicCard[i]}", newToggleCard.transform);
            makeScrollCard.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 0);
        }
    }

    void Update()
    {
        if (lastCoin != _pStat.gold)
        {
            lastCoin = Mathf.Lerp(lastCoin, (int)_pStat.gold, 10f * Time.deltaTime);
            Get<TextMeshProUGUI>((int)Texts.Coin_Text).text = $"{((int)lastCoin).ToString()}";
        }
        
    }

    // 스크롤 카드 선택 시
    public void CardInfoChange(Toggle toggle)
    {
        _makeAllBigCardList[toggle.name].SetActive(toggle.isOn);
        _BuyCost = toggle.GetComponentInChildren<UI_Card>()._cardBuyCost;
        Get<TextMeshProUGUI>((int)Texts.Price_Text).text = _BuyCost.ToString();
        Cardinfo(toggle.name);
    }

    public void CardBuy()
    {
        if (_pStat.gold >= _BuyCost)
        {
            BaseCard._initDeck.Add(Get<ToggleGroup>((int)ToggleGroups.StoreContent).GetFirstActiveToggle().name);
            BaseCard._MyDeck  .Add(Get<ToggleGroup>((int)ToggleGroups.StoreContent).GetFirstActiveToggle().name);
            
            _pStat.gold -= _BuyCost;
        }

        else
        {
            Debug.Log("카드 구입 실패");
        }
    }

    public void Cardinfo(string cardname)
    {
        switch (cardname)
        {
            case "Card_AmuletOfSteel":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "팀원 전체에게 방어막을 부여합니다.";
                break;

            case "Card_Armor":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "방어력이 증가합니다.";
                break;

            case "Card_BloodTransfusion":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "적의 피를 수혈합니다. 체력의 일부를 가져갑니다.";
                break;

            case "Card_Cannon":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "대포를 발사하여 적에게 피해를 줍니다";
                break;

            case "Card_CrisisAversion":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "죽음을 불사하고 잠시동안 큰 버프를 얻습니다. 이후 버프는 사라집니다";
                break;

            case "Card_Crystal":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "마나 수정을 한개 회복합니다.";
                break;

            case "Card_HealthPotion":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "체력의 일부를 회복합니다.";
                break;

            case "Card_HackingGrenade":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "적에게 해킹 수류탄을 던지고 맞은 적은 잠시동안 마나수정이 회복불가합니다.";
                break;

            case "Card_HealthKit":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "체력이 회복되고 최대 체력이 영구적으로 증가합니다.";
                break;

            case "Card_InvincibleShield":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "팀원 전체에게 무적을 부여하고 무적이 끝나면 방어막과 체력이 회복됩니다.";
                break;

            case "Card_InvincibleWeapon":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "비장의 얼음무기를 발사하여 얼음 주위에 있는 적이 큰 데미지를 얻습니다.";
                break;

            case "Card_Lava":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "용암을 뿌려 용암 위에 있는 적이 데미지를 입습니다.";
                break;

            case "Card_Shield":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "자신에게 방어막을 잠시동안 부여합니다.";
                break;

            case "Card_Spear":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "표창을 던져 적에게 데미지를 입힙니다.";
                break;
                
            case "Card_Speed":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "잠시동안 이동속도가 빨라집니다.";
                break;

            case "Card_Strike":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "강한 무기를 던져 맞은 적은 데미지 피해와 이동속도가 느려집니다.";
                break;

            case "Card_Purify":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "디버프가 사라집니다.";
                break;

            case "Card_Teleport":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "짧은 거리를 빠르게 이동합니다.";
                break;

            case "Card_IcePrison":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "자기 자신에게 얼음 감옥을 시전해 모든 공격을 잠시동안 피합니다";
                break;

            case "Card_Infection":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "독 장판을 깔아 장판 위에 적에게 독 디버프를 입힙니다.";
                break;

            case "Card_RadiantCrystal":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "마나 수정을 전부 회복합니다.";
                break;

            case "Card_Resurrection":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "죽었을 시 부활합니다.";
                break;

            case "Card_WingsOfTheBattlefield":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "팀원 전체에게 이동 속도를 부여합니다.";
                break;

            case "Card_BloodstainedCoin":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "피 뭍은 동전을 던져 적을 맞출때 마다 동전을 얻고 데미지를 입힙니다.";
                break;

            case "Card_Enhancement":
                Get<TextMeshProUGUI>((int)Texts.Card_Text).text = "자신에게 잠시동안 무기를 강화하여 공격력을 증가시킵니다.";
                
                break;
        }
    }

}
