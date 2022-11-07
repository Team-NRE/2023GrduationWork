using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    public List<Transform> PlayerCardList = new List<Transform>();
    public List<Transform> SkillPosition = new List<Transform>();

    private void Awake() {
        Transform CardPos = GameObject.Find("PlayerCard").transform; 
 
        for (int i = 0; i < CardPos.childCount; i++)
        {
            //Plane - PlayerCard의 하위 객체 (Q,W,E,R의 Transform 값 리스트에 저장)
            Transform child = CardPos.GetChild(i);
            SkillPosition.Add(child);
        }
    }
    public void UseCard() 
    {
        int RandomSkillNum = Random.Range(0,SkillPosition.Count); 

        //랜덤 카드 사용
        int RandomCardNum = Random.Range(0,PlayerCardList.Count);
        Debug.Log(PlayerCardList[RandomCardNum]);

        PlayerCardList[RandomCardNum].position = SkillPosition[RandomSkillNum].position;
        
    }
}
