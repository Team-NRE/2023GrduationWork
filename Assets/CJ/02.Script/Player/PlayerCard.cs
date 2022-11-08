using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    
    [Header ("---Card List---")]   
    //플레이어 덱
    public List<GameObject> PlayerCardDeck = new List<GameObject>();
    //사용하기 위한 카드 대기
    public List<GameObject> HoldCard = new List<GameObject>();
    //사용하고 난 버려진 카드 모음
    public List<GameObject> Discard = new List<GameObject>();
    //Q,W,E,R 카드 UI 위치 리스트
    List<GameObject> SkillPosition = new List<GameObject>();

    private void Awake()
    {
        // (카드 UI 위치 리스트 & 대기 카드 리스트) 의 값 초기화
        Transform CardPos = GameObject.Find("PlayerCard").transform;
        for (int i = 0; i < CardPos.childCount; i++)
        {
            //Plane - PlayerCard의 하위 객체 (Q,W,E,R의 gameobject 리스트에 저장)
            GameObject CardChild = CardPos.GetChild(i).gameObject;
            SkillPosition.Add(CardChild);
            
            //대기 카드 리스트 초기값 = null로 설정.
            HoldCard.Add(null);
        }
    }

    private void Start()
    {
        CreateHoldCard(0,4);
    }

    private void Update()
    {
        StartCoroutine(CardCheck());

    }

    //덱 안 카드가 없는 지 체크하는 코루틴
    IEnumerator CardCheck() 
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

        yield return new WaitForSeconds(0.3f);
    }

    //대기 카드 만드는 함수
    void CreateHoldCard(int j, int Routine) //i가 몇부터 몇까지 반복할건지 
    {        
        for (int i = j; i < Routine; i++)
        {
            //덱 안 카드 중 랜덤으로 불러오기
            int RandomCardNum = Random.Range(0, PlayerCardDeck.Count); 

            //덱 안 랜덤 카드를 (해당 키) 대기 카드로 옮기기 
            HoldCard[i] = PlayerCardDeck[RandomCardNum];
            //카드 UI도 함께.......
            SkillPosition[i].GetComponent<Image>().sprite = HoldCard[i].GetComponentInChildren<Image>().sprite;

            //덱 안 카드 제거
            PlayerCardDeck.RemoveAt(RandomCardNum);
        }
    }

    //카드 사용하기
    public void UseCard(int Key)
    {
        //-----카드 사용 알고리즘--------
        GameObject.Find("Store").GetComponent<CardList>().CardEffect(HoldCard[Key]); //해당 카드의 효과 받아야됨.

        Discard.Add(HoldCard[Key]); //해당 키의 카드 버리기 
        HoldCard[Key] = null; //해당 키의 대기 카드 잠시 비우기
    
        //해당 키에 카드 받아오기 
        CreateHoldCard(Key,Key+1);
    }
}
