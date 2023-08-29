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
    public class PlayerStat
    {
        public string type;

        public float basicAttackPower; //평타 공격력
        public float attackSpeed; //평타 공속
        public float attackRange; //평타 범위 
        public string attackType; //평타 타입

        public float maxHealth; //최대 체력
        public float healthRegeneration; //체력 재생량
        public float defensePower; //방어력

        public int level; //레벨
        public float experience; //경험치
        public float levelUpEX; //레벨업시 늘어나는 경험치 양
        public float levelUpAP; //레벨업시 늘어나는 평타 공격력
        public float levelUpAS; //레벨업시 늘어나는 평타 공속
        public float levelUpHP; //레벨업시 늘어나는 최대 체력
        public float levelUpHR; //레벨업시 늘어나는 체력 회복량
        public float levelUpDP; //레벨업시 늘어나는 방어력

        public float speed; //이동 속도

        public string nowState;

        public int playerArea; //내 진영
        public int enemyArea; //상대방 진영

        public float manaRegen; //마나 회복 속도
        public float maxMana; //최대 마나

        public float gold; //돈
    }


    [Serializable]
    public class ObjStat
    {
        public string type;

        public float basicAttackPower;
        public float attackSpeed;
        public float attackRange;
        public float recognitionRange;

        public float maxHealth;
        public float defensePower;
        public float nowBattery;

        public float speed;

        public float gold;
        public float experience;
    }

    [Serializable]
    public class Card
    {
        public string cardtype;
        public List<string> card;
    }

    [Serializable]
    public class Deck
    {
        public int id;
        public List<string> cards;
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
                //Debug.Log(stat.name);
                //Debug.Log(stat.attackPower);
            }
            return dict;
        }
    }

    [Serializable]
    public class PlayerStatData : ILoader<string, PlayerStat>
    {
        public List<PlayerStat> stats = new List<PlayerStat>();

        //json 딕셔너리 저장
        public Dictionary<string, PlayerStat> MakeDict()
        {
            Dictionary<string, PlayerStat> dict = new Dictionary<string, PlayerStat>();
            foreach (PlayerStat stat in stats)
            {
                dict.Add(stat.type, stat);
            }
            return dict;
        }
    }


    [Serializable]
    public class ObjStatData : ILoader<string, ObjStat>
    {
        public List<ObjStat> stats = new List<ObjStat>();

        //json 딕셔너리 저장
        public Dictionary<string, ObjStat> MakeDict()
        {
            Dictionary<string, ObjStat> dict = new Dictionary<string, ObjStat>();
            foreach (ObjStat stat in stats)
            {
                dict.Add(stat.type, stat);
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
                dict.Add(card.cardtype, card);
                //Debug.Log(card.name);
                //Debug.Log(card.damage);
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
                //Debug.Log(deck.cards.Count);
			}
			return dict;
		}
    }
}
