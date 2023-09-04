using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class BaseCard
{
    //모든 카드
    public static List<string> _AllPublicCard = new List<string>();
    public static List<string> _AllJobCard = new List<string>();

    //나만의 덱 카드
    public static List<string> _MyDeck = new List<string>();
    //덱 안의 카드 변환 List
    public static List<string> _initDeck = new List<string>();
    //현재 덱 안의 전체 카드 -> 상점에 띄우려면 필요할거같음
    public static List<string> _deckCards = new List<string>();
    public static List<string> _AddOns = new List<string>();


    //Range On이 되었을 때의 해당 키 정보
    public static string _NowKey = null;
    //타겟 정보
    public static GameObject _lockTarget = null;


    //Json으로 모든 카드들을 불러오기
    public static List<string> LoadAllCard(string cardType)
    {
        Dictionary<string, Data.Card> card = Managers.Data.CardDict;
        List<string> cardNames = new List<string>();

        for (int i = 0; i < card[cardType].card.Count; i++)
        {
            cardNames.Add(card[cardType].card[i]);
        }
        return cardNames;
    }

    public static List<string> ExportPublicCard()
    {
        return _AllPublicCard = LoadAllCard("PublicCard");
    }

    public static List<string> ExportJobCard()
    {
        return _AllJobCard = LoadAllCard("JobCard");
    }


    //Json으로 덱을 가져온다. 나중에 덱 숫자가 늘어나면 파라미터로 입력
    public static List<string> LoadDeck(int deckNum = 0)
    {
        Dictionary<int, Data.Deck> deck = Managers.Data.DeckDict;
        List<string> cardNames = new List<string>();

        for (int i = 0; i < deck[deckNum].cards.Count; i++)
        {
            cardNames.Add(deck[deckNum].cards[i]);
            //현재 카드 덱 정보
            _deckCards.Add(deck[deckNum].cards[i]);
            //Debug.Log($"카드 {i}번 : {_deckCards[i]}");
        }
        return cardNames;
    }

    public static List<string> ExportMyDeck()
    {
        return _MyDeck = LoadDeck();
    }

    //가져온 이름으로 List를 채운다
    public static List<string> ExportDeck()
    {
        return _initDeck = LoadDeck();
    }

    //카드 사용
    public static string UseCard(string ReloadCard = null)
    {
        //카드가 남아 있다면 랜덤으로 뽑아서 처리 
        int rand = UnityEngine.Random.Range(1, _initDeck.Count);
        //카드 이름 저장
        string ChoiseCard = _initDeck[rand];
        //남은 카드 List의 랜덤하게 뽑은 카드 삭제
        _initDeck.RemoveAt(rand);

        //리필 카드가 있다면
        if (ReloadCard != null) { _initDeck.Add(ReloadCard); }

        for (int i = 0; i < _initDeck.Count; i++)
        {
            Debug.Log($"덱 안 카드 이름 : {_initDeck[i]}");
        }

        return ChoiseCard;
    }

    //초기 4장 선정
    //1. 선정된 카드를 리스트에서 지운다
    //2. 초기 4장을 인스턴스 한다. 한번 사용하고 그 뒤로는 사용되지 않는다.
    public static string StartDeck()
    {
        //랜덤으로 카드 뽑기
        int rand = UnityEngine.Random.Range(0, _initDeck.Count);
        string ChoiseCard = _initDeck[rand];

        Debug.Log($"초기 핸드 안 카드 이름 : {ChoiseCard}");

        //뽑은 카드 덱에서 삭제
        _initDeck.RemoveAt(rand);

        return ChoiseCard;
    }

    public static string StartJobCard()
    {
        string ChoiseCard = _initDeck[0];
        //Job 카드를 리필카드 제외
        _initDeck.RemoveAt(0);

        return ChoiseCard;
    }

    //다쓰면 다시 채운다, 비었는지 여부는 UI 이벤트 단에서 바꾼다.
    public static List<string> ReloadDeck()
    {
        Debug.Log("Reload call");
        //비었으니 기본 덱으로 채운다
        ExportDeck();

        //만약 AddCard로 추가된 카드가 있다면
        if (_AddOns == null)
            Debug.Log("추가카드 없음");
        else
        {
            foreach (string name in _AddOns)
            {
                _initDeck.Add(name);
                Debug.Log($"Adds are : {name}");
            }
        }

        return _initDeck;
    }

    //새로운 카드를 추가해서 덱을 새로 만드는 카드
    public static List<string> AddCard(string name)
    {
        List<string> newDeck = new List<string>();

        _initDeck.Add(name);

        return newDeck;
    }

    public static int GetRemotePlayerId(GameObject target)
    {
        int remoteId = target.GetComponent<PhotonView>().ViewID;
        return remoteId;
    }

    public static GameObject GetRemotePlayer(int remoteId)
    {
        GameObject target = PhotonView.Find(remoteId)?.gameObject;
        return target;
    }

    public static Vector3 GetRemoteVector(int remoteId)
    {
        Vector3 targetVector = GetRemotePlayer(remoteId).transform.position;
        return targetVector;
    }
}
