using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardList : MonoBehaviour
{
    //상점 카드들의 리스트
    public List<Transform> StoreCardList = new List<Transform>();
    Button PurchaseButton;

    public void Awake() 
    {
        //버튼 찾아서 연결해주기
        PurchaseButton = GameObject.Find("PurChase").GetComponent<Button>();
        //AddListener로 구입 함수 연결
        PurchaseButton.onClick.AddListener(PurChase);
        
        
        //상점 카드 리스트에 전체 카드 추가 
        for (int i = 0; i < transform.childCount; i++)
        {
            //Plane - Store - StoreImg- Cards의 하위 객체
            Transform child = transform.GetChild(i);
            StoreCardList.Add(child);
        }

    }

    //구입 함수
    public void PurChase()
    {
        //상점 카드 리스트 중 Toggle.isOn이 True인 경우만 PlayerCardList에 추가
        for (int i = 0; i < StoreCardList.Count; i++)
        {
            Toggle toggle = StoreCardList[i].GetComponent<Toggle>();
            
            if(toggle.isOn == true)
            {
                Debug.Log(StoreCardList[i] + "구입");
                
                GameObject.Find("Player").GetComponent<PlayerCard>().PlayerCardList.Add(StoreCardList[i]);
            }

        }
    }
}


