using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Stat;
using Define;
using UnityEngine.UI;


public class UI_Deck : UI_Popup
{
    GameObject BigCard;
    GameObject CardText;


    GameObject _AddButtonCard;
    GameObject _AddBigCard;



    List<GameObject> BigCardList = new List<GameObject>();
    TextMeshProUGUI CardInfoText;
    List<UI_Card> UIcardList = new List<UI_Card>();

    int BeforeNum;


    public override void Init()
    {
        BaseCard.ExportMyDeck();

        BigCard = transform.GetChild(2).gameObject;
        CardText = transform.GetChild(3).gameObject;

        CardInfoText = CardText.GetComponentInChildren<TextMeshProUGUI>();
        MakeBigcardList();
    }

    public void OnEnable()
    {
        AddCard();
    }

    void AddCard()
    {
        for (int i = 0; i < BaseCard._MyDeck.Count; i++)
        {
            Debug.Log(BaseCard._MyDeck[i]);

            if (BigCardList.Count < BaseCard._MyDeck.Count && i + 1 == BaseCard._MyDeck.Count)
            {
                Transform buttonTrans = GameObject.Find($"Card_{BigCardList.Count}").transform;
                _AddButtonCard = Managers.Resource.Instantiate($"Cards/{BaseCard._MyDeck[i]}", buttonTrans);
                _AddButtonCard.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);

                _AddBigCard = Managers.Resource.Instantiate($"Cards/{BaseCard._MyDeck[i]}", BigCard.transform);
                _AddBigCard.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
                _AddBigCard.GetComponent<RectTransform>().localPosition = new Vector3(-225, 100, 0);
                BigCardList.Add(_AddBigCard);
                _AddBigCard.SetActive(false);
            }
        }
    }

    void MakeBigcardList()
    {
        for (int i = 0; i < BaseCard._MyDeck.Count; i++)
        {
            GameObject nowCard = BigCard.transform.GetChild(i).gameObject;
            UIcardList.Add(nowCard.GetComponent<UI_Card>());
            BigCardList.Add(nowCard);

            if (i == 0)
            {
                Cardinfo("JobCard_Grenade");
                continue;
            }

            BigCardList[i].SetActive(false);
        }
    }


    public void CardPush(int num = default)
    {
        if (BeforeNum != num)
        {
            BigCardList[num].SetActive(true);
            Cardinfo(BigCardList[num].name);
            BigCardList[BeforeNum].SetActive(false);
            BeforeNum = num;
        }
    }

    public void Cardinfo(string cardname)
    {
        switch (cardname)
        {
            case "JobCard_Grenade":
                CardInfoText.text = "수류탄을 던집니다. 이 카드는 고정입니다.";
                break;

            case "Card_AmuletOfSteel":
                CardInfoText.text = "팀원 전체에게 방어막을 부여합니다.";
                break;

            case "Card_Armor":
                CardInfoText.text = "방어력이 증가합니다.";
                break;

            case "Card_BloodTransfusion":
                CardInfoText.text = "적의 피를 수혈합니다. 체력의 일부를 가져갑니다.";
                break;

            case "Card_Cannon":
                CardInfoText.text = "대포를 발사하여 적에게 피해를 줍니다";
                break;

            case "Card_CrisisAversion":
                CardInfoText.text = "죽음을 불사하고 잠시동안 큰 버프를 얻습니다. 이후 버프는 사라집니다";
                break;

            case "Card_Crystal":
                CardInfoText.text = "마나 수정을 한개 회복합니다.";
                break;

            case "Card_HealthPotion ":
                CardInfoText.text = "체력의 일부를 회복합니다.";
                break;

            case "Card_HackingGrenade":
                CardInfoText.text = "적에게 해킹 수류탄을 던지고 맞은 적은 잠시동안 마나수정이 회복불가합니다.";
                break;

            case "Card_HealthKit":
                CardInfoText.text = "체력이 회복되고 최대 체력이 영구적으로 증가합니다.";
                break;

            case "Card_InvincibleShield":
                CardInfoText.text = "팀원 전체에게 무적을 부여하고 무적이 끝나면 방어막과 체력이 회복됩니다.";
                break;

            case "Card_InvincibleWeapon":
                CardInfoText.text = "비장의 얼음무기를 발사하여 얼음 주위에 있는 적이 큰 데미지를 얻습니다.";
                break;

            case "Card_Lava":
                CardInfoText.text = "용암을 뿌려 용암 위에 있는 적이 데미지를 입습니다.";
                break;

            case "Card_Shield":
                CardInfoText.text = "자신에게 방어막을 잠시동안 부여합니다.";
                break;

            case "Card_Spear":
                CardInfoText.text = "표창을 던져 적에게 데미지를 입힙니다.";
                break;

            case "Card_Speed":
                CardInfoText.text = "잠시동안 이동속도가 빨라집니다.";
                break;

            case "Card_Strike":
                CardInfoText.text = "강한 무기를 던져 맞은 적은 데미지 피해와 이동속도가 느려집니다.";
                break;

            case "Card_Purify":
                CardInfoText.text = "디버프가 사라집니다.";
                break;

            case "card_Teleport":
                CardInfoText.text = "짧은 거리를 빠르게 이동합니다.";
                break;

            case "Card_IcePrison":
                CardInfoText.text = "자기 자신에게 얼음 감옥을 시전해 모든 공격을 잠시동안 피합니다";
                break;

            case "Card_Infection":
                CardInfoText.text = "독 장판을 깔아 장판 위에 적에게 독 디버프를 입힙니다.";
                break;

            case "Card_RadiantCrystal":
                CardInfoText.text = "마나 수정을 전부 회복합니다.";
                break;

            case "Card_Resurrection":
                CardInfoText.text = "죽었을 시 부활합니다.";
                break;

            case "Card_WingsOfTheBattlefield":
                CardInfoText.text = "팀원 전체에게 이동 속도를 부여합니다.";
                break;

            case "Card_BloodstainedCoin":
                CardInfoText.text = "피 뭍은 동전을 던져 적을 맞출때 마다 동전을 얻고 데미지를 입힙니다.";
                break;

            case "Card_Enhancement":
                CardInfoText.text = "자신에게 잠시동안 무기를 강화하여 공격력을 증가시킵니다.";
                break;
        }
    }

}
