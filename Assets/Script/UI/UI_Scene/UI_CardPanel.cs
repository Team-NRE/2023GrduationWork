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
<<<<<<< HEAD
	public Define.KeyboardEvent _keyEvent { get; protected set; }

	GameObject Q_Btn;
	GameObject W_Btn;
	GameObject E_Btn;
	GameObject R_Btn;
=======
    GameObject Q_Btn;
    GameObject W_Btn;
    GameObject E_Btn;
    GameObject R_Btn;
>>>>>>> changjo

    GameObject Q_Card;
    GameObject W_Card;
    GameObject E_Card;
    GameObject R_Card;


    UI_Card Q_UI;
    UI_Card W_UI;
    UI_Card E_UI;
    UI_Card R_UI;

<<<<<<< HEAD
	private void Update()
	{
		KeyAction(_keyEvent);
		//Debug.Log(_keyEvent);
	}

	private void KeyAction(Define.KeyboardEvent evt)
	{
		if (evt == Define.KeyboardEvent.Q)
			UI_UseQ();
		if (evt == Define.KeyboardEvent.W)
			UI_UseW();
		if (evt == Define.KeyboardEvent.E)
			UI_UseE();
		if (evt == Define.KeyboardEvent.R)
			UI_UseR();
	}

	public override void Init()
	{
		Managers.Input.KeyAction -= KeyAction;
		Managers.Input.KeyAction += KeyAction;

		//나중에 덱이 늘어나면 여기에 파라미터로 덱 아이디를 전달
		BaseCard.ExportDeck();
		
		//Find and Bind UI object
		Bind<GameObject>(typeof(Cards));
		Q_Btn = Get<GameObject>((int)Cards.Q);
		W_Btn = Get<GameObject>((int)Cards.W);
		E_Btn = Get<GameObject>((int)Cards.E);
		R_Btn = Get<GameObject>((int)Cards.R);
=======

    Image Q_cardimg;
    Image W_cardimg;
    Image E_cardimg;
    Image R_cardimg;

>>>>>>> changjo

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

    private void Update()
    {
        MouseDownAction();
        KeyDownAction();
        CardUseable();
    }

    //마우스
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

<<<<<<< HEAD
		//if (useId != 0)
		//{
		Managers.Resource.Instantiate($"Cards/{BaseCard._initDeck[useId]}", R_Btn.transform);
		Debug.Log(useId);
		//}
=======
                    case "W":
                        Debug.Log("W UI Change");
                        UI_UseW();
                        break;
>>>>>>> changjo

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

    //키보드
    public void KeyDownAction()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (BaseCard.UseMana("Q").Item1 == true
                        && Q_Btn.GetComponentInChildren<UI_Card>()._rangeType == "None")
            {
                UI_UseQ();
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (BaseCard.UseMana("W").Item1 == true
                        && W_Btn.GetComponentInChildren<UI_Card>()._rangeType == "None")
            {
                UI_UseW();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (BaseCard.UseMana("E").Item1 == true
                        && E_Btn.GetComponentInChildren<UI_Card>()._rangeType == "None")
            {
                UI_UseE();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (BaseCard.UseMana("R").Item1 == true
                        && R_Btn.GetComponentInChildren<UI_Card>()._rangeType == "None")
            {
                UI_UseR();
            }
        }
    }


    public void CardUseable()
    {
        //Q Image
        if (BaseCard.UseMana(null, Q_UI).Item1 == false)
            Q_cardimg.color = new Color32(61, 61, 61, 255);
        if (BaseCard.UseMana(null, Q_UI).Item1 == true)
            Q_cardimg.color = new Color32(255, 255, 255, 255);

        //W Image
        if (BaseCard.UseMana(null, W_UI).Item1 == false)
            W_cardimg.color = new Color32(61, 61, 61, 255);
        if (BaseCard.UseMana(null, W_UI).Item1 == true)
            W_cardimg.color = new Color32(255, 255, 255, 255);

        //E Image
        if (BaseCard.UseMana(null, E_UI).Item1 == false)
            E_cardimg.color = new Color32(61, 61, 61, 255);
        if (BaseCard.UseMana(null, E_UI).Item1 == true)
            E_cardimg.color = new Color32(255, 255, 255, 255);

        //R Image
        if (BaseCard.UseMana(null, R_UI).Item1 == false)
            R_cardimg.color = new Color32(61, 61, 61, 255);
        if (BaseCard.UseMana(null, R_UI).Item1 == true)
            R_cardimg.color = new Color32(255, 255, 255, 255);
    }

    //초기 덱 
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
        //UI_Card 
        UI_Card Q_CardUI = Q_Btn.GetComponentInChildren<UI_Card>();
        //마나 사용
        BaseCard._nowMana -= BaseCard.UseMana(null, Q_CardUI).Item2;
        //사용한 카드
        string _nowCard = Q_Btn.transform.GetChild(0).name;
        Debug.Log($"사용한 카드 : {_nowCard}");
        //사용한 카드 파괴
        Q_CardUI.DestroyCard(0.5f);

        //새로운 카드 덱에서 리필
        Q_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.UseCard(_nowCard)}", Q_Btn.transform);
        Q_UI = Q_Card.GetComponentInChildren<UI_Card>();
        Q_cardimg = Q_Card.transform.GetChild(1).gameObject.GetComponent<Image>();
        
        //BindEvent(Q_Card, (PointerEventData data) => { UI_UseQ(data); });

    }

    //1번 인덱스의 리스트를 반드시 사용한다.
    public void UI_UseW(PointerEventData data = null)
    {
        //UI_Card 
        UI_Card W_CardUI = W_Btn.GetComponentInChildren<UI_Card>();
        //마나 사용
        BaseCard._nowMana -= BaseCard.UseMana(null, W_CardUI).Item2;
        //사용한 카드
        string _nowCard = W_Btn.transform.GetChild(0).name;
        Debug.Log($"사용한 카드 : {_nowCard}");
        //사용한 카드 파괴
        W_CardUI.DestroyCard(0.5f);

        //새로운 카드 덱에서 리필
        W_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.UseCard(_nowCard)}", W_Btn.transform);
        W_UI = W_Card.GetComponentInChildren<UI_Card>();
        W_cardimg = W_Card.transform.GetChild(1).gameObject.GetComponent<Image>();
        
        //BindEvent(W_Card, (PointerEventData data) => { UI_UseW(data); });
    }

    //2번 인덱스의 리스트를 반드시 사용한다.
    public void UI_UseE(PointerEventData data = null)
    {
        //UI_Card 
        UI_Card E_CardUI = E_Btn.GetComponentInChildren<UI_Card>();
        //마나 사용
        BaseCard._nowMana -= BaseCard.UseMana(null, E_CardUI).Item2;
        // 사용한 카드
        string _nowCard = E_Btn.transform.GetChild(0).name;
        Debug.Log($"사용한 카드 : {_nowCard}");
        //사용한 카드 파괴
        E_CardUI.DestroyCard(0.5f);

        //새로운 카드 덱에서 리필
        E_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.UseCard(_nowCard)}", E_Btn.transform);
        E_UI = E_Card.GetComponentInChildren<UI_Card>();
        E_cardimg = E_Card.transform.GetChild(1).gameObject.GetComponent<Image>();
        
        //BindEvent(E_Card, (PointerEventData data) => { UI_UseE(data); });
    }

    //3번 인덱스의 리스트를 반드시 사용한다.
    public void UI_UseR(PointerEventData data = null)
    {
        //UI_Card 
        UI_Card R_CardUI = R_Btn.GetComponentInChildren<UI_Card>();
        //마나 사용
        BaseCard._nowMana -= BaseCard.UseMana(null, R_CardUI).Item2;
        //사용한 카드
        string _nowCard = R_Btn.transform.GetChild(0).name;
        Debug.Log($"사용한 카드 : {_nowCard}");
        //사용한 카드 파괴
        R_CardUI.DestroyCard(0.5f);

        //새로운 카드 덱에서 리필
        R_Card = Managers.Resource.Instantiate($"Cards/{BaseCard.UseCard(_nowCard)}", R_Btn.transform);
        R_UI = R_Card.GetComponentInChildren<UI_Card>();
        R_cardimg = R_Card.transform.GetChild(1).gameObject.GetComponent<Image>();
        
        //BindEvent(R_Card, (PointerEventData data) => { UI_UseR(data); });
    }
}
