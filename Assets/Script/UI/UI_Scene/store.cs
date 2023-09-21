using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Stat;
using Define;
using UnityEngine.UI;

public class store : UI_Store
{
    GameObject _MyCoin;
    GameObject _CardCoin;
    GameObject _CardText;
    GameObject _Buy;

    Transform _BigAllCard;
    Transform _ScrollAllCard;

    TextMeshProUGUI _MyCoinText;
    TextMeshProUGUI _CardCoinText;
    TextMeshProUGUI _CardInfoText;

    List<GameObject> _makeAllBigCardList = new List<GameObject>();

    int _BeforeStoreNum;

    int _BuyCost;

    PlayerStats _pStat;

    public override void Init()
    {
        BaseCard.ExportPublicCard();
        BaseCard.ExportJobCard();
        
        _pStat = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<PlayerStats>();
        _ScrollAllCard = GameObject.Find("StoreContent").transform;
        

        _MyCoin = transform.GetChild(2).gameObject;
        _CardCoin = transform.GetChild(3).gameObject;
        _CardText = transform.GetChild(4).gameObject;
        _BigAllCard = transform.GetChild(5);
        _Buy = transform.GetChild(6).gameObject;

        _MyCoinText = _MyCoin.GetComponentInChildren<TextMeshProUGUI>();
        _CardCoinText = _CardCoin.GetComponentInChildren<TextMeshProUGUI>();
        _CardInfoText = _CardText.GetComponentInChildren<TextMeshProUGUI>();


        MakeUI_AllBigcard();
    }

    void MakeUI_AllBigcard()
    {
        //내 덱의 카드가 없다면 return
        if (BaseCard._AllPublicCard.Count == 0) { return; }

        for (int i = 0; i < BaseCard._AllPublicCard.Count; i++)
        {
            //나만의 덱의 큰 카드 세팅
            _makeAllBigCardList.Add(Managers.Resource.Instantiate($"Cards/{BaseCard._AllPublicCard[i]}", _BigAllCard));
            _makeAllBigCardList[i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
            _makeAllBigCardList[i].GetComponent<RectTransform>().localPosition = new Vector3(-225, 100, 0);

            //나만의 덱의 스크롤 카드 세팅
            GameObject makeScrollCard = Managers.Resource.Instantiate($"Cards/{BaseCard._AllPublicCard[i]}", _ScrollAllCard.GetChild(i));
            makeScrollCard.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);


            //첫번째 카드는 항상 키기
            if (i == 0)
            {
                Cardinfo(BaseCard._AllPublicCard[i]);
                continue;
            }

            _makeAllBigCardList[i].SetActive(false);
        }
    }

    void Update()
    {
        // _MyCoinText.text = $"{_pStat.gold.ToString()}";

        // //카드 코스트 띄워주기
        // _BuyCost = _makeAllBigCardList[_BeforeStoreNum].GetComponent<UI_Card>()._cardBuyCost;
        // _CardCoinText.text = $"{_BuyCost.ToString()}";
    }

    //스크롤 카드 클릭 시
    public void ScrollCardClick(int num = default)
    {
        if (_BeforeStoreNum != num)
        {
            //클릭한 카드 BigCard로 띄워주기
            _makeAllBigCardList[num].SetActive(true);
            Cardinfo(_makeAllBigCardList[num].name);

            //이전에 켜져잇던 BigCard 끄기
            _makeAllBigCardList[_BeforeStoreNum].SetActive(false);
            _BeforeStoreNum = num;
        }
    }

    public void CardBuy()
    {
        if (_pStat.gold >= _BuyCost)
        {
            BaseCard._initDeck.Add(_makeAllBigCardList[_BeforeStoreNum].name);
            BaseCard._MyDeck.Add(_makeAllBigCardList[_BeforeStoreNum].name);
            
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
                _CardInfoText.text = "팀원 전체에게 방어막을 부여합니다.";
                break;

            case "Card_Armor":
                _CardInfoText.text = "방어력이 증가합니다.";
                break;

            case "Card_BloodTransfusion":
                _CardInfoText.text = "적의 피를 수혈합니다. 체력의 일부를 가져갑니다.";
                break;

            case "Card_Cannon":
                _CardInfoText.text = "대포를 발사하여 적에게 피해를 줍니다";
                break;

            case "Card_CrisisAversion":
                _CardInfoText.text = "죽음을 불사하고 잠시동안 큰 버프를 얻습니다. 이후 버프는 사라집니다";
                break;

            case "Card_Crystal":
                _CardInfoText.text = "마나 수정을 한개 회복합니다.";
                break;

            case "Card_HealthPotion ":
                _CardInfoText.text = "체력의 일부를 회복합니다.";
                break;

            case "Card_HackingGrenade":
                _CardInfoText.text = "적에게 해킹 수류탄을 던지고 맞은 적은 잠시동안 마나수정이 회복불가합니다.";
                break;

            case "Card_HealthKit":
                _CardInfoText.text = "체력이 회복되고 최대 체력이 영구적으로 증가합니다.";
                break;

            case "Card_InvincibleShield":
                _CardInfoText.text = "팀원 전체에게 무적을 부여하고 무적이 끝나면 방어막과 체력이 회복됩니다.";
                break;

            case "Card_InvincibleWeapon":
                _CardInfoText.text = "비장의 얼음무기를 발사하여 얼음 주위에 있는 적이 큰 데미지를 얻습니다.";
                break;

            case "Card_Lava":
                _CardInfoText.text = "용암을 뿌려 용암 위에 있는 적이 데미지를 입습니다.";
                break;

            case "Card_Shield":
                _CardInfoText.text = "자신에게 방어막을 잠시동안 부여합니다.";
                break;

            case "Card_Spear":
                _CardInfoText.text = "표창을 던져 적에게 데미지를 입힙니다.";
                break;
                
            case "Card_Speed":
                _CardInfoText.text = "잠시동안 이동속도가 빨라집니다.";
                break;

            case "Card_Strike":
                _CardInfoText.text = "강한 무기를 던져 맞은 적은 데미지 피해와 이동속도가 느려집니다.";
                break;

            case "Card_Purify":
                _CardInfoText.text = "디버프가 사라집니다.";
                break;

            case "card_Teleport":
                _CardInfoText.text = "짧은 거리를 빠르게 이동합니다.";
                break;

            case "Card_IcePrison":
                _CardInfoText.text = "자기 자신에게 얼음 감옥을 시전해 모든 공격을 잠시동안 피합니다";
                break;

            case "Card_Infection":
                _CardInfoText.text = "독 장판을 깔아 장판 위에 적에게 독 디버프를 입힙니다.";
                break;

            case "Card_RadiantCrystal":
                _CardInfoText.text = "마나 수정을 전부 회복합니다.";
                break;

            case "Card_Resurrection":
                _CardInfoText.text = "죽었을 시 부활합니다.";
                break;

            case "Card_WingsOfTheBattlefield":
                _CardInfoText.text = "팀원 전체에게 이동 속도를 부여합니다.";
                break;

            case "Card_BloodstainedCoin":
                _CardInfoText.text = "피 뭍은 동전을 던져 적을 맞출때 마다 동전을 얻고 데미지를 입힙니다.";
                break;

            case "Card_Enhancement":
                _CardInfoText.text = "자신에게 잠시동안 무기를 강화하여 공격력을 증가시킵니다.";
                
                break;
        }
    }

}
