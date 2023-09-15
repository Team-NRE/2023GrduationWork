using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Stat;
using Define;
using UnityEngine.UI;


public class UI_Deck : UI_Popup
{
    Transform _BigCard;
    Transform _ScrollMyCard;
    
    GameObject _CardText;

    List<GameObject> _makeBigCardList = new List<GameObject>();

    TextMeshProUGUI _CardInfoText;

    int _BeforeNum;


    public override void Init()
    {
        BaseCard.ExportMyDeck((int)Managers.game.myCharacterType);

        _BigCard = transform.GetChild(2);
        _CardText = transform.GetChild(3).gameObject;
        _ScrollMyCard = GameObject.Find("DeckContent").transform;

        _CardInfoText = _CardText.GetComponentInChildren<TextMeshProUGUI>();

        //초기 세팅
        MakeUI_Mycard();
    }

    public void OnEnable()
    {
        //BaseCard.ExportMyDeck();

        //이후 덱을 열었을 때
        MakeUI_Mycard();
    }

    //내 덱 UI로 만들어주기
    void MakeUI_Mycard()
    {
        //내 덱의 카드가 없다면 return
        if (BaseCard._MyDeck.Count == 0) { return; }
        //나만의 덱의 큰 카드 리셋 
        if (_makeBigCardList.Count > 0) { _makeBigCardList.Clear(); }

        for (int i = 0; i < BaseCard._MyDeck.Count; i++)
        {
            //나만의 덱의 큰 카드 세팅
            _makeBigCardList.Add(Managers.Resource.Instantiate($"Cards/{BaseCard._MyDeck[i]}", _BigCard));
            _makeBigCardList[i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
            _makeBigCardList[i].GetComponent<RectTransform>().localPosition = new Vector3(-225, 100, 0);

            //나만의 덱의 스크롤 카드 세팅
            GameObject makeScrollCard = Managers.Resource.Instantiate($"Cards/{BaseCard._MyDeck[i]}", _ScrollMyCard.GetChild(i));
            makeScrollCard.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);

            //Job카드는 UI 위치 고정
            if (i == 0)
            {
                Cardinfo(BaseCard._MyDeck[i]);
                continue;
            }

            _makeBigCardList[i].SetActive(false);
        }
    }

    //스크롤 카드 클릭 시
    public void ScrollCardClick(int num = default)
    {
        if (_BeforeNum != num)
        {
            //클릭한 카드 BigCard로 띄워주기
            _makeBigCardList[num].SetActive(true);
            Cardinfo(_makeBigCardList[num].name);

            //이전에 켜져잇던 BigCard 끄기
            _makeBigCardList[_BeforeNum].SetActive(false);
            _BeforeNum = num;
        }
    }

    //카드 설명
    public void Cardinfo(string cardname)
    {
        switch (cardname)
        {
            case "JobCard_Grenade":
                _CardInfoText.text = "수류탄을 던집니다. 이 카드는 고정입니다.";
                break;

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
