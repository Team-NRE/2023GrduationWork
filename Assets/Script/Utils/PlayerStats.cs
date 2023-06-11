using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Define;

namespace Stat
{
    public class PlayerStats : MonoBehaviour
    {
        #region PlayerStats

        [Header("-- 공격 --")]
        [SerializeField] private float _basicAttackPower; //평타 공격력
        [SerializeField] private float _attackSpeed; //평타 공속
        [SerializeField] private float _attackDelay; //평타 딜레이
        [SerializeField] private float _attackRange; //평타 범위 
        [SerializeField] private string _attackType; //평타 타입
        [SerializeField] private float _kill; //킬


        [Header("-- 방어 --")]
        [SerializeField] private float _nowHealth; //현재 체력
        [SerializeField] private float _maxHealth; //최대 체력
        [SerializeField] private float _healthRegeneration; //체력 재생량
        [SerializeField] private float _defensePower; //방어력
        [SerializeField] private float _death; //데스

        [Header("-- 레벨 --")]
        private int _nowlevel;
        [SerializeField] private int _level; //레벨
        [SerializeField] private float _experience; //경험치
        [SerializeField] private float _levelUpEX; //레벨업시 늘어나는 경험치 양
        [SerializeField] private float _levelUpAP; //레벨업시 늘어나는 평타 공격력
        [SerializeField] private float _levelUpAS; //레벨업시 늘어나는 평타 공속
        [SerializeField] private float _levelUpHP; //레벨업시 늘어나는 최대 체력
        [SerializeField] private float _levelUpHR; //레벨업시 늘어나는 체력 회복량
        [SerializeField] private float _levelUpDP; //레벨업시 늘어나는 방어력


        [Header("-- 이동 --")]
        [SerializeField] private float _speed; //이동 속도


        [Header("-- 현재 상태 --")]
        [SerializeField] private string _nowState;


        [Header("-- 진영 --")]
        [SerializeField] private int _playerArea; //내 진영
        [SerializeField] private int _enemyArea; //상대방 진영


        [Header("-- 마나 --")]
        [SerializeField] private float _nowMana; //현재 마나
        [SerializeField] private float _manaRegen; //마나 회복 속도
        [SerializeField] private float _maxMana; //최대 마나


        [Header("-- 자원 --")]
        [SerializeField] private float _gold; //돈

        #endregion


        //공격
        public float basicAttackPower { get { return _basicAttackPower; } set { _basicAttackPower = value; } }
        public float attackSpeed
        {
            get { return _attackSpeed; }
            set
            {
                _attackSpeed = value;
                //공격 속도 계산식
                attackDelay = (1 / _attackSpeed);
            }
        }
        public float attackDelay { get { return _attackDelay; } set { _attackDelay = value; } }
        public float attackRange { get { return _attackRange; } set { _attackRange = value; } }
        public string attackType { get { return _attackType; } set { _attackType = value; } }
        public float kill { get { return _kill; } set { _kill = value; } }



        //방어
        public float nowHealth
        {
            get { return _nowHealth; }
            set
            {
                if (value > 0)
                {
                    _nowHealth = value;
                }
                else if (value < 0)
                {
                    value *= 100 / (100 + defensePower);
                    _nowHealth = value;
                }

                if (_nowHealth >= maxHealth) _nowHealth = maxHealth;
                if (_nowHealth < 0) { death += 1; _nowHealth = 0; }
            }
        }
        public float maxHealth
        {
            get { return _maxHealth; }
            set
            {
                _maxHealth = value;
                nowHealth = _maxHealth;
            }
        }
        public float healthRegeneration
        {
            get { return _healthRegeneration; }
            set
            {
                _healthRegeneration = value * Time.deltaTime;
                nowHealth += _healthRegeneration;
            }
        }
        public float defensePower { get { return _defensePower; } set { _defensePower = value; } }
        public float death { get { return _death; } set { _death = value; } }



        //레벨
        public int level
        {
            get { return _level; }
            set
            {
                _level = value;
                if (_level != _nowlevel)
                {
                    basicAttackPower += _levelUpAP;
                    attackSpeed += _levelUpAS;
                    maxHealth += _levelUpHP;
                    healthRegeneration += _levelUpHR;
                    defensePower += _levelUpDP;
                    _nowlevel = _level;
                }
            }
        }
        public float experience
        {
            get { return _experience; }
            set
            {
                _experience += value;
                if (_experience > 50 + levelUpEx)
                {
                    level += 1;
                    levelUpEx += 20;
                    _experience = 0;
                }
            }
        }
        public float levelUpEx { get { return _levelUpEX; } set { _levelUpEX = value; } }



        //이동
        public float speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                if (GetComponent<NavMeshAgent>())
                {
                    GetComponent<NavMeshAgent>().speed = _speed;
                    GetComponent<NavMeshAgent>().acceleration = 80.0f;
                    GetComponent<NavMeshAgent>().updateRotation = false;
                }
            }
        }



        //현재 상태
        public string nowState { get { return _nowState; } set { _nowState = value; } }



        //진영
        public int playerArea
        {
            get { return _playerArea; }
            set
            {
                _playerArea = value;
                this.gameObject.layer = _playerArea;
            }
        }
        public int enemyArea { get { return _enemyArea; } set { _enemyArea = value; } }



        //마나
        public float nowMana
        {
            get { return _nowMana; }
            set
            {
                _nowMana = value;
                if (_nowMana >= maxMana * manaRegen) { _nowMana = maxMana * manaRegen; }
                if (_nowMana <= 0) { _nowMana = 0; }
            }
        }
        public float manaRegen { get { return _manaRegen; } set { _manaRegen = value; } }
        public float maxMana { get { return _maxMana; } set { _maxMana = value; } }
        //마나 사용
        public (bool, float) UseMana(string _key = null, UI_Card ui_card = null)
        {
            bool CanUseCard;

            if (_key != null) { ui_card = GameObject.Find(_key).GetComponentInChildren<UI_Card>(); }
            float cardValue = ui_card._cost * manaRegen;

            if (nowMana >= cardValue) { CanUseCard = true; }
            else { CanUseCard = false; }

            return (CanUseCard, cardValue);
        }


        //자원
        public float gold { get { return _gold; } set { _gold = value; } }


        public void PlayerStatSetting(PlayerType type)
        {
            Dictionary<string, Data.PlayerStat> dict = Managers.Data.PlayerStatDict;
            Data.PlayerStat stat = dict[type.ToString()];

            //공격
            basicAttackPower = stat.basicAttackPower;
            attackSpeed = stat.attackSpeed;
            attackRange = stat.attackRange;
            attackType = stat.attackType;
            kill = 0;

            //방어
            maxHealth = stat.maxHealth;
            nowHealth = maxHealth;
            healthRegeneration = stat.healthRegeneration;
            defensePower = stat.defensePower;
            death = 0;

            //레벨
            level = stat.level;
            _nowlevel = level;
            experience = stat.experience;
            _levelUpEX = stat.levelUpEX;
            _levelUpAP = stat.levelUpAP;
            _levelUpAS = stat.levelUpAS;
            _levelUpHP = stat.levelUpHP;
            _levelUpHR = stat.levelUpHR;
            _levelUpDP = stat.levelUpDP;

            //이동
            speed = stat.speed;

            //현재 상태
            nowState = stat.nowState;

            //진영
            playerArea = stat.playerArea;
            enemyArea = stat.enemyArea;

            //마나
            nowMana = 0; //현재 마나
            manaRegen = stat.manaRegen; //마나 회복 속도
            maxMana = stat.maxMana; //최대 마나

            //자원
            gold = stat.gold;
        }
    }
}
