using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; /* for Serializable */

public class JsonTest : MonoBehaviour
{
    [Serializable]
    //카드 변수
    public class CardVariable
    {
        public int id;
        public string cardname;
        public float attack;
        public float defense;
        public float health;
    }

    //카드 스텟 리스트화
    public class CardstatList
    {
        public List<CardVariable> Cards; //변수 명 = json 상위 명 
    }

    
    Dictionary<int, CardVariable> CardDic = new Dictionary<int, CardVariable>();

    void Start()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Json/CardStats");

        CardstatList cardstatList = JsonUtility.FromJson<CardstatList>(textAsset.text);

        foreach (CardVariable lt in cardstatList.Cards)
        {
            CardDic.Add(lt.id, lt);
        }

        foreach (KeyValuePair<int, CardVariable> lt in CardDic)
        {
            Debug.Log(lt.Key);
            Debug.Log(lt.Value.cardname);
            Debug.Log(lt.Value.defense);
            Debug.Log("=============");
        }
    }
}
