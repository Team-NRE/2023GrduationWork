using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    //플레이어 덱
    public List<GameObject> PlayerCardDeck = new List<GameObject>();
    
    //Q,W,E,R 카드 위치
    public List<GameObject> SkillPosition = new List<GameObject>();

    public List<GameObject> Discard = new List<GameObject>();

    private void Awake() {
        Transform CardPos = GameObject.Find("PlayerCard").transform; 
 
        for (int i = 0; i < CardPos.childCount; i++)
        {
            //Plane - PlayerCard의 하위 객체 (Q,W,E,R의 gameobject 리스트에 저장)
            GameObject CardChild = CardPos.GetChild(i).gameObject;
            SkillPosition.Add(CardChild);
        }
    }

    private void Update() 
    {
        if(PlayerCardDeck.Count == 0)
        {
            for(int i = 0; i < Discard.Count; i++)
            {
                PlayerCardDeck.Add(Discard[i]);
            }
            
            Discard.Clear();
        }
    }
    public void UseCard(int Key) 
    {
        
        //플레이어 덱 안 랜덤 카드 받기
        int RandomCardNum = Random.Range(0, PlayerCardDeck.Count);

        //누른 키 안 카드 이미지 = 랜덤 카드 이미지
        SkillPosition[Key].GetComponent<Image>().sprite = PlayerCardDeck[RandomCardNum].GetComponentInChildren<Image>().sprite;

        Discard.Add(PlayerCardDeck[RandomCardNum]);    
        PlayerCardDeck.RemoveAt(RandomCardNum);
    }


}
