using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("---Purchase---")]
    //구입 버튼
    public Button PurchaseButton;
	[Header("---Store---")]
    public bool CheckStoreImg = false;

	#region card 
    [Header("---Use Card Info---")]
    //카드 이펙트 판별 숫자
    public int CardNumber;
    //카드 마나 코스트
    public int CardCost;
    //카드 정보
    public GameObject card;
    //들고있는 카드
    public GameObject HoldCard;
	#endregion

	#region List
    [Header("---PlayerCard List---")]
    //플레이어 덱
    public List<GameObject> PlayerCardDeck = new List<GameObject>();
    //사용하고 난 버려진 카드 모음
    public List<GameObject> Discard = new List<GameObject>();
    //Q,W,E,R 카드 UI 위치 리스트
    public List<GameObject> SkillPosition = new List<GameObject>();

    [Header("---StoreCard List---")]
    //상점 카드들의 리스트
    public List<GameObject> StoreCardList = new List<GameObject>();

    [Header("---Mana List---")]
    public List<GameObject> ManaList = new List<GameObject>();
	#endregion

    public void Awake()
    {		
		//Setting.cs
        ObjectSetting("StoreImage"); //구입 버튼 
        ObjectSetting("PlayerCardUI"); //핸드 카드 List 초기 세팅
        ObjectSetting("Cards"); //상점 카드 List 세팅
        ObjectSetting("Mana"); //마나 List 시스템 세팅
		
		GameObject.Find("StoreImage").SetActive(false);
		
		//Card.cs
        CreateHoldCard(0); //들고있는 카드 초기 세팅
    }

	public void Update()
	{
		//Card.cs
        card.SetActive(false); //카드 정보 (수정해야함.)
		ManaPlay();
		CardCheck(); //버려진 카드 리셋
	}

	public GameObject GetStore(string StoreImg)
	{
		GameObject check = null;
		check = GameObject.Find(StoreImg);
        
        Debug.Log(check);
		if(CheckStoreImg == false || CheckStoreImg == true)
		{
			CheckStoreImg = !CheckStoreImg;
			check.transform.GetChild(0).gameObject.SetActive(CheckStoreImg);
		}

		return check;
	}

    //오브젝트 세팅
    public void ObjectSetting(string ObjectName)
    {
        Transform ObjectChild = GameObject.Find(ObjectName).transform;
        switch (ObjectName)
        {
            //구입버튼
            case "StoreImage":
                {
                    PurchaseButton = ObjectChild.GetChild(0).GetComponent<Button>();
                    PurchaseButton.onClick.AddListener(PurChase);

                    return;
                }

            //핸드 카드 List 초기 세팅
            case "PlayerCardUI":
                {
                    for (int i = 0; i < ObjectChild.childCount; i++)
                    {
                        //Plane - PlayerCard의 하위 객체 (Q,W,E,R,Next Card의 gameobject 리스트에 저장)
                        GameObject CardChild = ObjectChild.GetChild(i).gameObject;
                        SkillPosition.Add(CardChild);
                    }

                    return;
                }

            //상점 카드 세팅
            case "Cards":
                {
                    for (int i = 0; i < ObjectChild.childCount; i++)
                    {
                        //Plane - Store - StoreImg- Cards의 하위 객체
                        GameObject child = ObjectChild.GetChild(i).gameObject;
                        StoreCardList.Add(child);
                    }

                    return;
                }

            //마나시스템 세팅
            case "Mana":
                {
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject ManaChild = ObjectChild.GetChild(i).gameObject;
                        ManaList.Add(ManaChild);

                        ManaList[i].GetComponent<Slider>().minValue = 4 * i;
                        ManaList[i].GetComponent<Slider>().maxValue = 4 * (i + 1);
                    }

                    return;
                }
        }
    }

	//카드 구입
    public void PurChase()
    {
        //상점 카드 리스트 중 Toggle.isOn이 True인 경우만 PlayerCardDeck에 추가
        for (int i = 0; i < StoreCardList.Count; i++)
        {
            Toggle toggle = StoreCardList[i].GetComponent<Toggle>();

            if (toggle.isOn == true)
            {
                Debug.Log(StoreCardList[i] + "구입");

                PlayerCardDeck.Add(StoreCardList[i].gameObject);
            }

        }
    }

	//마나 플레이
    public void ManaPlay()
    {
        //gameManager.instance.player.NowMana = 마나회복시간 * 최대 마나
        gameManager.instance.player.NowMana += Time.deltaTime;

        //마나 회복 (한칸 차면 다음 칸 참)
        for (int i = 0; i < gameManager.instance.player.MaxMana; i++)
        {
            ManaList[i].GetComponent<Slider>().value = Mathf.Lerp(gameManager.instance.player.NowMana, gameManager.instance.player.GetStats("마나회복시간") * (i + 1), Time.deltaTime);
        }

        //gameManager.instance.player.NowMana의 최대값
        if (gameManager.instance.player.NowMana >= gameManager.instance.player.GetStats("마나회복시간") * gameManager.instance.player.GetStats("최대 마나"))
        {
            gameManager.instance.player.NowMana = gameManager.instance.player.GetStats("마나회복시간") * gameManager.instance.player.GetStats("최대 마나");
        }

        //gameManager.instance.player.NowMana의 최소값
        if (gameManager.instance.player.NowMana <= 0)
        {
            gameManager.instance.player.NowMana = 0;
        }
    }


	   //들고있는 카드 만드는 함수
    public void CreateHoldCard(int j) //j = 0 (HoldCard[0] = Q) / j = 1 (HoldCard[1] = W) .... / j = 4 (HoldCard[4] = Next Card) 
    {
        for (int i = j; i < 5; i++)
        {
            //덱 안 카드 중 랜덤으로 불러오기
            int RandomCardNum = Random.Range(0, PlayerCardDeck.Count);

            HoldCard = Instantiate(PlayerCardDeck[RandomCardNum]); //무작위 카드 인스턴스 화
            HoldCard.transform.GetChild(0).gameObject.SetActive(false); //toggle 끄기
            HoldCard.transform.SetParent(SkillPosition[i].transform); //해당 카드 -> 해당 키
            HoldCard.GetComponent<RectTransform>().anchoredPosition = Vector2.one; //RectTransform -> Vector.one으로 선언

            //덱 안 카드 제거
            PlayerCardDeck.RemoveAt(RandomCardNum);

        }
    }

    //덱 안 카드가 없는 지 체크
    public void CardCheck()
    {
        //덱 안 카드가 없으면
        if (PlayerCardDeck.Count == 0)
        {
            for (int i = 0; i < Discard.Count; i++)
            {
                //덱 리스트로 버려진 카드 리셋
                PlayerCardDeck.Add(Discard[i]);
            }

            //버려진 카드 리셋
            Discard.Clear();
        }
    }

    //카드 사용하기
    public void UseCard(int Key)
    {
        HoldCard = SkillPosition[Key].transform.GetChild(0).gameObject;
        
        FindCard(HoldCard); //해당 키의 카드 찾기

        //현재 마나 >= 카드 코스트
        if (gameManager.instance.player.NowMana >= CardCost * gameManager.instance.player.ManaRegenerationTime)
        {
            //마나 코스트 사용
            gameManager.instance.player.AddStats("현재 마나", CardCost);
            CardEffect(CardNumber); //해당 키의 카드 효과 발행

            Discard.Add(StoreCardList[CardNumber]); //해당 키의 카드 버리기 

            Destroy(HoldCard, 0.1f); //해당 키의 카드 파괴

            GameObject ChangeCard = SkillPosition[4].transform.GetChild(0).gameObject; //Next Card 변수 선언
            ChangeCard.transform.SetParent(SkillPosition[Key].transform); // Next Card의 부모 -> 해당 키로 변환
            ChangeCard.GetComponent<RectTransform>().anchoredPosition = Vector2.one; //RectTransform -> Vector.one으로 선언

            CreateHoldCard(4); //Next Card에 랜덤카드 받기

        } 

        else { Debug.Log("마나가 부족합니다."); }
    }

    //해당하는 카드 찾기
    public void FindCard(GameObject HoldCard)
    {
        //들고있는 카드가 상점 카드 리스트판별
        for (int i = 0; i < StoreCardList.Count; i++)
        {
            //들고있는 카드가 상점 카드 리스트와 같은 것을 찾으면 Swtich문으로 가면서 반복문 탈출
            if (StoreCardList[i].name + "(Clone)" == HoldCard.name)
            {
                CardNumber = i;
                CardCost = StoreCardList[i].layer - 12;

                break;
            }
        }
    }


    //카드 효과 -> 리스트 번호로 찾는 방법
    public void CardEffect(int CardNumber)
    {
        //각 카드의 효과
       switch (CardNumber)
        {
            case 0:
                {
                    Debug.Log("쉴드");
                    return;
                }

            
        }
    }

}
