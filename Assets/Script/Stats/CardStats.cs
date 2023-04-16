using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class CardStats : MonoBehaviour
{
	Data.Card _card;

	public int _cost;
    public float _damage;
    public float _defence;
    public float _debuff;
    public float _buff;


    public int cost { get { return _cost; } set { value = _cost; } }
    public float damage { get { return _damage; } set { value = _damage; } }
    public float defence { get { return _defence; } set { value = _defence; } }
    public float debuff { get { return _debuff; } set { value = _debuff; } }
    public float buff { get { return _buff; } set { value = _buff; } }
	


	public void SetCardStat(string name)
	{
		Dictionary<string, Data.Card> dict = Managers.Data.CardDict;
		Data.Card card = dict[name];

	}



	//���⼭ Dictionary�� ������ �̸��� ��ȯ
	public string FindCardName(string pickCard)
	{
		Dictionary<string, Data.Card> dict = Managers.Data.CardDict;
		Data.Card card = dict[pickCard];

		if (pickCard == card.name)
			return card.name;
		else
			return null;
	}

	//Dictionary�� ������ type�� ��ȯ
	public string FindCardType(string pickCard)
	{
		Dictionary<string, Data.Card> dict = Managers.Data.CardDict;
		Data.Card card = dict[pickCard];

		if (pickCard == card.name)
			return card.cardType;
		else
			return null;
	}
}
