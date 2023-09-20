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


    UI_Card Q_UI;
    UI_Card W_UI;
    UI_Card E_UI;
    UI_Card R_UI;


    Image Q_cardimg;
    Image W_cardimg;
    Image E_cardimg;
    Image R_cardimg;

    public PlayerStats pStat;
    PlayerType _pType;

    float targetDis;

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

        //나중에 덱이 늘어나면 여기에 파라미터로 덱 아이디를 전달
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


        //UI_img
        Q_cardimg = Q_Card.transform.GetChild(1).gameObject.GetComponent<Image>();
        W_cardimg = W_Card.transform.GetChild(1).gameObject.GetComponent<Image>();
        E_cardimg = E_Card.transform.GetChild(1).gameObject.GetComponent<Image>();
        R_cardimg = R_Card.transform.GetChild(1).gameObject.GetComponent<Image>();


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
        if(pStat == null)
        {
            pStat = Managers.game.myCharacter?.GetComponent<PlayerStats>();
            return;
        }

        if (pStat.nowHealth > 0)
        {
            MouseDownAction();
            KeyDownAction();
        }
        CardUseable();
    }

    //Range 후 사용 카드
    public void MouseDownAction()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (BaseCard._lockTarget != null)
            {
                targetDis = Vector3.Distance(BaseCard._lockTarget.transform.position, Managers.game.myCharacter.transform.position);
            }

            //Range -> _lockTarget 있어야 사용
            //Arrow, Cone, Line, Point -> _lockTarget이 없어도 사용 가능
            switch (BaseCard._NowKey)
            {
                case "Q":
                    UI_Card Q_UICard = Q_Btn.GetComponentInChildren<UI_Card>();
                    Debug.Log($"{Q_UICard._rangeType}");
                    //Range 스킬
                    if (BaseCard._lockTarget != null && targetDis <= Q_UICard._rangeScale)
                    {
                        UI_UseQ();
                    }

                    //Arrow, Cone, Line, Point 스킬
                    if (BaseCard._lockTarget == null && Q_UICard._rangeType != Define.CardType.Range)
                    {
                       
                        UI_UseQ();
                    }

                    break;

                case "W":
                    UI_Card W_UICard = W_Btn.GetComponentInChildren<UI_Card>();
                    Debug.Log($"{W_UICard._rangeType}");
                    //Range 스킬
                    if (BaseCard._lockTarget != null && targetDis <= W_UICard._rangeScale)
                    {
                
                        UI_UseW();
                    }

                    //Arrow, Cone, Line, Point 스킬
                    if (BaseCard._lockTarget == null && W_UICard._rangeType != Define.CardType.Range)
                    {
                        
                        UI_UseW();
                    }

                    break;

                case "E":
                    UI_Card E_UICard = E_Btn.GetComponentInChildren<UI_Card>();
                    Debug.Log($"{E_UICard._rangeType}");
                    //Range 스킬
                    if (BaseCard._lockTarget != null && targetDis <= E_UICard._rangeScale)
                    {
                        
                        UI_UseE();
                    }

                    //Arrow, Cone, Line, Point 스킬
                    if (BaseCard._lockTarget == null && E_UICard._rangeType != Define.CardType.Range)
                            
                    {
                        
                        UI_UseE();
                    }
                    break;

                case "R":
                    UI_Card R_UICard = R_Btn.GetComponentInChildren<UI_Card>();
                    Debug.Log($"{R_UICard._rangeType}");
                    //Range 스킬
                    if (BaseCard._lockTarget != null && targetDis <= R_UICard._rangeScale)
                    {
                        
                        UI_UseR();
                    }

                    //Arrow, Cone, Line, Point 스킬
                    if (BaseCard._lockTarget == null && R_UICard._rangeType != Define.CardType.Range)
                    {
                        
                        UI_UseR();
                    }

                    break;
            }

            
            
        }
    }

    //바로 사용 카드
    public void KeyDownAction()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (pStat.UseMana("Q").Item1 == true
                        && Q_Btn.GetComponentInChildren<UI_Card>()._rangeType == Define.CardType.None)
            {
                UI_UseQ();
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (pStat.UseMana("W").Item1 == true
                        && W_Btn.GetComponentInChildren<UI_Card>()._rangeType == Define.CardType.None)
            {
                UI_UseW();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pStat.UseMana("E").Item1 == true
                        && E_Btn.GetComponentInChildren<UI_Card>()._rangeType == Define.CardType.None)
            {
                UI_UseE();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (pStat.UseMana("R").Item1 == true
                        && R_Btn.GetComponentInChildren<UI_Card>()._rangeType == Define.CardType.None)
            {
                UI_UseR();
            }
        }
    }


    public void CardUseable()
    {
        //Q Image
        if (pStat.UseMana(null, Q_UI).Item1 == false)
            Q_cardimg.color = new Color32(61, 61, 61, 255);
        if (pStat.UseMana(null, Q_UI).Item1 == true)
            Q_cardimg.color = new Color32(255, 255, 255, 255);

        //W Image
        if (pStat.UseMana(null, W_UI).Item1 == false)
            W_cardimg.color = new Color32(61, 61, 61, 255);
        if (pStat.UseMana(null, W_UI).Item1 == true)
            W_cardimg.color = new Color32(255, 255, 255, 255);

        //E Image
        if (pStat.UseMana(null, E_UI).Item1 == false)
            E_cardimg.color = new Color32(61, 61, 61, 255);
        if (pStat.UseMana(null, E_UI).Item1 == true)
            E_cardimg.color = new Color32(255, 255, 255, 255);

        //R Image
        if (pStat.UseMana(null, R_UI).Item1 == false)
            R_cardimg.color = new Color32(61, 61, 61, 255);
        if (pStat.UseMana(null, R_UI).Item1 == true)
            R_cardimg.color = new Color32(255, 255, 255, 255);
    }



    //0번 인덱스의 리스트를 반드시 사용한다.
    public void UI_UseQ()
    {
        //UI_Card 
        UI_Card Q_CardUI = Q_Btn.GetComponentInChildren<UI_Card>();

        //마나 사용
        pStat.nowMana -= pStat.UseMana(null, Q_CardUI).Item2;

        //사용한 카드
        string _nowCard = Q_Btn.transform.GetChild(0).name;

        //사용한 카드 파괴
        Q_CardUI.DestroyCard(0.1f);

        //새로운 카드 덱에서 리필
        Q_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.UseCard(_nowCard)}", Q_Btn.transform);
        Q_UI = Q_Card.GetComponentInChildren<UI_Card>();
        Q_cardimg = Q_Card.transform.GetChild(1).gameObject.GetComponent<Image>();

        //현재 누른 키 리셋
        BaseCard._NowKey = null;

        //마우스 액션
        //BindEvent(Q_Card, (PointerEventData data) => { UI_UseQ(data); });

    }

    //1번 인덱스의 리스트를 반드시 사용한다.
    public void UI_UseW()
    {
        //UI_Card 
        UI_Card W_CardUI = W_Btn.GetComponentInChildren<UI_Card>();

        //마나 사용
        pStat.nowMana -= pStat.UseMana(null, W_CardUI).Item2;

        //사용한 카드
        string _nowCard = W_Btn.transform.GetChild(0).name;

        //사용한 카드 파괴
        W_CardUI.DestroyCard(0.1f);

        //새로운 카드 덱에서 리필
        W_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.UseCard(_nowCard)}", W_Btn.transform);
        W_UI = W_Card.GetComponentInChildren<UI_Card>();
        W_cardimg = W_Card.transform.GetChild(1).gameObject.GetComponent<Image>();

        //현재 누른 키 리셋
        BaseCard._NowKey = null;

        //마우스 액션
        //BindEvent(W_Card, (PointerEventData data) => { UI_UseW(data); });
    }

    //2번 인덱스의 리스트를 반드시 사용한다.
    public void UI_UseE()
    {
        //UI_Card 
        UI_Card E_CardUI = E_Btn.GetComponentInChildren<UI_Card>();

        //마나 사용
        pStat.nowMana -= pStat.UseMana(null, E_CardUI).Item2;

        // 사용한 카드
        string _nowCard = E_Btn.transform.GetChild(0).name;

        //사용한 카드 파괴
        E_CardUI.DestroyCard(0.1f);

        //새로운 카드 덱에서 리필
        E_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.UseCard(_nowCard)}", E_Btn.transform);
        E_UI = E_Card.GetComponentInChildren<UI_Card>();
        E_cardimg = E_Card.transform.GetChild(1).gameObject.GetComponent<Image>();

        //현재 누른 키 리셋
        BaseCard._NowKey = null;

        //마우스 액션
        //BindEvent(E_Card, (PointerEventData data) => { UI_UseE(data); });
    }

    //3번 인덱스의 리스트를 반드시 사용한다.
    public void UI_UseR()
    {
        //UI_Card 
        UI_Card R_CardUI = R_Btn.GetComponentInChildren<UI_Card>();

        //마나 사용
        pStat.nowMana -= pStat.UseMana(null, R_CardUI).Item2;

        //사용한 카드
        string _nowCard = R_Btn.transform.GetChild(0).name;
        
        //현재 누른 키 리셋
        BaseCard._NowKey = null;

        //마우스 액션
        //BindEvent(R_Card, (PointerEventData data) => { UI_UseR(data); });
    }
}
