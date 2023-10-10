using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Stat;
using Define;
using TMPro;

public class UI_CardPanel : UI_Card
{
    GameObject Q_Btn;
    GameObject W_Btn;
    GameObject E_Btn;
    GameObject R_Btn;

    GameObject Q_Card;
    GameObject W_Card;
    GameObject E_Card;
    GameObject R_Card;


    public UI_Card Q_UI;
    public UI_Card W_UI;
    public UI_Card E_UI;
    public UI_Card R_UI;

    GameObject Q_CardEffect;
    GameObject W_CardEffect;
    GameObject E_CardEffect;
    GameObject R_CardEffect;

    Image Q_CardImage;
    Image W_CardImage;
    Image E_CardImage;
    Image R_CardImage;

    GameObject Q_CardCountObject;
    GameObject W_CardCountObject;
    GameObject E_CardCountObject;
    GameObject R_CardCountObject;

    TextMeshProUGUI Q_CardCount;
    TextMeshProUGUI W_CardCount;
    TextMeshProUGUI E_CardCount;
    TextMeshProUGUI R_CardCount;

    BaseController bc;
    Players p; 
    PlayerStats pStat;

    float targetDis;
    float CountSet;

    public enum CardObjects
    {
        Panel,
    }

    public enum Cards
    {
        Q,
        W,
        E,
        R,
    }

    public override void Init()
    {
        pStat = Managers.game.myCharacter?.GetComponent<PlayerStats>();
        p = Managers.game.myCharacter?.GetComponent<Players>();
        bc = Managers.game.myCharacter?.GetComponent<BaseController>();

        //나중에 덱이 늘어나면 여기에 파라미터로 덱 아이디를 전달
        BaseCard.ExportMyDeck((int)Managers.game.myCharacterType);
        BaseCard.ExportDeck((int)Managers.game.myCharacterType);

        //Find and Bind UI object
        Bind<GameObject>(typeof(Cards));
        Q_Btn = Get<GameObject>((int)Cards.Q);
        W_Btn = Get<GameObject>((int)Cards.W);
        E_Btn = Get<GameObject>((int)Cards.E);
        R_Btn = Get<GameObject>((int)Cards.R);

        DeckStart();

        //UI
        Q_UI = Q_Card.GetComponentInChildren<UI_Card>();
        W_UI = W_Card.GetComponentInChildren<UI_Card>();
        E_UI = E_Card.GetComponentInChildren<UI_Card>();
        R_UI = R_Card.GetComponentInChildren<UI_Card>();


        //UI_Card Effect
        Q_CardEffect = Q_Card.transform.Find("Card_Effect").gameObject;
        W_CardEffect = W_Card.transform.Find("Card_Effect").gameObject;
        E_CardEffect = E_Card.transform.Find("Card_Effect").gameObject;
        R_CardEffect = R_Card.transform.Find("Card_Effect").gameObject;


        //UI_Card Color
        Q_CardImage = Q_Card.transform.Find("Card_img").gameObject.GetComponent<Image>();
        W_CardImage = W_Card.transform.Find("Card_img").gameObject.GetComponent<Image>();
        E_CardImage = E_Card.transform.Find("Card_img").gameObject.GetComponent<Image>();
        R_CardImage = R_Card.transform.Find("Card_img").gameObject.GetComponent<Image>();


        //UI_Card Count Gameobject
        Q_CardCountObject = Q_Card.transform.Find("Card_Count").gameObject;
        W_CardCountObject = W_Card.transform.Find("Card_Count").gameObject;
        E_CardCountObject = E_Card.transform.Find("Card_Count").gameObject;
        R_CardCountObject = R_Card.transform.Find("Card_Count").gameObject;


        //UI_Card Count
        Q_CardCount = Q_CardCountObject.GetComponent<TextMeshProUGUI>();
        W_CardCount = W_CardCountObject.GetComponent<TextMeshProUGUI>();
        E_CardCount = E_CardCountObject.GetComponent<TextMeshProUGUI>();
        R_CardCount = R_CardCountObject.GetComponent<TextMeshProUGUI>();

        CountSet = 3.0f;

        Managers.Input.MouseAction -= MouseDownAction;
        Managers.Input.MouseAction += MouseDownAction;

        //BindEvent(Q_Card, (PointerEventData data) => { UI_UseQ(data); });
        //BindEvent(W_Card, (PointerEventData data) => { UI_UseW(data); });
        //BindEvent(E_Card, (PointerEventData data) => { UI_UseE(data); });
        //BindEvent(R_Card, (PointerEventData data) => { UI_UseR(data); });
    }

    //초기 덱 
    public void DeckStart()
    {
        //여기서 이제 UI를 자식객체로 넣어준다.
        R_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.StartJobCard()}", R_Btn.transform);
        Q_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.StartDeck()}", Q_Btn.transform);
        W_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.StartDeck()}", W_Btn.transform);
        E_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.StartDeck()}", E_Btn.transform);
    }

    public void Update()
    {
        if (bc._startDie == true) { Managers.Input.UIKeyboardAction -= UIKeyDownAction; }
        if (bc._startDie == false)
        {
            //카드 사용 불가능
            if(bc._stopSkill == true) 
            { 
                Card_Cant_Use();

                Managers.Input.UIKeyboardAction -= UIKeyDownAction;

                return; 
            }
            //카드 사용 가능
            if(bc._stopSkill == false)  
            { 
                Card_Can_Use();

                Managers.Input.UIKeyboardAction -= UIKeyDownAction;
                Managers.Input.UIKeyboardAction += UIKeyDownAction;

                //range 카드일 때 타겟이 정해지고 Range가 On일 시 
                if (BaseCard._lockTarget != null && p._IsRange == true)
                {
                    //타겟와의 거리 Update
                    targetDis = Vector3.Distance(BaseCard._lockTarget.transform.position, Managers.game.myCharacter.transform.position);

                    NowKey();
                }
            }
        }
    }


    //Range On 이후 사용하는 카드
    public void MouseDownAction(Define.MouseEvent _evt)
    {
        if (_evt == Define.MouseEvent.LeftButton)
        {
            //Range 카드 제외 논타겟팅 카드
            if (BaseCard._lockTarget == null) { targetDis = 0; }

            //현재 키에 맞는 카드 사용
            NowKey();
        }
    }

    public void NowKey()
    {
        //range off 일 때 return 
        if (p._IsRange == false) return;
        switch (BaseCard._NowKey)
        {
            case "Q":
                //거리 짧으면
                if (Q_UI._rangeScale < targetDis) return;
                //Range 카드가 땅을 클릭 했다면
                if (Q_UI._rangeType == Define.CardType.Range && BaseCard._lockTarget == null) return;
                //카드 타입이 None이면
                if (Q_UI._rangeType == Define.CardType.None) return;
                
                UI_UseQ();

                break;


            case "W":
                //거리 짧으면
                if (W_UI._rangeScale < targetDis) return;
                //Range 카드가 땅을 클릭 했다면
                if (W_UI._rangeType == Define.CardType.Range && BaseCard._lockTarget == null) return;
                //카드 타입이 None이면
                if (W_UI._rangeType == Define.CardType.None) return;

                UI_UseW();

                break;


            case "E":
                //거리 짧으면
                if (E_UI._rangeScale < targetDis) return;
                //Range 카드가 땅을 클릭 했다면
                if (E_UI._rangeType == Define.CardType.Range && BaseCard._lockTarget == null) return;
                //카드 타입이 None이면
                if (E_UI._rangeType == Define.CardType.None) return;

                UI_UseE();

                break;


            case "R":
                //거리 짧으면
                if (R_UI._rangeScale < targetDis) return;
                //Range 카드가 땅을 클릭 했다면
                if (R_UI._rangeType == Define.CardType.Range && BaseCard._lockTarget == null) return;
                //카드 타입이 None이면
                if (R_UI._rangeType == Define.CardType.None) return;

                UI_UseR();

                break;
        }
    }



    // 바로 사용 하는 카드
    public void UIKeyDownAction(Define.UIKeyboard _key)
    {
        //키보드 입력 시
        switch (_key)
        {
            case Define.UIKeyboard.Q:
                //바로 사용 카드
                if (pStat.UseMana(_key.ToString()).Item1 == true && Q_UI._rangeType == Define.CardType.None)
                {
                    UI_UseQ();
                }

                break;

            case Define.UIKeyboard.W:
                if (pStat.UseMana(_key.ToString()).Item1 == true && W_UI._rangeType == Define.CardType.None)
                {
                    UI_UseW();
                }

                break;

            case Define.UIKeyboard.E:
                if (pStat.UseMana(_key.ToString()).Item1 == true && E_UI._rangeType == Define.CardType.None)
                {
                    UI_UseE();
                }

                break;

            case Define.UIKeyboard.R:
                if (pStat.UseMana(_key.ToString()).Item1 == true && R_UI._rangeType == Define.CardType.None)
                {
                    UI_UseR();
                }

                break;
        }
    }
    
    //카드 사용 가능
    public void Card_Can_Use()
    {
        CountSet = 2;
        //effect
        Q_CardEffect.SetActive(pStat.UseMana(null, Q_UI).Item1);
        W_CardEffect.SetActive(pStat.UseMana(null, W_UI).Item1);
        E_CardEffect.SetActive(pStat.UseMana(null, E_UI).Item1);
        R_CardEffect.SetActive(pStat.UseMana(null, R_UI).Item1);

        //Color
        Q_CardImage.color = Q_CardEffect.activeSelf == true ? Color.white : new Color32(60, 60, 60, 255);
        W_CardImage.color = W_CardEffect.activeSelf == true ? Color.white : new Color32(60, 60, 60, 255);
        E_CardImage.color = E_CardEffect.activeSelf == true ? Color.white : new Color32(60, 60, 60, 255);
        R_CardImage.color = R_CardEffect.activeSelf == true ? Color.white : new Color32(60, 60, 60, 255);

        //Count
        Q_CardCountObject.SetActive(false);
        W_CardCountObject.SetActive(false);
        E_CardCountObject.SetActive(false);
        R_CardCountObject.SetActive(false);
    }

    //카드 사용 못함
    public void Card_Cant_Use()
    {
        if (CountSet <= 0) { CountSet = 2; }
        if (CountSet != 2)
        {
            Managers.Input.UIKeyboardAction -= UIKeyDownAction;
        }

        //effect 
        Q_CardEffect.SetActive(false);
        W_CardEffect.SetActive(false);
        E_CardEffect.SetActive(false);
        R_CardEffect.SetActive(false);

        //color
        Q_CardImage.color = new Color32(60, 60, 60, 255);
        W_CardImage.color = new Color32(60, 60, 60, 255);
        E_CardImage.color = new Color32(60, 60, 60, 255);
        R_CardImage.color = new Color32(60, 60, 60, 255);

        //Count
        Q_CardCountObject.SetActive(true);
        W_CardCountObject.SetActive(true);
        E_CardCountObject.SetActive(true);
        R_CardCountObject.SetActive(true);

        //Count Start
        CountSet -= Time.deltaTime;
        Q_CardCount.text = CountSet.ToString("F0");
        W_CardCount.text = CountSet.ToString("F0");
        E_CardCount.text = CountSet.ToString("F0");
        R_CardCount.text = CountSet.ToString("F0");
    }


    //0번 인덱스의 리스트를 반드시 사용한다.
    public void UI_UseQ()
    {
        //마나 사용
        pStat.nowMana -= pStat.UseMana(null, Q_UI).Item2;

        //새로운 카드 덱에서 리필
        Refill_Q(Q_Card.name);

        //마우스 액션
        //BindEvent(Q_Card, (PointerEventData data) => { UI_UseQ(data); });

    }
    public void Refill_Q(string _nowCard = null)
    {
        //사용한 카드 파괴
        Q_UI.DestroyCard(0.1f);

        //카드 생성
        Q_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.UseCard(_nowCard)}", Q_Btn.transform);

        //해당 카드 정보 초기화
        Q_UI = Q_Card.GetComponentInChildren<UI_Card>();

        //해당 카드 자식 객체 초기화
        Q_CardImage = Q_Card.transform.Find("Card_img").gameObject.GetComponent<Image>();
        Q_CardEffect = Q_Card.transform.Find("Card_Effect").gameObject;
        Q_CardCountObject = Q_Card.transform.Find("Card_Count").gameObject;
        Q_CardCount = Q_CardCountObject.GetComponent<TextMeshProUGUI>();
    }



    //1번 인덱스의 리스트를 반드시 사용한다.
    public void UI_UseW()
    {
        //마나 사용
        pStat.nowMana -= pStat.UseMana(null, W_UI).Item2;

        //새로운 카드 덱에서 리필
        Refill_W(W_Card.name);

        //마우스 액션
        //BindEvent(W_Card, (PointerEventData data) => { UI_UseW(data); });

    }
    public void Refill_W(string _nowCard = null)
    {
        //사용한 카드 파괴
        W_UI.DestroyCard(0.1f);

        //카드 생성
        W_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.UseCard(_nowCard)}", W_Btn.transform);

        //해당 카드 정보 초기화
        W_UI = W_Card.GetComponentInChildren<UI_Card>();

        //해당 카드 자식 객체 초기화
        W_CardImage = W_Card.transform.Find("Card_img").gameObject.GetComponent<Image>();
        W_CardEffect = W_Card.transform.Find("Card_Effect").gameObject;
        W_CardCountObject = W_Card.transform.Find("Card_Count").gameObject;
        W_CardCount = W_CardCountObject.GetComponent<TextMeshProUGUI>();
    }



    //2번 인덱스의 리스트를 반드시 사용한다.
    public void UI_UseE()
    {
        //마나 사용
        pStat.nowMana -= pStat.UseMana(null, E_UI).Item2;

        //새로운 카드 덱에서 리필
        Refill_E(E_Card.name);

        //마우스 액션
        //BindEvent(E_Card, (PointerEventData data) => { UI_UseE(data); });

    }

    public void Refill_E(string _nowCard = null)
    {
        //사용한 카드 파괴
        E_UI.DestroyCard(0.1f);

        //카드 생성
        E_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.UseCard(_nowCard)}", E_Btn.transform);

        //해당 카드 정보 초기화
        E_UI = E_Card.GetComponentInChildren<UI_Card>();

        //해당 카드 자식 객체 초기화
        E_CardImage = E_Card.transform.Find("Card_img").gameObject.GetComponent<Image>();
        E_CardEffect = E_Card.transform.Find("Card_Effect").gameObject;
        E_CardCountObject = E_Card.transform.Find("Card_Count").gameObject;
        E_CardCount = E_CardCountObject.GetComponent<TextMeshProUGUI>();
    }


    //3번 인덱스의 리스트를 반드시 사용한다.
    public void UI_UseR()
    {
        //마나 사용
        pStat.nowMana -= pStat.UseMana(null, R_UI).Item2;

        //마우스 액션
        //BindEvent(R_Card, (PointerEventData data) => { UI_UseR(data); });
    }
}
