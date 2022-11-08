using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardList : MonoBehaviour
{
    //상점 카드들의 리스트
    public List<GameObject> StoreCardList;
    Button PurchaseButton;

    //카드 이펙트 판별 숫자
    int CardNumber;

    //버튼 & 상점 리스트 초기화
    public void Awake() 
    {
        //버튼 찾아서 연결해주기
        PurchaseButton = GameObject.Find("PurChase").GetComponent<Button>();
        //AddListener로 구입 함수 연결
        PurchaseButton.onClick.AddListener(PurChase);
        
        //상점 카드 리스트에 전체 카드 추가 
        Transform Cards = GameObject.Find("Cards").transform;  
        for (int i = 0; i < Cards.childCount; i++)
        {
            //Plane - Store - StoreImg- Cards의 하위 객체
            GameObject child = Cards.GetChild(i).gameObject;
            StoreCardList.Add(child);
        }
        
    }

    //구입 함수
    public void PurChase()
    {
        //상점 카드 리스트 중 Toggle.isOn이 True인 경우만 PlayerCardDeck에 추가
        for (int i = 0; i < StoreCardList.Count; i++)
        {
            Toggle toggle = StoreCardList[i].GetComponent<Toggle>();
            
            if(toggle.isOn == true)
            {
                //Debug.Log(StoreCardList[i] + "구입");
                
                GameObject.Find("Player").GetComponent<PlayerCard>().PlayerCardDeck.Add(StoreCardList[i].gameObject);
            }

        }
    }


    //카드 효과
    public void CardEffect(GameObject HoldCard)
    {
        //들고있는 카드가 상점 카드 리스트판별
        for(int i = 0; i < StoreCardList.Count; i++)
        {
            if(StoreCardList[i] == HoldCard)
            {
                CardNumber = i;
                break;
            }
        }

        //각 카드의 효과
        switch (CardNumber)
        {
            case 0:
            {
                Debug.Log("Card_1의 효과 발동");
                return; 
            }

            case 1:
            {
                Debug.Log("Card_2의 효과 발동");
                return; 
            }

            case 2:
            {
                Debug.Log("Card_3의 효과 발동");
                return; 
            }

            case 3:
            {
                Debug.Log("Card_4의 효과 발동");
                return; 
            }

            case 4:
            {
                Debug.Log("Card_5의 효과 발동");
                return; 
            }

            case 5:
            {
                Debug.Log("Card_6의 효과 발동");
                return; 
            }
            
            case 6:
            {
                Debug.Log("Card_7의 효과 발동");
                return; 
            }
            
            case 7:
            {
                Debug.Log("Card_8의 효과 발동");
                return; 
            }

            case 8:
            {
                Debug.Log("Card_9의 효과 발동");
                return; 
            }
            
        }
    }


}


