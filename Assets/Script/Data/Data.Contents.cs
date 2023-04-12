using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Data
{
    [Serializable]  //�޸𸮻��� �����͸� �о����
    public class Stat
    {
        public string name;
        public int level;   //json�� �ݿ��ؾ���

        public float moveSpeed;
        public float attackPower;
        public float attackSpeed;
        public float attackRange;

        public float defence;
        public float Hp;
        public float MaxHp; //json�� �ݿ��ؾ���
        public float HealthRecover;

        public float EnergyRecover;

        public float Range;
    }

    [Serializable]
    public class Card
    {
        public string name;
        public string cardType;
        public float damage;
    }

    [Serializable]
    public class Deck
    {
        public int id;
        public string name;
    }

    [Serializable]
    public class StatData : ILoader<string, Stat>
    {
        public List<Stat> stats = new List<Stat>();

        //json 딕셔너리 저장
        public Dictionary<string, Stat> MakeDict()
        {
            Dictionary<string, Stat> dict = new Dictionary<string, Stat>();
            foreach (Stat stat in stats)
            {
                dict.Add(stat.name, stat);
                Debug.Log(stat.name);
                Debug.Log(stat.attackPower);
            }
            return dict;
        }
    }

    [Serializable]
    public class CardData : ILoader<string, Card>
    {
        public List<Card> cards = new List<Card>();

        public Dictionary<string, Card> MakeDict()
        {
            Dictionary<string, Card> dict = new Dictionary<string, Card>();
            foreach (Card card in cards)
            {
                dict.Add(card.name, card);
                Debug.Log(card.name);
                Debug.Log(card.damage);
            }
            return dict;
        }
    }

    [Serializable]
    public class DeckData : ILoader<int, Deck>
    {
        public List<Deck> decks = new List<Deck>();

        public Dictionary<int, Deck> MakeDict()
        {
            Dictionary<int, Deck> dict = new Dictionary<int, Deck>();
            foreach (Deck deck in decks)
            {
                dict.Add(deck.id, deck);
                //Debug.Log(deck.id);
                //Debug.Log(deck.name);
            }
			return dict;
		}
    }
}
