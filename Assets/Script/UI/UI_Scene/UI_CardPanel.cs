using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    GameObject Card_Mana;
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
        //나중에 덱이 늘어나면 여기에 파라미터로 덱 아이디를 전달
        BaseCard.ExportDeck();

        //Find and Bind UI object
        Bind<GameObject>(typeof(Cards));
        Q_Btn = Get<GameObject>((int)Cards.Q);
        W_Btn = Get<GameObject>((int)Cards.W);
        E_Btn = Get<GameObject>((int)Cards.E);
        R_Btn = Get<GameObject>((int)Cards.R);

        DeckStart();

        BindEvent(Q_Card, (PointerEventData data) => { UI_UseQ(data); });
        BindEvent(W_Card, (PointerEventData data) => { UI_UseW(data); });
        BindEvent(E_Card, (PointerEventData data) => { UI_UseE(data); });
        BindEvent(R_Card, (PointerEventData data) => { UI_UseR(data); });

        Card_Mana = Util.FindChild(Managers.UI.Root);
    }

    private void Update()
    {
        MouseDownAction();
        KeyDownAction();
    }

    public void MouseDownAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //타겟이 있을 때 (아군, 도로, 적)
            if (BaseCard._lockTarget != null)
            {
                switch (BaseCard._NowKey)
                {
                    case "Q":
                        Debug.Log("Q UI Change");
                        UI_UseQ();
                        break;

                    case "W":
                        Debug.Log("W UI Change");
                        UI_UseW();
                        break;

                    case "E":
                        Debug.Log("E UI Change");
                        UI_UseE();
                        break;

                    case "R":
                        Debug.Log("R UI Change");
                        UI_UseR();
                        break;
                }
            }
        }
    }

    public void KeyDownAction()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Q_Btn.GetComponentInChildren<UI_Card>()._rangeType == "None")
            {
                UI_UseQ();
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (W_Btn.GetComponentInChildren<UI_Card>()._rangeType == "None")
            {
                UI_UseW();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (E_Btn.GetComponentInChildren<UI_Card>()._rangeType == "None")
            {
                UI_UseE();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (R_Btn.GetComponentInChildren<UI_Card>()._rangeType == "None")
            {
                UI_UseR();
            }
        }
    }


    public void DeckStart()
    {
        //여기서 이제 UI를 자식객체로 넣어준다.
        Q_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.StartDeck()}", Q_Btn.transform);
        W_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.StartDeck()}", W_Btn.transform);
        E_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.StartDeck()}", E_Btn.transform);
        R_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.StartDeck()}", R_Btn.transform);
    }


    //0번 인덱스의 리스트를 반드시 사용한다.
    public void UI_UseQ(PointerEventData data = null)
    {
        //카드에 _Cost 저장 -> 
        //Q_Btn.GetComponentInChildren<UI_Card>()._cost;
        //사용한 카드
        string _nowCard = Q_Btn.transform.GetChild(0).name;
        Debug.Log($"사용한 카드 : {_nowCard}");
        //사용한 카드 파괴
        Q_Btn.GetComponentInChildren<UI_Card>().DestroyCard(0.5f);
        //새로운 카드 덱에서 리필
        Q_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.UseCard(_nowCard)}", Q_Btn.transform);

        //BindEvent(Q_Card, (PointerEventData data) => { UI_UseQ(data); });
    }


    //1번 인덱스의 리스트를 반드시 사용한다.
    public void UI_UseW(PointerEventData data = null)
    {
        //사용한 카드
        string _nowCard = W_Btn.transform.GetChild(0).name;
        Debug.Log($"사용한 카드 : {_nowCard}");
        //사용한 카드 파괴
        W_Btn.GetComponentInChildren<UI_Card>().DestroyCard(0.5f);
        //새로운 카드 덱에서 리필
        W_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.UseCard(_nowCard)}", W_Btn.transform);

        //BindEvent(W_Card, (PointerEventData data) => { UI_UseW(data); });
    }

    //2번 인덱스의 리스트를 반드시 사용한다.
    public void UI_UseE(PointerEventData data = null)
    {
        //사용한 카드
        string _nowCard = E_Btn.transform.GetChild(0).name;
        Debug.Log($"사용한 카드 : {_nowCard}");
        //사용한 카드 파괴
        E_Btn.GetComponentInChildren<UI_Card>().DestroyCard(0.5f);
        //새로운 카드 덱에서 리필
        E_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.UseCard(_nowCard)}", E_Btn.transform);

        //BindEvent(E_Card, (PointerEventData data) => { UI_UseE(data); });
    }

    //3번 인덱스의 리스트를 반드시 사용한다.
    public void UI_UseR(PointerEventData data = null)
    {
        //사용한 카드
        string _nowCard = R_Btn.transform.GetChild(0).name;
        Debug.Log($"사용한 카드 : {_nowCard}");
        //사용한 카드 파괴
        R_Btn.GetComponentInChildren<UI_Card>().DestroyCard(0.5f);
        //새로운 카드 덱에서 리필
        R_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.UseCard(_nowCard)}", R_Btn.transform);

        //BindEvent(R_Card, (PointerEventData data) => { UI_UseR(data); });
    }
}
